using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldOnPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // stay still until the scene is not the main menu
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Menu")
        {
            Time.timeScale = 1;
        }
    }
}
