using System.Collections;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public TMPro.TMP_Text count;
    public IEnumerator CountDown() {
        yield return new WaitForSecondsRealtime(1f);
        count.text = "2";
        yield return new WaitForSecondsRealtime(1f);
        count.text = "1";
        yield return new WaitForSecondsRealtime(1f);
        count.text = "0";
        yield return new WaitForSecondsRealtime(0.5f);
    } 
}
