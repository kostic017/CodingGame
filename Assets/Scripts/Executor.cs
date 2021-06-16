using Kostic017.Pigeon;
using Kostic017.Pigeon.Symbols;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class Executor : MonoBehaviour
{
    public LevelLoader levelLoader;
    public PigeonEditor pigeonEditor;

    private readonly Dictionary<int, Thread> threads = new Dictionary<int, Thread>();

    internal bool IsRunning { get; private set; }

    void OnDestroy()
    {
        foreach (var thread in threads.Values)
            thread.Abort();
        threads.Clear();
    }

    internal void Run()
    {
        IsRunning = true;
    }

    internal void Stop()
    {
        OnDestroy();
        IsRunning = false;
        foreach (var robot in levelLoader.Level.Robots)
            Destroy(robot.gameObject);
        levelLoader.Level.Robots.Clear();
    }

    internal void StartExecution(Robot robot)
    {
        var b = new Builtins();

        b.RegisterFunction(PigeonType.Int, "x", robot.X);
        b.RegisterFunction(PigeonType.Int, "y", robot.Y);

        b.RegisterVariable(PigeonType.Int, "EXIT_X", true, levelLoader.Level.Exit.x);
        b.RegisterVariable(PigeonType.Int, "EXIT_Y", true, levelLoader.Level.Exit.y);

        b.RegisterVariable(PigeonType.Int, "LEVEL_WIDTH", true, levelLoader.Level.W);
        b.RegisterVariable(PigeonType.Int, "LEVEL_HEIGHT", true, levelLoader.Level.H);

        b.RegisterFunction(PigeonType.Void, "move_up", robot.MoveUp);
        b.RegisterFunction(PigeonType.Void, "move_down", robot.MoveDown);
        b.RegisterFunction(PigeonType.Void, "move_left", robot.MoveLeft);
        b.RegisterFunction(PigeonType.Void, "move_right", robot.MoveRight);
            
        b.RegisterFunction(PigeonType.String, "get_tile", levelLoader.Level.GetTile, PigeonType.Int, PigeonType.Int);

        b.RegisterFunction(PigeonType.Int, "string_length", PigeonString.Length, PigeonType.String);
        b.RegisterFunction(PigeonType.String, "string_char", PigeonString.Char, PigeonType.String, PigeonType.Int);
            
        b.RegisterFunction(PigeonType.Int, "queue_create", PigeonQueue.Create);
        b.RegisterFunction(PigeonType.Void, "queue_destroy", PigeonQueue.Destroy, PigeonType.Int);
        b.RegisterFunction(PigeonType.Void, "queue_enqueue", PigeonQueue.Enqueue, PigeonType.Int, PigeonType.Any);
        b.RegisterFunction(PigeonType.Any, "queue_dequeue", PigeonQueue.Dequeue, PigeonType.Int);
        b.RegisterFunction(PigeonType.Bool, "queue_empty", PigeonQueue.Empty, PigeonType.Int);

        b.RegisterFunction(PigeonType.Int, "list_create", PigeonList.Create);
        b.RegisterFunction(PigeonType.Void, "list_destroy", PigeonList.Destroy, PigeonType.Int);
        b.RegisterFunction(PigeonType.Void, "list_add", PigeonList.Add, PigeonType.Int, PigeonType.Any);
        b.RegisterFunction(PigeonType.Void, "list_remove", PigeonList.Remove, PigeonType.Int, PigeonType.Int);
        b.RegisterFunction(PigeonType.Any, "list_get", PigeonList.Get, PigeonType.Int, PigeonType.Int);
        b.RegisterFunction(PigeonType.Int, "list_size", PigeonList.Size, PigeonType.Int);

        b.RegisterFunction(PigeonType.Int, "set_create", PigeonSet.Create);
        b.RegisterFunction(PigeonType.Void, "set_destroy", PigeonSet.Destroy, PigeonType.Int);
        b.RegisterFunction(PigeonType.Void, "set_add", PigeonSet.Add, PigeonType.Int, PigeonType.Any);
        b.RegisterFunction(PigeonType.Void, "set_remove", PigeonSet.Remove, PigeonType.Int, PigeonType.Any);
        b.RegisterFunction(PigeonType.Bool, "set_in", PigeonSet.In, PigeonType.Int, PigeonType.Any);

        b.RegisterFunction(PigeonType.Void, "print", Print, PigeonType.Any);
            
        var interpreter = new Interpreter(pigeonEditor.GetCode("Robot"), b);

        if (interpreter.HasNoErrors())
        {
            Thread thread = new Thread(interpreter.Evaluate);
            threads.Add(robot.gameObject.GetInstanceID(), thread);
            thread.Start();
        }
        else
        {
            var sw = new StringWriter();
            interpreter.PrintErr(sw);
            Debug.Log(sw.ToString());
        }
    }

    public object Print(object[] args)
    {
        print(args[0].ToString());
        return null;
    }
}
