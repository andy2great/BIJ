using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnScreenEffect : BaseEffect
{
    protected abstract Material Material { get; }
    protected OnScreenEffectController _onScreenEffectController;

    void Start()
    {
        _onScreenEffectController = GameObject
            .FindWithTag("OnScreenEffectController")
            .GetComponent<OnScreenEffectController>() as OnScreenEffectController;
    }

    public override IEnumerator ApplyEffect()
    {
        Debug.Log(Name);
        _onScreenEffectController.AddMaterial(Material);

        yield return null;
    }

    public override void RemoveEffect()
    {
        _onScreenEffectController.RemoveMaterial(Material);
    }
}
