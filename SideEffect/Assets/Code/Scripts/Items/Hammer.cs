using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : BaseItem {
  public override string Name { get; set; }
  public override int Damage { get; set; }
  public override int Health { get; set; }

  public Hammer() {
    Name = "Hammer";
    Damage = 1;
    Health = 1;
  }

  public override void ApplyEffect() {
    Debug.Log("Hammer hit!");
  }
}