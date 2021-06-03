using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public int W { get; }
    public int H { get; }
    public LevelObject[,] Grid { get; private set; }
    public List<Vector2Int> Exits { get; } = new List<Vector2Int>();

    public Level(int w, int h)
    {
        W = w;
        H = h;
        Grid = new LevelObject[h, w];
    }
}
