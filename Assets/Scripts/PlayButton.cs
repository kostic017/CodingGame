using Kostic017.Pigeon;
using Kostic017.Pigeon.Symbols;
using System.IO;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public LevelLoader levelLoader;
    public PigeonEditor pigeonEditor;

    private readonly Builtins robotBuiltins = new Builtins();

    void Awake()
    {
        robotBuiltins.RegisterFunction(PigeonType.Void, "MoveUp", new Variable[] { new Variable(PigeonType.Int) }, Robot.MoveUp);
        robotBuiltins.RegisterFunction(PigeonType.Void, "MoveDown", new Variable[] { new Variable(PigeonType.Int) }, Robot.MoveDown);
        robotBuiltins.RegisterFunction(PigeonType.Void, "MoveLeft", new Variable[] { new Variable(PigeonType.Int) }, Robot.MoveLeft);
        robotBuiltins.RegisterFunction(PigeonType.Void, "MoveRight", new Variable[] { new Variable(PigeonType.Int) }, Robot.MoveRight);
    }

    public void OnClick()
    {
        for (var i = 0; i < LevelLoader.Level.Robots.Count; ++i)
        {
            robotBuiltins.RegisterVariable(PigeonType.Int, "idx", true, i);
            var interpreter = new Interpreter(pigeonEditor.GetCode("Robot"), robotBuiltins);
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
