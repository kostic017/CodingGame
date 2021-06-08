using Kostic017.Pigeon;
using Kostic017.Pigeon.Symbols;
using System.IO;
using System.Threading;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public LevelLoader levelLoader;
    public PigeonEditor pigeonEditor;

    public void OnClick()
    {
        foreach (var robot in levelLoader.Level.Robots)
        {
            var b = new Builtins();
            b.RegisterFunction(PigeonType.Int, "GetX", new Variable[] { }, robot.GetX);
            b.RegisterFunction(PigeonType.Int, "GetY", new Variable[] { }, robot.GetY);

            b.RegisterFunction(PigeonType.Void, "MoveUp", new Variable[] {}, robot.MoveUp);
            b.RegisterFunction(PigeonType.Void, "MoveDown", new Variable[] { }, robot.MoveDown);
            b.RegisterFunction(PigeonType.Void, "MoveLeft", new Variable[] { }, robot.MoveLeft);
            b.RegisterFunction(PigeonType.Void, "MoveRight", new Variable[] { }, robot.MoveRight);
            
            b.RegisterFunction(PigeonType.Int, "GetExitX", new Variable[] { }, levelLoader.Level.GetExitX);
            b.RegisterFunction(PigeonType.Int, "GetExitY", new Variable[] { }, levelLoader.Level.GetExitY);
            b.RegisterFunction(PigeonType.String, "GetTile", new Variable[] { new Variable(PigeonType.Int), new Variable(PigeonType.Int) }, levelLoader.Level.GetTile);

            b.RegisterFunction(PigeonType.Void, "Print", new Variable[] { new Variable(PigeonType.Any) }, Print);
            b.RegisterFunction(PigeonType.Int, "RandomRange", new Variable[] { new Variable(PigeonType.Int), new Variable(PigeonType.Int) }, RandomRange);

            var interpreter = new Interpreter(pigeonEditor.GetCode("Robot"), b);
            if (interpreter.HasNoErrors())
                new Thread(interpreter.Evaluate).Start();
            else
            {
                var sw = new StringWriter();
                interpreter.PrintErr(sw);
                Debug.Log(sw.ToString());
            }
        }
    }

    public object RandomRange(object[] args)
    {
        return new System.Random().Next((int)args[0], (int)args[1]);
    }

    public object Print(object[] args)
    {
        print(args[0].ToString());
        return null;
    }
}
