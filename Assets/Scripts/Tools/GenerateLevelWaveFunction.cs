using System;
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

    private void Start()
    {
        Debug.Log(Width +" "+ Height);
        SetGridCells();
        SetGridNeighbors();
    }
    private void SetGridCells()
    {
        CellLevelGrid = new Cell[Width, Height];
        CellLevelGrid[0,0] = new Cell(Cell.GetRandomCell());
        //Debug.Log($"<color=red> x: 0 y: 0 </color>");
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                //Debug.Log($"<color=red> x: {x} y: {y} </color>");
                CellLevelGrid[x,y] = new Cell(Cell.GetRandomCell());//for now
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

    private bool CheckIfInBounds(Index2D cellIndex)
    {
        if(cellIndex.X >= 0 && cellIndex.X < Width)
            if(cellIndex.Y >= 0 && cellIndex.Y < Height)
                return true;
        return false;
    } 
}
