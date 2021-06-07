using System.Collections.Generic;
using UnityEngine;

public class PigeonEditor : MonoBehaviour
{
    private string file;
    private InGameCodeEditor.CodeEditor inGameCodeEditor;

    private readonly Dictionary<string, string> codes = new Dictionary<string, string>();

    void Awake()
    {
        inGameCodeEditor = GetComponent<InGameCodeEditor.CodeEditor>();
        SaveCode("Robot",
            "// Robot\n" +
            "// MoveUp(idx)\n" +
            "// MoveDown(idx)\n" +
            "// MoveLeft(idx)\n" +
            "// MoveRight(idx)\n"
        );
        SaveCode("Turret", "// Turret\n");
    }

    public string GetCode(string name)
    {
        return codes[name];
    }

    public void LoadCode(string name)
    {
        file = name;
        inGameCodeEditor.Text = codes[name] ?? "";
        inGameCodeEditor.InputField.MoveTextEnd(false);
        inGameCodeEditor.InputField.Select();
    }

    public void SaveCode(string name, string code)
    {
        codes[name] = code;
    }

    public void SaveCode(string code)
    {
        SaveCode(file, code);
        file = null;
    }
}
