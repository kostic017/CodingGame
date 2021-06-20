using System.Collections.Generic;
using UnityEngine;

public class Level
{
    internal int W { get; }
    internal int H { get; }
    internal string[,] Tiles { get; private set; }
    internal List<Turret> Turrets { get; } = new List<Turret>();
    internal Vector2Int Exit { get; private set; } = new Vector2Int();
    internal Dictionary<int, Robot> Robots { get; } = new Dictionary<int, Robot>();

    internal Level(int w, int h)
    {
        W = w;
        H = h;
        Tiles = new string[h, w];
    }

    internal void SetExit(int r, int c)
    {
        Exit = new Vector2Int(c, r);
    }

    internal void Add(Robot robot)
    {
        robot.Id = Robots.Count;
        Robots.Add(robot.Id, robot);
    }

    internal void Add(Turret turret)
    {
        turret.Id = Turrets.Count;
        Turrets.Add(turret);
    }

    internal void Remove(Robot robot)
    {
        Robots[robot.Id] = null;
    }

    public object GetTile(object[] args)
    {
        return Tiles[(int)args[1], (int)args[0]];
    }

    public object RobotX(object[] args)
    {
        var id = (int)args[0];
        if (Robots[id] == null)
            return int.MaxValue;
        return Robots[id].Positon.x;
    }

    public object RobotY(object[] args)
    {
        var id = (int)args[0];
        if (Robots[id] == null) 
            return int.MaxValue;
        return Robots[id].Positon.z;
    }

    public object RobotMaxId(object[] _)
    {
        return Robots.Count;
    }
}
