using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class PerlinNoiseLevelGenerator : MonoBehaviour
{

    private int _width = 0;
    private int _height;
    [SerializeField] private int _platformMinHeight;
    [SerializeField] private int _platformMaxHeight;
    
    [SerializeField] private int _platformMinWidth;
    [SerializeField] private int _platformMaxWidth;

    [SerializeField] private int _numberOfPlatforms;

    [SerializeField] private Transform PlatformFather;
 
    private int _numOfSpikes;

    int[,] TileMap;
    int[] TileMapPlatformsHeights;

    private int xCounter = 0;

    #region platform prefabs 
    public GameObject TopCornerLeftPrefab;
    public GameObject TopCornerMidPrefab;
    public GameObject TopCornerRightPrefab;
    public GameObject MidLeftPrefab;
    public GameObject MidRightPrefab;
    public GameObject BottomCornerMidPrefab;
    public GameObject BottomCornerRightPrefab;
    public GameObject BottomCornerLeftPrefab;
    public GameObject MidPrefab;
    #endregion


    //public Dictionary<int, GameObject> TileObject;

    public Transform LevelParent;

    public EdgeCollider2D EdgeCollider;

    List<Vector2> EdgeColliderPoints;

    private void Start()
    {
        CreateLevel();
    }

    //private void CalculateMapWidth()
    //{
    //    for (int i = 0; i < _numberOfPlatforms; i++)
    //    {
    //        _width += Random.Range(_platformMinWidth,_platformMaxWidth+1);
    //    }
    //    _width += 2 * _numOfSpikes;
    //}

    [ContextMenu("Generate Level")]
    void CreateLevel()
    {
        for (int x = 0; x < _numberOfPlatforms; x++)
        {
            float num1 = Random.Range(289, 10000);
            float perlinNoiseValue = Mathf.PerlinNoise((x + 1) * _width * _height * num1, num1 * 2 / 7);
            int heightBetweenMinAndMax = Random.Range(_platformMinHeight, _platformMaxHeight);
            int perlinNoiceHeight = Mathf.RoundToInt(perlinNoiseValue * heightBetweenMinAndMax);
            int platformWidth = Random.Range(_platformMinWidth, _platformMaxWidth + 1);
            Debug.Log($"platformWidth {platformWidth} perlinNoiceHeight: {perlinNoiceHeight}");
            CreatePlatform(platformWidth,perlinNoiceHeight);
            xCounter += 2;
            _width += 2;
        }

    }
    [ContextMenu("generate platform test")]
    void CreatePlatform(int platformWidth , int platformHeight)
    {
        _width += platformWidth;
        platformWidth = _width;
        Transform platform = Instantiate(PlatformFather);
        Debug.Log($"<color=blue>platformWidth {platformWidth}</color>");
        int startXGenerating = xCounter;
        for (int x = startXGenerating; x < platformWidth; x++)
        {
            for (int y = 0; y < platformHeight; y++)
            {
                if (x== startXGenerating && y==0)//.<
                    Instantiate(BottomCornerLeftPrefab, new Vector3(x - 7.5f, y - 4.5f), Quaternion.identity, platform.transform);
                else if(x == platformWidth - 1 && y == platformHeight - 1)//^>
                    Instantiate(TopCornerRightPrefab, new Vector3(x - 7.5f, y - 4.5f), Quaternion.identity, platform.transform);
                else if(x== startXGenerating && y == platformHeight -1)//<^
                    Instantiate(TopCornerLeftPrefab, new Vector3(x - 7.5f, y - 4.5f), Quaternion.identity, platform.transform);
                else if (x == platformWidth -1 && y == 0)//.>
                    Instantiate(BottomCornerRightPrefab, new Vector3(x - 7.5f, y - 4.5f), Quaternion.identity, platform.transform);
                else if (x == startXGenerating)//<
                    Instantiate(MidLeftPrefab, new Vector3(x - 7.5f, y - 4.5f), Quaternion.identity, platform.transform);
                else if (x == platformWidth - 1)//>
                    Instantiate(MidRightPrefab, new Vector3(x - 7.5f, y - 4.5f), Quaternion.identity, platform.transform);
                else if(y == platformHeight - 1)//^
                    Instantiate(TopCornerMidPrefab, new Vector3(x - 7.5f, y - 4.5f), Quaternion.identity, platform.transform);
                else if (y == 0)//.
                    Instantiate(BottomCornerMidPrefab, new Vector3(x - 7.5f, y - 4.5f), Quaternion.identity, platform.transform);
                else
                    Instantiate(MidPrefab, new Vector3(x - 7.5f, y - 4.5f), Quaternion.identity, platform.transform);
                Debug.Log($"<color=red>x: {x} y: {y}</color>");
            }
            xCounter++;
        }
    }


}
