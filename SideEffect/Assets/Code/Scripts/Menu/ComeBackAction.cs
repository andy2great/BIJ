using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.VisualScripting;

public class ComeBackAction : MonoBehaviour
{
    public string wtf { get; set;}

    public void OnButtonClickEvent() {
        // load the scene spooky in the scene manager
        SceneManager.LoadScene("Spooky", LoadSceneMode.Single);

    }
}
