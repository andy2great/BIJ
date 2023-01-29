using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Psychedelic : OnScreenEffect
{
    public override string Name => "Psychedelic";
    protected override Material Material => Resources.Load("Materials/Effects/PsychedelicMaterial", typeof(Material)) as Material;
}
