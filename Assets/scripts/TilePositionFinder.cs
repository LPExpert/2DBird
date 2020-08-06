using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePositionFinder : MonoBehaviour
{
    public Tile wormSpawn1;
    public Tile wormSpawn2;
    public Tile wormSpawn3;
    List<Vector3> spawnList;
    Tilemap tilemap;
    Vector3[] spawnPositions;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        spawnList = new List<Vector3>();

        for (int n = tilemap.cellBounds.xMin; n < tilemap.cellBounds.xMax; n++)
        {
            for (int p = tilemap.cellBounds.yMin; p < tilemap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)tilemap.transform.position.y));
                Vector3 place = tilemap.CellToWorld(localPlace);
                if (tilemap.HasTile(localPlace) && (tilemap.GetTile(localPlace).Equals(wormSpawn1) || tilemap.GetTile(localPlace).Equals(wormSpawn2) || tilemap.GetTile(localPlace).Equals(wormSpawn3)))
                {
                    //Tile at "place"
                    spawnList.Add(place);
                }
                else
                {
                    //No tile at "place"
                }
            }
        }

        spawnPositions = spawnList.ToArray();
        WormSpawner.instance.setSpawnPositions(spawnPositions);
        //Debug.Log(spawnPositions[2]);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
