using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject MPChoice;
    public GameObject JoinIP;
    public GameObject HostLobby;
    public GameObject MapSelection;
    public GameObject OptionMenu;


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
        //TODO: LAUNCH THE SERVER
        MPChoice.SetActive(false);
        HostLobby.SetActive(true);

    }

    public void MPChoiceJoin()
    {
        MPChoice.SetActive(false);
        JoinIP.SetActive(true);

    }
    public void JoinIPJoin()
    {
        // TODO: Take the ip put it in the networkmanager, if they are an error
        // put the error in the ErrorLogin.text, if not show the next menu

        var input = GameObject.Find("IP").GetComponent<TMP_InputField>();
        var Error = GameObject.Find("ErrorLogin").GetComponent<TMP_Text>();
        // input.text to get the IP


        JoinIP.SetActive(false);
        HostLobby.SetActive(true);
    }

    public void HostLobbyLaunchGame()
    {
        //TODO: Connect the StatusLobby text to the ammount of player in the lobby
        HostLobby.SetActive(false);
        MapSelection.SetActive(true);
    }
    public void MapSelectionGame(int id)
    {
        MapSelection.SetActive(false);
        // TODO: LAUNCH LA BONNE MAP
        switch (id)
        {
            case 1:
                SceneManager.LoadScene("Niveaux poutine");
                break;
            case 2:
                SceneManager.LoadScene("Niveaux patate");

                break;
            case 3:
                SceneManager.LoadScene("Niveaux celine dion");

                break;
        }

    }

    public void Quit() {
        Application.Quit();
    }
}
