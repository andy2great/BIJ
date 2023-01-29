using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medecine : BaseItem
{
  public override string Name { get; set; } = "Cure";
  public override int Damage { get; set; } = 0;
  public override int Health { get; set; } = 0;
  public override float XSpeedFactor { get; set; } = 1f;
  public override float YSpeedFactor { get; set; } = 1f;
  public override float Spin { get; set; } = 0.5f;

  public override Type[] EffectTypes => new Type[0];
}
