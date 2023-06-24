using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class GenerateLevelWaveFunction : MonoBehaviour
{
    Cell[,] CellLevelGrid;

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

    private void Start()
    {
        SetGridCells();
    }
    private void SetGridCells()
    {
        CellLevelGrid = new Cell[Width, Height];
        CellLevelGrid[0,0] = new Cell(Cell.GetRandomCell());
        for (int x = 1; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                CellLevelGrid[x,y].SetCoordinates(x, y);
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
                int neighborsCellX = cell._x + x;
                int neighborsCellY = cell._y + y;
                if (CheckIfInBounds(neighborsCellX, neighborsCellY))
                    neighbors.Add(CellLevelGrid[neighborsCellX, neighborsCellY]);
            }
        }
        cell.SetNeighbors(neighbors);
    }
    private bool CheckIfInBounds(int x,int y)
    {
        if(x >= 0 && x < Width)
            if(y>=0 && y < Height)
                return true;
        return false;
    } 
}
