using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BloodSpatter : OnScreenEffect
{
    public override string Name => "BloodSpatter";
    protected override Material Material => Resources.Load("Materials/Effects/BloodMaterial", typeof(Material)) as Material;
}

