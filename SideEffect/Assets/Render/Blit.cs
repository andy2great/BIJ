using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Cyan {

	public class Blit : ScriptableRendererFeature
    {
		public BlitSettings settings = new BlitSettings();
		public Dictionary<Material, BlitPass> blitPasses = new Dictionary<Material, BlitPass>();

		public override void Create()
		{
			Editor.stop += Destroy;
		}

        public void AddMaterial(Material material, BlitSettings? blitSettings = null)
        {
            var settings = blitSettings?.Copy() ?? this.settings.Copy();

            var passIndex = material != null ? material.passCount - 1 : 1;
			settings.blitMaterialPassIndex = Mathf.Clamp(settings.blitMaterialPassIndex, -1, passIndex);
			var blitPass = new BlitPass(settings.Event, settings, material, name);

            blitPasses.Add(material, blitPass);
        }

        public void RemoveMaterial(Material material)
        {
            blitPasses.Remove(material);
        }

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
            if (blitPasses.Count == 0) return;

            foreach (var keyValue in blitPasses)
            {
                var blitPass = keyValue.Value;

                if (blitPass.settings.graphicsFormat == UnityEngine.Experimental.Rendering.GraphicsFormat.None) {
                    blitPass.settings.graphicsFormat = SystemInfo.GetGraphicsFormat(UnityEngine.Experimental.Rendering.DefaultFormat.LDR);
                }

                blitPass.Setup(renderer);
                renderer.EnqueuePass(blitPass);
            }
		}

		protected void Destroy()
		{
            blitPasses = new Dictionary<Material, BlitPass>();
		}
	}

    public enum Target
    {
        CameraColor,
        TextureID,
        RenderTextureObject
    }

    [System.Serializable]
    public class BlitSettings
    {
        public RenderPassEvent Event = RenderPassEvent.AfterRenderingOpaques;

        public int blitMaterialPassIndex = 0;
        public bool setInverseViewMatrix = false;
        public bool requireDepthNormals = false;

        public Target srcType = Target.CameraColor;
        public string srcTextureId = "_CameraColorTexture";
        public RenderTexture srcTextureObject;

        public Target dstType = Target.CameraColor;
        public string dstTextureId = "_BlitPassTexture";
        public RenderTexture dstTextureObject;

        public bool overrideGraphicsFormat = false;
        public UnityEngine.Experimental.Rendering.GraphicsFormat graphicsFormat;

        public BlitSettings Copy()
        {
            return new BlitSettings
            {
                Event = Event,
                blitMaterialPassIndex = blitMaterialPassIndex,
                setInverseViewMatrix = setInverseViewMatrix,
                requireDepthNormals = requireDepthNormals,
                srcType = srcType,
                srcTextureId = srcTextureId,
                srcTextureObject = srcTextureObject,
                dstType = dstType,
                dstTextureId = dstTextureId,
                dstTextureObject = dstTextureObject,
                overrideGraphicsFormat = overrideGraphicsFormat,
                graphicsFormat = graphicsFormat,
            };
        }
    }

	public class BlitPass : ScriptableRenderPass {

		public Material blitMaterial = null;
		public FilterMode filterMode { get; set; }

		public BlitSettings settings;

		private RenderTargetIdentifier source { get; set; }
		private RenderTargetIdentifier destination { get; set; }

		RenderTargetHandle m_TemporaryColorTexture;
		RenderTargetHandle m_DestinationTexture;
		string m_ProfilerTag;

#if !UNITY_2020_2_OR_NEWER // v8
		private ScriptableRenderer renderer;
#endif

		public BlitPass(RenderPassEvent renderPassEvent, BlitSettings settings, Material material, string tag) {
			this.renderPassEvent = renderPassEvent;
			this.settings = settings;
			blitMaterial = material;
			m_ProfilerTag = tag;
			m_TemporaryColorTexture.Init("_TemporaryColorTexture");
			if (settings.dstType == Target.TextureID) {
				m_DestinationTexture.Init(settings.dstTextureId);
			}
		}

		public void Setup(ScriptableRenderer renderer) {
#if UNITY_2020_2_OR_NEWER // v10+
			if (settings.requireDepthNormals)
				ConfigureInput(ScriptableRenderPassInput.Normal);
#else // v8
			this.renderer = renderer;
#endif
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
			CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);
			RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
			opaqueDesc.depthBufferBits = 0;

			// Set Source / Destination
#if UNITY_2020_2_OR_NEWER // v10+
			var renderer = renderingData.cameraData.renderer;
#else // v8
			// For older versions, cameraData.renderer is internal so can't be accessed. Will pass it through from AddRenderPasses instead
			var renderer = this.renderer;
#endif

			// note : Seems this has to be done in here rather than in AddRenderPasses to work correctly in 2021.2+
			if (settings.srcType == Target.CameraColor) {
				source = renderer.cameraColorTarget;
			} else if (settings.srcType == Target.TextureID) {
				source = new RenderTargetIdentifier(settings.srcTextureId);
			} else if (settings.srcType == Target.RenderTextureObject) {
				source = new RenderTargetIdentifier(settings.srcTextureObject);
			}

			if (settings.dstType == Target.CameraColor) {
				destination = renderer.cameraColorTarget;
			} else if (settings.dstType == Target.TextureID) {
				destination = new RenderTargetIdentifier(settings.dstTextureId);
			} else if (settings.dstType == Target.RenderTextureObject) {
				destination = new RenderTargetIdentifier(settings.dstTextureObject);
			}

			if (settings.setInverseViewMatrix) {
				Shader.SetGlobalMatrix("_InverseView", renderingData.cameraData.camera.cameraToWorldMatrix);
			}

			if (settings.dstType == Target.TextureID) {
				if (settings.overrideGraphicsFormat) {
					opaqueDesc.graphicsFormat = settings.graphicsFormat;
				}
				cmd.GetTemporaryRT(m_DestinationTexture.id, opaqueDesc, filterMode);
			}

			//Debug.Log($"src = {source},     dst = {destination} ");
			// Can't read and write to same color target, use a TemporaryRT
			if (source == destination || (settings.srcType == settings.dstType && settings.srcType == Target.CameraColor)) {
				cmd.GetTemporaryRT(m_TemporaryColorTexture.id, opaqueDesc, filterMode);
				Blit(cmd, source, m_TemporaryColorTexture.Identifier(), blitMaterial, settings.blitMaterialPassIndex);
				Blit(cmd, m_TemporaryColorTexture.Identifier(), destination);
			} else {
				Blit(cmd, source, destination, blitMaterial, settings.blitMaterialPassIndex);
			}

			context.ExecuteCommandBuffer(cmd);
			CommandBufferPool.Release(cmd);
		}

		public override void FrameCleanup(CommandBuffer cmd) {
			if (settings.dstType == Target.TextureID) {
				cmd.ReleaseTemporaryRT(m_DestinationTexture.id);
			}
			if (source == destination || (settings.srcType == settings.dstType && settings.srcType == Target.CameraColor)) {
				cmd.ReleaseTemporaryRT(m_TemporaryColorTexture.id);
			}
		}
	}
}