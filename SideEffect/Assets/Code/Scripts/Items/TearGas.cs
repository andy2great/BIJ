using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearGas : BaseItem
{
  public override string Name { get; set; } = "TearGas";
  public override int Damage { get; set; } = 10;
  public override int Health { get; set; } = 1;
  public override float XSpeedFactor { get; set; } = 0.5f;
  public override float YSpeedFactor { get; set; } = 4f;
  public override float Spin { get; set; } = 1.5f;

  public override Type[] EffectTypes => new Type[] { typeof(Blinking) };
}
