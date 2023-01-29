using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Microscope : BaseItem
{
  public override string Name { get; set; } = "Microscope";
  public override int Damage { get; set; } = 3;
  public override int Health { get; set; } = 2;
  public override float XSpeedFactor { get; set; } = 1f;
  public override float YSpeedFactor { get; set; } = 0.8f;
  public override float Spin { get; set; } = 0.25f;

  public override Type[] EffectTypes => new Type[] { typeof(GrayOut) };
}
