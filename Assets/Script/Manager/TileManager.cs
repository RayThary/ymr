using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
#if UNITY_EDITOR
    public GameObject[,] tiles;
    public GameObject edit;
    public Transform parent;
    public int minX;
    public int maxX;
    public int minY;
    public int maxY;
    private int layer = 1 << 7 | 1 << 8 /* 1 << 7 | 1 << 8 */;
    public int Layer { get { return layer; } }

    public GameObject CreateTile(GameObject obj)
    {
        return Instantiate(obj, parent);
    }
    public void CreateTile(int x, int y)
    {
        if (tiles[x,y] != null)
        {
            DeleteTile(tiles[x, y]);
        }
        tiles[x, y] = CreateTile(edit);
        tiles[x, y].transform.position = new Vector3(x, 0, y);
        tiles[x, y].transform.name = "Tile {" + x + "," + y +"}";
    }
    public void CreateTiles()
    {
        for(int i = minX; i < maxX; i++)
        {
            for(int j = minY; j < maxY; j++)
            {
                if (tiles[i, j] != null)
                {
                    DeleteTile(tiles[i, j]);
                }
                tiles[i, j] = CreateTile(edit);
                tiles[i, j].transform.position = new Vector3(i, 0, j);
                tiles[i, j].transform.name = "Tile {" + i + "," + j + "}";
            }
        }
    }

    public void DeleteTiles()
    {
        foreach (var tile in tiles)
        {
            if(!tile.CompareTag("Tile"))
            {
                DestroyImmediate(tile);
            }
        }
    }
    public void DeleteTile(GameObject obj)
    {
        DestroyImmediate(obj);
    }
#endif



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
