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

    public EditButton robotButton;
    public EditButton turretButton;

    private readonly Dictionary<int, Thread> threads = new Dictionary<int, Thread>();
    private readonly Dictionary<string, object> globals = new Dictionary<string, object>();

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
        robotButton.gameObject.SetActive(false);
        turretButton.gameObject.SetActive(false);
        foreach (var turret in levelLoader.Level.Turrets)
            StartExecution(turret);
    }

    internal void Stop()
    {
        OnDestroy();
        IsRunning = false;
        foreach (var robot in levelLoader.Level.Robots)
            Destroy(robot.gameObject);
        foreach (var turret in levelLoader.Level.Turrets)
            turret.transform.rotation = Quaternion.identity;
        levelLoader.Level.Robots.Clear();
        robotButton.gameObject.SetActive(true);
        turretButton.gameObject.SetActive(true);
    }

    internal void StartExecution(Robot robot)
    {
        var b = new Builtins();

        b.RegisterFunction(PigeonType.Int, "r", robot.R);
        b.RegisterFunction(PigeonType.Int, "c", robot.C);

        b.RegisterVariable(PigeonType.Int, "ID", true, robot.Id);

        b.RegisterVariable(PigeonType.Int, "EXIT_C", true, levelLoader.Level.Exit.x);
        b.RegisterVariable(PigeonType.Int, "EXIT_R", true, levelLoader.Level.Exit.y);

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

        b.RegisterFunction(PigeonType.Void, "global_set", GlobalSet, PigeonType.String, PigeonType.Any);
        b.RegisterFunction(PigeonType.Void, "global_unset", GlobalUnset, PigeonType.String);
        b.RegisterFunction(PigeonType.Bool, "global_check", GlobalCheck, PigeonType.String);
        b.RegisterFunction(PigeonType.Any, "global_get", GlobalGet, PigeonType.String);

        var interpreter = new Interpreter(pigeonEditor.GetCode("Robot"), b);

        if (interpreter.HasNoErrors())
            StartExecution(interpreter, robot.gameObject);
        else
            PrintErrors(interpreter);
    }

    internal void StartExecution(Turret turret)
    {
        var b = new Builtins();

        b.RegisterVariable(PigeonType.Int, "ID", true, turret.Id);
        b.RegisterVariable(PigeonType.Float, "RANGE", true, turret.Range);

        b.RegisterFunction(PigeonType.Float, "x", turret.X);
        b.RegisterFunction(PigeonType.Float, "y", turret.Y);

        b.RegisterFunction(PigeonType.Float, "robot_x", levelLoader.Level.RobotX, PigeonType.Int);
        b.RegisterFunction(PigeonType.Float, "robot_y", levelLoader.Level.RobotY, PigeonType.Int);
        b.RegisterFunction(PigeonType.Int, "robot_count", levelLoader.Level.RobotCount);

        b.RegisterFunction(PigeonType.Void, "shoot", turret.Shoot, PigeonType.Int);

        b.RegisterFunction(PigeonType.Float, "sqrt", Sqrt, PigeonType.Float);
        b.RegisterFunction(PigeonType.Void, "print", Print, PigeonType.Any);

        var interpreter = new Interpreter(pigeonEditor.GetCode("Turret"), b);

        if (interpreter.HasNoErrors())
            StartExecution(interpreter, turret.gameObject);
        else
            PrintErrors(interpreter);
    }

    void StartExecution(Interpreter interpreter, GameObject obj)
    {
        Thread thread = new Thread(interpreter.Evaluate);
        threads.Add(obj.GetInstanceID(), thread);
        thread.Start();
    }

    internal void StopExecution(GameObject obj)
    {
        if (threads.ContainsKey(obj.GetInstanceID()))
        {
            threads[obj.GetInstanceID()].Abort();
            threads.Remove(obj.GetInstanceID());
        }
    }

    void PrintErrors(Interpreter interpreter)
    {
        var sw = new StringWriter();
        interpreter.PrintErr(sw);
        Debug.LogError(sw.ToString());
    }

    public object GlobalSet(object[] args)
    {
        globals[(string)args[0]] = args[1];
        return null;
    }

    public object GlobalCheck(object[] args)
    {
        return globals.ContainsKey((string)args[0]);
    }

    public object GlobalUnset(object[] args)
    {
        globals.Remove((string)args[0]);
        return null;
    }

    public object GlobalGet(object[] args)
    {
        return globals[(string)args[0]];
    }

    public object Print(object[] args)
    {
        print(args[0].ToString());
        return null;
    }

    public object Sqrt(object[] args)
    {
        return Mathf.Sqrt((float)args[0]);
    }
}
