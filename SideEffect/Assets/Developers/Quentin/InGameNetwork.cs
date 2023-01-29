using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class InGameNetwork : NetworkBehaviour
{
    public GameObject PlayerPrefab;
    public List<GameObject> Spawn;
    // Start is called before the first frame update
    void Start()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds) 
            {
                int random = Random.Range(0, Spawn.Count);
                GameObject player = Instantiate(PlayerPrefab);
                player.transform.position = Spawn[random].transform.position;
                player.GetComponent<NetworkObject>().SpawnWithOwnership(uid);
                // ici possible erreur avec SpawnWithOwnership pour faire bouger le client
            }
        }
    }


}
