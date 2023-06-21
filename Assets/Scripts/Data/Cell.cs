using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public int _x { get; private set; }
    public int _y { get; private set; }

    public Action OnStateSet { get; private set; }
    public List<Cell> NeighborCells { get; private set; }   

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
    public CellStateEnum CellState { get; private set; }
    /// <summary>
    /// Neighbors of this Cell
    /// </summary>
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

    public void SetCoordinates(int x,int y)//class of index is too much for now
    {
        _x = x;
        _y = y;
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
    public Cell(CellStateEnum cellState)
    {
        CellState = cellState;
        //OnStateSet
    }
    void UpdateCurrentPossibleCellStatesToConnect()
    {

    }


    public void SetNeighbors(List<Cell> cells)
    {
        NeighborCells = cells;
    }

    public void SetNeighbors(Cell right,Cell left, Cell down, Cell downRight, Cell downLeft, Cell top, Cell topRight, Cell topLeft) 
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
