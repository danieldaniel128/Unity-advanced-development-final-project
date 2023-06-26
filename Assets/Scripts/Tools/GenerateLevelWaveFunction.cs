﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class GenerateLevelWaveFunction : MonoBehaviour
{
    public Cell[,] CellLevelGrid;

    [SerializeField] private int _width, _height;
    public int Width
    { 
        get 
        {
            return _width; 
        } 
        private set 
        { 
            _width = value; 
        } 
    }

    public int Height
    {
        get
        {
            return _height;
        }
        private set
        {
            _height = value;
        }
    }
    /// <summary>
    /// like in num pad, the names of each prefab will be like that, representing their locations on the tile set:
    /// 7 8 9
    /// 4 5 6
    /// 1 2 3
    /// </summary>
    [Header("Ground")]
    #region TileSet Prefabs
    [SerializeField] GameObject _ground9;
    [SerializeField] GameObject _ground8;
    [SerializeField] GameObject _ground7;
    [SerializeField] GameObject _ground6;
    [SerializeField] GameObject _ground5;
    [SerializeField] GameObject _ground4;
    [SerializeField] GameObject _ground3;
    [SerializeField] GameObject _ground2;
    [SerializeField] GameObject _ground1;
    List<GameObject> _tileSetPrefabs;
    #endregion

    private void Start()
    {
        //Debug.Log(Width +" "+ Height);
        SetGridCells();
        SetGridNeighbors();
        ConvertCellsToPrefabs();
    }
    private void SetGridCells()
    {
        CellLevelGrid = new Cell[Width, Height];
        CellLevelGrid[0,0] = new Cell(Cell.GetRandomCell(), _ground8);
        //Debug.Log($"<color=red> x: 0 y: 0 </color>");
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                //Debug.Log($"<color=red> x: {x} y: {y} </color>");
                CellLevelGrid[x,y] = new Cell(Cell.GetRandomCell(), _ground8);//for now
                CellLevelGrid[x,y].SetCoordinates(x, y);
            }
        }
    }
    private void SetGridNeighbors()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                SetNeighbors(CellLevelGrid[x, y]);
            }
        }
    }

    void SetNeighbors(Cell cell)
    {
        List<Cell> neighbors = new List<Cell>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                int neighborsCellX = cell.CellIndex.X + x;
                int neighborsCellY = cell.CellIndex.Y + y;
                Index2D cellIndex = new Index2D(neighborsCellX, neighborsCellY);
                if (CheckIfInBounds(cellIndex))
                {
                    Cell neighbor = CellLevelGrid[neighborsCellX, neighborsCellY];
                    if (neighbor == null)//catching bugs of null reference exception cell
                    {
                        Debug.Log($"Neighbor at ({neighborsCellX}, {neighborsCellY}) is null.");
                    }
                    else
                    {
                        neighbors.Add(neighbor);
                    }
                }
            }
        }
        cell.SetCellNeighbors(cell,neighbors);
    }

    void ConvertCellsToPrefabs()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                InstantiatePrefab(CellLevelGrid[x, y]);
            }
        }
    }


    private bool CheckIfInBounds(Index2D cellIndex)
    {
        if(cellIndex.X >= 0 && cellIndex.X < Width)
            if(cellIndex.Y >= 0 && cellIndex.Y < Height)
                return true;
        return false;
    } 


    private void InstantiatePrefab(Cell cell)
    {
        Instantiate(cell.CellPrefab,new Vector3(cell.CellIndex.X-7.5f,cell.CellIndex.Y-4.5f),Quaternion.identity);
    }


}
