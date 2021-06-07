using Kostic017.Pigeon;
using Kostic017.Pigeon.Symbols;
using System.IO;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public LevelLoader levelLoader;
    public PigeonEditor pigeonEditor;

    public void OnClick()
    {
        for (var i = 0; i < LevelLoader.Level.Robots.Count; ++i)
        {
            var builtins = new Builtins();
            builtins.RegisterVariable(PigeonType.Int, "idx", true, i);
            builtins.RegisterFunction(PigeonType.Void, "MoveUp", new Variable[] { new Variable(PigeonType.Int) }, Robot.MoveUp);
            builtins.RegisterFunction(PigeonType.Void, "MoveDown", new Variable[] { new Variable(PigeonType.Int) }, Robot.MoveDown);
            builtins.RegisterFunction(PigeonType.Void, "MoveLeft", new Variable[] { new Variable(PigeonType.Int) }, Robot.MoveLeft);
            builtins.RegisterFunction(PigeonType.Void, "MoveRight", new Variable[] { new Variable(PigeonType.Int) }, Robot.MoveRight);

            var interpreter = new Interpreter(pigeonEditor.GetCode("Robot"), builtins);
            if (interpreter.HasNoErrors())
                interpreter.Evaluate();
            else
            {
                var sw = new StringWriter();
                interpreter.PrintErr(sw);
                Debug.Log(sw.ToString());
            }
        }
    }
}
