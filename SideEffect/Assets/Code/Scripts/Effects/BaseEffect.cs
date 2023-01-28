using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEffect {
  public abstract string Name { get; set; }
  public int Stage { get; set; } = 1;

  public abstract IEnumerator ApplyEffect();
  public abstract void RemoveEffect();
}