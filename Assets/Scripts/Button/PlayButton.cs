using Kostic017.Pigeon;
using Kostic017.Pigeon.Symbols;
using System.IO;
using System.Threading;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public LevelLoader levelLoader;
    public PigeonEditor pigeonEditor;

    private Thread thread;

    void OnDestroy()
    {
        if (thread != null)
        {
            thread.Abort();
            thread = null;
        }
    }

    public void OnClick()
    {
        foreach (var robot in levelLoader.Level.Robots)
        {
            var b = new Builtins();

            b.RegisterFunction(PigeonType.Int, "x", new Variable[] { }, robot.X);
            b.RegisterFunction(PigeonType.Int, "y", new Variable[] { }, robot.Y);

            b.RegisterVariable(PigeonType.Int, "EXIT_X", true, levelLoader.Level.Exit.x);
            b.RegisterVariable(PigeonType.Int, "EXIT_Y", true, levelLoader.Level.Exit.y);

            b.RegisterVariable(PigeonType.Int, "LEVEL_WIDTH", true, levelLoader.Level.W);
            b.RegisterVariable(PigeonType.Int, "LEVEL_HEIGHT", true, levelLoader.Level.H);

            b.RegisterFunction(PigeonType.Void, "move_up", new Variable[] {}, robot.MoveUp);
            b.RegisterFunction(PigeonType.Void, "move_down", new Variable[] { }, robot.MoveDown);
            b.RegisterFunction(PigeonType.Void, "move_left", new Variable[] { }, robot.MoveLeft);
            b.RegisterFunction(PigeonType.Void, "move_right", new Variable[] { }, robot.MoveRight);
            
            b.RegisterFunction(PigeonType.String, "get_tile", new Variable[] { new Variable(PigeonType.Int), new Variable(PigeonType.Int) }, levelLoader.Level.GetTile);

            b.RegisterFunction(PigeonType.Int, "string_length", new Variable[] { new Variable(PigeonType.String) }, PigeonString.Length);
            b.RegisterFunction(PigeonType.String, "string_char", new Variable[] { new Variable(PigeonType.String), new Variable(PigeonType.Int) }, PigeonString.Char);
            
            b.RegisterFunction(PigeonType.Int, "queue_create", new Variable[] { }, PigeonQueue.Create);
            b.RegisterFunction(PigeonType.Void, "queue_destroy", new Variable[] { new Variable(PigeonType.Int) }, PigeonQueue.Destroy);
            b.RegisterFunction(PigeonType.Void, "queue_enqueue", new Variable[] { new Variable(PigeonType.Int), new Variable(PigeonType.Any) }, PigeonQueue.Enqueue);
            b.RegisterFunction(PigeonType.Any, "queue_dequeue", new Variable[] { new Variable(PigeonType.Int) }, PigeonQueue.Dequeue);
            b.RegisterFunction(PigeonType.Bool, "queue_empty", new Variable[] { new Variable(PigeonType.Int) }, PigeonQueue.Empty);

            b.RegisterFunction(PigeonType.Int, "list_create", new Variable[] { }, PigeonList.Create);
            b.RegisterFunction(PigeonType.Void, "list_destroy", new Variable[] { new Variable(PigeonType.Int) }, PigeonList.Destroy);
            b.RegisterFunction(PigeonType.Void, "list_add", new Variable[] { new Variable(PigeonType.Int), new Variable(PigeonType.Any) }, PigeonList.Add);
            b.RegisterFunction(PigeonType.Void, "list_remove", new Variable[] { new Variable(PigeonType.Int), new Variable(PigeonType.Int) }, PigeonList.Remove);
            b.RegisterFunction(PigeonType.Any, "list_get", new Variable[] { new Variable(PigeonType.Int), new Variable(PigeonType.Int) }, PigeonList.Get);
            b.RegisterFunction(PigeonType.Int, "list_size", new Variable[] { new Variable(PigeonType.Int) }, PigeonList.Size);

            b.RegisterFunction(PigeonType.Int, "set_create", new Variable[] { }, PigeonSet.Create);
            b.RegisterFunction(PigeonType.Void, "set_destroy", new Variable[] { new Variable(PigeonType.Int) }, PigeonSet.Destroy);
            b.RegisterFunction(PigeonType.Void, "set_add", new Variable[] { new Variable(PigeonType.Int), new Variable(PigeonType.Any) }, PigeonSet.Add);
            b.RegisterFunction(PigeonType.Void, "set_remove", new Variable[] { new Variable(PigeonType.Int), new Variable(PigeonType.Any) }, PigeonSet.Remove);
            b.RegisterFunction(PigeonType.Bool, "set_in", new Variable[] { new Variable(PigeonType.Int), new Variable(PigeonType.Any) }, PigeonSet.In);

            b.RegisterFunction(PigeonType.Void, "print", new Variable[] { new Variable(PigeonType.Any) }, Print);
            
            var interpreter = new Interpreter(pigeonEditor.GetCode("Robot"), b);
            if (interpreter.HasNoErrors())
            {
                if (thread != null)
                    thread.Abort();
                thread = new Thread(interpreter.Evaluate);
                thread.Start();
            }
            else
            {
                var sw = new StringWriter();
                interpreter.PrintErr(sw);
                Debug.Log(sw.ToString());
            }
        }
    }

    public object Print(object[] args)
    {
        print(args[0].ToString());
        return null;
    }
}
