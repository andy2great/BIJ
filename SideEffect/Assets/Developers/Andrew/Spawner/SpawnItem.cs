using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public List<GameObject> itemsToSpawn = new List<GameObject>();
    public int spawnTime = 10;

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
        // spawn an item each 10 seconds
    }

    // spawn items
    IEnumerator SpawnItems()
    {
        while (true)
        {
            int randomTime = Random.Range(spawnTime-5, spawnTime+5);
            // spawn an item
            int randomItem = Random.Range(0, itemsToSpawn.Count);
            int randomX = Random.Range(_startX, _endX);
            Vector3 spawnPosition = new Vector3(randomX, _droppingPoint, 0);
            Instantiate(itemsToSpawn[randomItem], spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(randomTime);
        }
        
    }
}
