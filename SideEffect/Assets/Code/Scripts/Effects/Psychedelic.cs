using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class Psychedelic : OnScreenEffect
{
    public override string Name => "Psychedelic";
    public override Material Material => Resources.Load("Materials/Effects/PsychedelicMaterial", typeof(Material)) as Material;

    private OnScreenEffectController _onScreenEffectController;

    void Start()
    {
        _onScreenEffectController = GameObject
            .FindWithTag("OnScreenEffectController")
            .GetComponent<OnScreenEffectController>() as OnScreenEffectController;
    }

    public override IEnumerator ApplyEffect()
    {
        _onScreenEffectController.AddMaterial(Material);

        yield return null;
    }

    public override void RemoveEffect()
    {
        _onScreenEffectController.RemoveMaterial(Material);
    }
}
