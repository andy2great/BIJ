using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.VisualScripting;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject MPChoice;
    public GameObject JoinIP;
    public GameObject HostLobby;
    public GameObject MapSelection;
    public GameObject OptionMenu;
    UnityTransport Transport;
    public ushort Port;

    void Start()
    {
        MainMenu.SetActive(true);
        MPChoice.SetActive(false);
        JoinIP.SetActive(false);
        HostLobby.SetActive(false);
        MapSelection.SetActive(false);  
        OptionMenu.SetActive(false);
    }



    public void MainMenuPlay()
    {
        MainMenu.SetActive(false);
        MPChoice.SetActive(true);

    }
    public void MainMenuOption()
    {
        // TODO: DO an option menu or replace it with a menu to show all the bind and what does each weapon does
        MainMenu.SetActive(false);
        OptionMenu.SetActive(true);
    }

    public void MpChoiceHost()
    {
        NetworkManager.Singleton.StartHost();

        MPChoice.SetActive(false);
        HostLobby.SetActive(true);
        GameObject.Find("Client").SetActive(false);
        GameObject.Find("Server").SetActive(true);


    }

    public void MPChoiceJoin()
    {
        MPChoice.SetActive(false);
        JoinIP.SetActive(true);

    }
    public void JoinIPJoin()
    {
        //TODO: IF an errror while connection show it
        var input = GameObject.Find("IP").GetComponent<TMP_InputField>();
        var Error = GameObject.Find("ErrorLogin").GetComponent<TMP_Text>();

        Transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        Transport.SetConnectionData(input.text, 7777);
        NetworkManager.Singleton.StartClient();


        JoinIP.SetActive(false);
        HostLobby.SetActive(true);
        GameObject.Find("Client").SetActive(true);
        GameObject.Find("Server").SetActive(false);

    }

    public void HostLobbyLaunchGame()
    {
        HostLobby.SetActive(false);
        MapSelection.SetActive(true);
    }
    public void MapSelectionGame(int id)
    {
        MapSelection.SetActive(false);

        switch (id)
        {
            case 1:
                gameObject.GetComponent<MenuNetworking>().LaunchGame("Spooky");
                break;
            case 2:
                break;
            case 3:
                break;
        }

    }

    public void Quit() {
        Application.Quit();
    }
}
