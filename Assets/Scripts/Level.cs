using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public int W { get; }
    public int H { get; }
    public string[,] Tiles { get; private set; }
    public List<Robot> Robots { get; } = new List<Robot>();
    public Vector2Int Exit { get; private set; } = new Vector2Int();

    public Level(int w, int h)
    {
        W = w;
        H = h;
        Tiles = new string[h, w];
    }

    internal void SetExit(int c, int r)
    {
        Exit = new Vector2Int(c, r);
    }

    public object GetExitX(object[] _)
    {
        return Exit.x;
    }

    public object GetExitY(object[] _)
    {
        return Exit.y;
    }

    public object GetTile(object[] args)
    {
        return Tiles[(int)args[1], (int)args[0]];
    }
}
