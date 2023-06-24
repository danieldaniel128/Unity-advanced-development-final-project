using System;
using System.Collections.Generic;

public struct Index2D
{
    public int X { get; private set; }
    public int Y { get; private set; }

   public Index2D(int x, int y)
    {
        X = x;
        Y = y;
    }
}
