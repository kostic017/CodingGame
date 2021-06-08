using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public int W { get; }
    public int H { get; }
    public LevelObject[,] Grid { get; private set; }
    public List<Vector2Int> Exits { get; } = new List<Vector2Int>();
    public List<Robot> Robots { get; } = new List<Robot>();

    public Level(int w, int h)
    {
        W = w;
        H = h;
        Grid = new LevelObject[h, w];
    }

    public object GetTile(object[] args)
    {
        return Grid[(int)args[1], (int)args[0]].prefab.name;
    }
}
