using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnItem : MonoBehaviour
{
    public List<GameObject> ItemsToSpawn = new List<GameObject>();
    public int SpawnTime = 5;
    public int MaxItems = 10;
    public string ItemTag = "Item";

    private float _startX = 0f;
    private float _endX = 0f;
    private int _droppingPoint = 0;
    
    void Start()
    {
        _startX = (int)gameObject.transform.position.x - ((int)gameObject.GetComponent<SpriteRenderer>().bounds.size.x/2);
        _endX = _startX + (int)gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        _droppingPoint = (int)gameObject.transform.position.y;
        StartCoroutine(SpawnItems());
    }

    // Update is called once per frame
    void Update()
    {
    }

    [ServerRpc]
    IEnumerator SpawnItems()
    {
        try {
            if (!NetworkManager.Singleton.IsServer)
            {
                yield break;
            }
        } catch {
            Debug.Log("No NetworkManager found");
        }

        while (true)
        {
            GameObject[] items = GameObject.FindGameObjectsWithTag(ItemTag);
            if (items.Length >= MaxItems)
            {
                yield return new WaitForSeconds(1);
                continue;
            }

            int randomTime = Random.Range(SpawnTime-5, SpawnTime+5);
            // spawn an item
            int randomItem = Random.Range(0, ItemsToSpawn.Count);
            var randomX = Random.Range(_startX, _endX);
            Vector3 spawnPosition = new Vector3(randomX, _droppingPoint, 0);


            try {
                var go = Instantiate(ItemsToSpawn[randomItem], spawnPosition, Quaternion.identity);
                go.GetComponent<NetworkObject>().Spawn();
            } catch {
                Debug.Log("No NetworkObject found");
            }

            yield return new WaitForSeconds(randomTime);
        }
        
    }
}
