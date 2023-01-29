using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEffect : MonoBehaviour {
  public abstract string Name { get; }
  public int Stage { get; set; } = 1;

  public abstract IEnumerator ApplyEffect();
  public abstract IEnumerator RemoveEffect();
}