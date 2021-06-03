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
        SaveCode("Robot", "// Robot Related Code\n");
        SaveCode("Turret", "// Turret Related Code\n");
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
