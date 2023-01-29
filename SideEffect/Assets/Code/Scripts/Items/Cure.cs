using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cure : BaseItem
{
  public override string Name { get; set; } = "Cure";
  public override int Damage { get; set; } = 0;
  public override int Health { get; set; } = 0;
  public override int XSpeedFactor { get; set; } = 1;
  public override int YSpeedFactor { get; set; } = 1;
  public override float Spin { get; set; } = 0.5f;

  public override Type[] EffectTypes => new Type[0];
}
