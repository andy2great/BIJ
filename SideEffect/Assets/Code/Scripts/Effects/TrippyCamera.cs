using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrippyCamera : BaseEffect {
  public override string Name => "Trippy Camera";

  private bool _rotating = false;
  
  public override IEnumerator ApplyEffect()
  {
    _rotating = true;

    while (true)
    {
      if (!_rotating) break;

      var cameraRotation = Camera.main.transform.rotation;
      Camera.main.transform.Rotate(0, 0, 1 * Stage);
      yield return new WaitForSeconds(0.05f);
    }
  }

  public override IEnumerator RemoveEffect()
  {
    _rotating = false;
    Camera.main.transform.Rotate(0, 0, 0);

    yield return null;
  }
}