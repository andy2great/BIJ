using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuNetworking : NetworkBehaviour
{
    /// <summary>
    /// For most cases, you want to register once your NetworkBehaviour's
    /// NetworkObject (typically in-scene placed) is spawned.
    /// </summary>
    public override void OnNetworkSpawn()
    {
     
        if (IsServer)
        {
            // Server broadcasts to all clients when a new client connects (just for example purposes)
            NetworkManager.OnClientConnectedCallback += OnClientConnectedCallback;
        }
        else
        {
            // Clients send a unique Guid to the server
        }
    }
    private void OnClientConnectedCallback(ulong clientId)
    {
        GameObject.Find("StatusLobby").GetComponent<TMP_Text>().text = NetworkManager.ConnectedClients.Count + " Player in lobby";
        UpdateLobbyUIClientRpc(NetworkManager.ConnectedClients.Count);
        Debug.Log("PATATE");
    }
    [ClientRpc]
    void UpdateLobbyUIClientRpc(int playercount)
    {
        GameObject.Find("StatusLobby").GetComponent<TMP_Text>().text = playercount + " Player in lobby";

    }

    public void LaunchGame(string Scene)
    {
        if (IsHost)
        {
            var status = NetworkManager.SceneManager.LoadScene(Scene,LoadSceneMode.Single);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load {Scene} " +
                      $"with a {nameof(SceneEventProgressStatus)}: {status}");
            }
        }
    }
}
