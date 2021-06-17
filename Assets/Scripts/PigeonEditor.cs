using System.Collections.Generic;
using UnityEngine;

public class PigeonEditor : MonoBehaviour
{
    public GameObject playButton;

    private string file;

    private FreeCam freeCam;
    private InGameCodeEditor.CodeEditor inGameCodeEditor;

    private readonly Dictionary<string, string> codes = new Dictionary<string, string>();

    void Awake()
    {
        gameObject.SetActive(false);
        freeCam = Camera.main.GetComponent<FreeCam>();
        inGameCodeEditor = GetComponent<InGameCodeEditor.CodeEditor>();
        SaveCode("Robot", Resources.Load<TextAsset>("Robot").text);
        SaveCode("Turret", Resources.Load<TextAsset>("Turret").text);
    }

    void OnEnable()
    {
        freeCam.enabled = false;
        playButton.SetActive(false);
    }

    void OnDisable()
    {
        if (freeCam != null)
            freeCam.enabled = true;
        playButton.SetActive(true);
    }

    public string GetCode(string name)
    {
        return codes[name];
    }

    public void LoadCode(string name)
    {
        file = name;
        inGameCodeEditor.Text = codes[name] ?? "";
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
