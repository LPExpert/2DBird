using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WormSpawner : MonoBehaviour
{
    public static WormSpawner instance { get; private set; }

    private Vector3[] spawnPositions;
    public Object worm;
    int maxNumber = 10;
    int wormNumber;
    float spawnDelay = 0.1f;
    float spawnDelayTimer;

    // Start is called before the first frame update
    void Start()
    {
        wormNumber = 0;
        spawnDelayTimer = spawnDelay;
        //Instantiate(worm, new Vector2(1.5f,-1.375f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if(wormNumber < maxNumber && spawnDelayTimer == spawnDelay)
        {
            Worm[] activeWorms;
            bool available;
            Vector3 spawn;
            do
            {
                available = true;
                spawn = spawnPositions[Mathf.RoundToInt(Random.value * (spawnPositions.Length - 1))];
                activeWorms = GameObject.FindObjectsOfType<Worm>();
                foreach(Worm w in activeWorms)
                {
                    if (w.transform.position == spawn + new Vector3(0.5f, 0.625f))
                    {
                        available = false;
                    }
                }

            } while(!available);

            Instantiate(worm, spawn + new Vector3(0.5f, 0.625f), Quaternion.identity);
            spawnTimer();
            wormNumber++;
        }

        if(spawnDelayTimer != spawnDelay)
        {
            spawnTimer();
        }
    }

    private void Awake()
    {
        instance = this;
    }
    public void setSpawnPositions(Vector3[] spawnArray)
    {
        spawnPositions = spawnArray;
    }

    void spawnTimer()
    {
        if(isSpawnReady())
        {
            spawnDelayTimer = spawnDelay;
        }
        else
        {
            spawnDelayTimer -= Time.deltaTime;
        }
    }

    bool isSpawnReady()
    {
        return spawnDelayTimer - Time.deltaTime <= 0;
    }

    public void wormDestroyed()
    {
        wormNumber--;
    }

    
}
