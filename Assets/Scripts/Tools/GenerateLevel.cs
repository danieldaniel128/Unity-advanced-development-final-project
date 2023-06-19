using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateLevel : MonoBehaviour
{
    public int Width;// { get; private set; }
    public int Height;// { get; private set; }
    int[,] TileMap;
    int[] TileMapHeights;

    //public Dictionary<int, GameObject> TileObject;

    public GameObject GroundPrefab;
    public Transform LevelParent;

    [ContextMenu("Generate Level")]
    void CreateLevel()
    {
        TileMap = new int[Width, Height];
        TileMapHeights = new int[Width];
        //Debug.Log($"Width: {Width}");
        //Debug.Log($"Height: {Height}");
        for (int x = 0; x < Width; x++)
        {
            float num1 = Random.Range(289, 10000);
            int perlinNoiceHeight = Mathf.RoundToInt(Mathf.PerlinNoise((x + 1) * Width * Height * num1, num1 * 2 / 7) * Height/2);
            Debug.Log($"<color=red>perlinNoiceHeight: {perlinNoiceHeight}</color>");
            TileMapHeights[x] = perlinNoiceHeight;
        }

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (y < TileMapHeights[x])
                { 
                    TileMap[x, y] = 0;
                    Instantiate(GroundPrefab,new Vector3(x,y),Quaternion.identity, LevelParent);
                }
                //TileMap[x,y] = TileMapHeights.Where(a=>TileObject.Where(b=>b.Key == a))
            }
        }

    }
}
