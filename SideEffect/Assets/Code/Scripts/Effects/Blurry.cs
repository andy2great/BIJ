using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Blurry : OnScreenEffect
{
    public override string Name => "Blurry";
    protected override Material Material => Resources.Load("Materials/Effects/BlurryMaterial", typeof(Material)) as Material;
}

