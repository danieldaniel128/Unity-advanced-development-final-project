using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//Daniel
public class Cell
{
    public Index2D CellIndex { get; private set; }

    public Action<CellStateEnum> OnStateSet { get; private set; }
    public List<Cell> NeighborCells { get; private set; }   
    #region NeighborsCells
    public Cell LeftNeighborCell { get; private set; }
    public Cell RightNeighborCell { get; private set; }
    public Cell BottomNeighborCell { get; private set; }
    public Cell BottomRightNeighborCell { get; private set; }
    public Cell BottomleftNeighborCell { get; private set; }
    public Cell TopNeighborCell { get; private set; }
    public Cell TopRightNeighborCell { get; private set; }
    public Cell TopleftNeighborCell { get; private set; }
    #endregion

    /// <summary>
    /// cell states
    /// </summary>
    public enum CellStateEnum 
    {
        Ground,
        CornerRight,
        CornerLeft,
        EmptyCell
    }
    /// <summary>
    /// the cell state of the cell
    /// </summary>
    private CellStateEnum _cellState;
    public CellStateEnum CellState { get => _cellState; private set { _cellState = value; OnStateSetInvoke(_cellState); } }
    /// <summary>
    /// Neighbors of this Cell
    /// </summary>

    /// <summary>
    /// CellStates That the CellState can possibly connect to
    /// </summary>
    public List<CellStateEnum> PossibleCellStatesToConnect { get; private set; } 
        = new List<CellStateEnum> 
        {
            CellStateEnum.EmptyCell
            , CellStateEnum.CornerLeft
            , CellStateEnum.CornerRight
            , CellStateEnum.Ground 
        }; //default cell states
    /// <summary>
    /// each neighbor cell that got set will affect the new possible states of this cell
    /// </summary>
    public List<CellStateEnum> CurrentPossibleCellStatesToConnect { get; private set; }
        = new List<CellStateEnum>
        {
            CellStateEnum.EmptyCell
            , CellStateEnum.CornerLeft
            , CellStateEnum.CornerRight
            , CellStateEnum.Ground
        }; //default cell states

    public Cell(CellStateEnum cellState)
    {
        CellState = cellState;
        OnStateSet = CalculateNewPossibleCellStatesToConnect;
    }

    private void CalculateNewPossibleCellStatesToConnect(CellStateEnum cellState)
    {

    }
    private void OnStateSetInvoke(CellStateEnum cellState)
    {
        OnStateSet?.Invoke(cellState);
    }

    public void SetCoordinates(int x,int y)//class of index is too much for now
    {
        CellIndex = new Index2D(x, y);
    }
    public void SetCoordinates(Index2D cellIndex)//class of index is too much for now
    {
        CellIndex = new Index2D(cellIndex.X, cellIndex.Y);
    }

    /// <summary>
    /// sets the cell State
    /// </summary>
    /// <param name="cellState"></param>
    public void SetState(CellStateEnum cellState)
    {
        CellState = cellState;
    }

    /// <summary>
    /// sets the the possible states to connect to the cell state of the states
    /// </summary>
    /// <param name="possibleCellStatesToConnect"></param>
    public void SetPossibleCellStatesToConnect(List<CellStateEnum> possibleCellStatesToConnect) 
    {
        PossibleCellStatesToConnect = possibleCellStatesToConnect;
    }

    public void RemovePossibleCellStateToConnect(CellStateEnum possibleCellStateToConnect)
    {
        PossibleCellStatesToConnect.Remove(possibleCellStateToConnect);
    }
    public void ClearPossibleCellStatesToConnect()
    {
        PossibleCellStatesToConnect.Clear();
    }
    void UpdateCurrentPossibleCellStatesToConnect()
    {

    }

    /// <summary>
    /// sets the list of all neighbor cells
    /// </summary>
    /// <param name="cells"></param>
    public void SetNeighbors(List<Cell> cells)
    {
        NeighborCells = cells;
    }
    /// <summary>
    /// sets all neighbors and sets the list of all neighbor cells
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="neighbors"></param>
    public void SetCellNeighbors(Cell cell, List<Cell> neighbors)
    {
        cell.SetNeighbors(neighbors);
        foreach (Cell neighbor in neighbors)
        {
            int dx = neighbor.CellIndex.X - cell.CellIndex.X;
            int dy = neighbor.CellIndex.Y - cell.CellIndex.Y;
            switch ((dx, dy))
            {
                case (-1, 0):
                    cell.LeftNeighborCell = neighbor;
                    break;
                case (1, 0):
                    cell.RightNeighborCell = neighbor;
                    break;
                case (0, -1):
                    cell.BottomNeighborCell = neighbor;
                    break;
                case (1, -1):
                    cell.BottomRightNeighborCell = neighbor;
                    break;
                case (-1, -1):
                    cell.BottomleftNeighborCell = neighbor;
                    break;
                case (0, 1):
                    cell.TopNeighborCell = neighbor;
                    break;
                case (1, 1):
                    cell.TopRightNeighborCell = neighbor;
                    break;
                case (-1, 1):
                    cell.TopleftNeighborCell = neighbor;
                    break;
                default:
                    break;
            }
        }
    }

    public void SetNeighbors(Cell right = null,Cell left = null, Cell down = null, Cell downRight = null, Cell downLeft = null, Cell top = null, Cell topRight = null, Cell topLeft = null) 
    {
        LeftNeighborCell = left;
        RightNeighborCell = right;
        BottomNeighborCell = down;
        BottomRightNeighborCell = downRight;
        BottomleftNeighborCell = downLeft;
        TopNeighborCell = top;
        TopRightNeighborCell = topRight;
        TopleftNeighborCell = topLeft;
    }

    public static CellStateEnum GetRandomCell()
    {
        return (CellStateEnum)UnityEngine.Random.Range(0, 3);
    }
}
