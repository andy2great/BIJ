using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public List<GameObject> ItemsToSpawn = new List<GameObject>();
    public int SpawnTime = 10;
    public int MaxItems = 5;
    public string ItemTag = "Item";

    private int _startX = 0;
    private int _endX = 0;
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

    IEnumerator SpawnItems()
    {
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
            int randomX = Random.Range(_startX, _endX);
            Vector3 spawnPosition = new Vector3(randomX, _droppingPoint, 0);
            Instantiate(ItemsToSpawn[randomItem], spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(randomTime);
        }
        
    }
}
