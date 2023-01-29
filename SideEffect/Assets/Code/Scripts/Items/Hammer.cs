using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : BaseItem
{
  public override string Name { get; set; } = "Hammer";
  public override int Damage { get; set; } = 5;
  public override int Health { get; set; } = 3;
  public override float XSpeedFactor { get; set; } = 1f;
  public override float YSpeedFactor { get; set; } = 1f;
  public override float Spin { get; set; } = 0.75f;

  public override Type[] EffectTypes => new Type[] { typeof(TrippyCamera) };
}