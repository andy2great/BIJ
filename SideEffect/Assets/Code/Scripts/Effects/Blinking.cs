using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Blinking : OnScreenEffect
{
    public override string Name => "Blinking";
    protected override Material Material => Resources.Load("Materials/Effects/BlinkingMaterial", typeof(Material)) as Material;
}

