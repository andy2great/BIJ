using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrugNeedle : BaseItem
{
  public override string Name { get; set; } = "DrugNeedle";
  public override int Damage { get; set; } = 3;
  public override int Health { get; set; } = 1;
  public override float XSpeedFactor { get; set; } = 2f;
  public override float YSpeedFactor { get; set; } = 0.5f;
  public override float Spin { get; set; } = 0f;

  public override Type[] EffectTypes => new Type[] { typeof(Psychedelic) };
}
