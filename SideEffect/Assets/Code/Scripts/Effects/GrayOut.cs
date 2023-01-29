using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GrayOut : OnScreenEffect
{
    public override string Name => "GrayOut";
    protected override Material Material => Resources.Load("Materials/Effects/GrayOutMaterial", typeof(Material)) as Material;
}

