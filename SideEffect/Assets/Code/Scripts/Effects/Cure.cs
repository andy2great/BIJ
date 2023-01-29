using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cure : BaseEffect
{
    public override string Name => "Cure";

    public override IEnumerator ApplyEffect()
    {
        yield return null;
    }

    public override IEnumerator RemoveEffect()
    {
        yield return null;
    }
}
