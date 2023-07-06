using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [SerializeField] private int _minSpikesBetweenPlatforms;
    [SerializeField] private int _maxSpikesBetweenPlatforms;

    private Transform _lastGeneratedPlatform;
    private Transform _lastGeneratedSpikes;


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
    [SerializeField] GameObject _spikesPrefab;

    //public Dictionary<int, GameObject> TileObject;

    public Transform LevelParent;

    [SerializeField] string _platformTag;
    [SerializeField] string _platformLayer;

    [SerializeField] string _obstacleTag;

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
            float perlinNoiseValue = Mathf.PerlinNoise(0, (x + 1) * _width * _height * num1);
            int heightBetweenMinAndMax = Random.Range(_platformMinHeight, _platformMaxHeight);
            int perlinNoiceHeight = Mathf.RoundToInt(perlinNoiseValue * heightBetweenMinAndMax);
            int platformWidth = Random.Range(_platformMinWidth, _platformMaxWidth + 1);
            Debug.Log($"platformWidth {platformWidth} perlinNoiceHeight: {perlinNoiceHeight}");
            CreatePlatform(platformWidth,perlinNoiceHeight);
            CreateSpikes();
        }

    }
    [ContextMenu("generate platform test")]
    void CreatePlatform(int platformWidth , int platformHeight)
    {
        //_width += platformWidth;
        //platformWidth = _width;
        _lastGeneratedPlatform = Instantiate(PlatformFather, Vector3.zero,Quaternion.identity);
        LayerMask platformLayer = LayerMask.NameToLayer(_platformLayer);
        _lastGeneratedPlatform.gameObject.layer = platformLayer;
        _lastGeneratedPlatform.tag = _platformTag;
        //Debug.Log($"<color=blue>platformWidth {platformWidth} , platformHeight {platformHeight}</color>");
        int startXGenerating = xCounter;
        int stopXGenerating = platformWidth + _width;
        for (int x = startXGenerating; x < stopXGenerating; x++)
        {
            for (int y = 0; y < platformHeight; y++)
            {
                if (x== startXGenerating && y==0)//.<
                    Instantiate(BottomCornerLeftPrefab, new Vector3(x - 7.5f, y - 4.5f), Quaternion.identity, _lastGeneratedPlatform);
                else if(x == stopXGenerating - 1 && y == platformHeight - 1)//^>
                    Instantiate(TopCornerRightPrefab, new Vector3(x - 7.5f, y - 4.5f), Quaternion.identity, _lastGeneratedPlatform);
                else if(x== startXGenerating && y == platformHeight -1)//<^
                    Instantiate(TopCornerLeftPrefab, new Vector3(x - 7.5f, y - 4.5f), Quaternion.identity, _lastGeneratedPlatform);
                else if (x == stopXGenerating - 1 && y == 0)//.>
                    Instantiate(BottomCornerRightPrefab, new Vector3(x - 7.5f, y - 4.5f), Quaternion.identity, _lastGeneratedPlatform);
                else if (x == startXGenerating)//<
                    Instantiate(MidLeftPrefab, new Vector3(x - 7.5f, y - 4.5f), Quaternion.identity, _lastGeneratedPlatform);
                else if (x == stopXGenerating - 1)//>
                    Instantiate(MidRightPrefab, new Vector3(x - 7.5f, y - 4.5f), Quaternion.identity, _lastGeneratedPlatform);
                else if(y == platformHeight - 1)//^
                    Instantiate(TopCornerMidPrefab, new Vector3(x - 7.5f, y - 4.5f), Quaternion.identity, _lastGeneratedPlatform);
                else if (y == 0)//.
                    Instantiate(BottomCornerMidPrefab, new Vector3(x - 7.5f, y - 4.5f), Quaternion.identity, _lastGeneratedPlatform);
                else
                    Instantiate(MidPrefab, new Vector3(x - 7.5f, y - 4.5f), Quaternion.identity, _lastGeneratedPlatform);
                //Debug.Log($"<color=red>x: {x} y: {y}</color>");
            }
        }
            xCounter += platformWidth;
        _width += platformWidth;
        GenerateBoxCollider(_lastGeneratedPlatform, platformWidth, platformHeight);
    }
    private void CreateSpikes()
    {
        _lastGeneratedSpikes = Instantiate(PlatformFather,Vector3.zero,Quaternion.identity);
        _lastGeneratedSpikes.name = "spikes";
        _lastGeneratedSpikes.tag = _obstacleTag;//should be in const
        int spikesNumber = Random.Range(_minSpikesBetweenPlatforms, _maxSpikesBetweenPlatforms + 1);
        for (int x = xCounter; x < _width + spikesNumber; x++)
        {
            Instantiate(_spikesPrefab, new Vector3(x - 7.5f, - 4.5f), Quaternion.identity, _lastGeneratedSpikes);
        }
        xCounter += spikesNumber;
        GenerateBoxCollider(_lastGeneratedSpikes, spikesNumber, 1);
        _width += spikesNumber;
    }

    void GenerateBoxCollider(Transform collidlessGameObject,int sizeX , int sizeY)
    {
        BoxCollider2D generatedBoxCollider = collidlessGameObject.AddComponent<BoxCollider2D>();
        generatedBoxCollider.size = new Vector2(sizeX, sizeY);
        //if(sizeY%2==0)
        //    generatedBoxCollider.offset = new Vector2(xCounter - sizeX / 2f -0.5f - 7.5f, -4.5f);
        //else
        int sizeYFactor=0;
        if (sizeY > 10)
            sizeYFactor = sizeY - 10;
        else if(sizeY<10)
            sizeYFactor = sizeY - 10;
        generatedBoxCollider.offset = new Vector2(xCounter - sizeX / 2f - 0.5f - 7.5f, 0 + 0.5f * sizeYFactor);
    }

}
