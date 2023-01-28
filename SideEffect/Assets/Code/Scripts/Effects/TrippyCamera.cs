using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrippyCamera: BaseEffect {
  public override string Name { get; set; } = "Trippy Camera";
  
  public override IEnumerator ApplyEffect() {
    while (true) {
      var cameraRotation = Camera.main.transform.rotation;
      Camera.main.transform.Rotate(0, 0, (cameraRotation.z + 1) % 360);
      yield return new WaitForSeconds(0.01f - (0.0001f * Stage));
    }
  }

  public override void RemoveEffect() {
    Camera.main.transform.Rotate(0, 0, 0);
  }
}