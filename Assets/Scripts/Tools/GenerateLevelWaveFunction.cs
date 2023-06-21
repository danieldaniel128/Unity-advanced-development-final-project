using System;
using System.Collections.Generic;
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
                //CellLevelGrid[x,y] = new Cell
            }
        }
    }
    private bool CheckIfInBounds(int x,int y)
    {
        if(x >= 0 && x < Width)
            if(y>=0 && y < Height)
                return true;
        return false;
    } 
}
