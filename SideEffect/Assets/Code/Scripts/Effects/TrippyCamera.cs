using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrippyCamera: BaseEffect {
  public override string Name { get; set; } = "Trippy Camera";
  
  public override IEnumerator ApplyEffect() {
    while (true) {
      var cameraRotation = Camera.main.transform.rotation;
      Camera.main.transform.Rotate(0, 0, 1 * Stage);
      yield return new WaitForSeconds(0.05f);
    }
  }

  public override void RemoveEffect() {
    Camera.main.transform.Rotate(0, 0, 0);
  }
}