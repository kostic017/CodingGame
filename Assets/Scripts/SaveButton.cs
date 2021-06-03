using UnityEngine;

public class SaveButton : MonoBehaviour
{
    public GameObject codeEditor;

    public void OnClick()
    {
        codeEditor.SetActive(false);
        var pigeoneEditor = codeEditor.GetComponentInChildren<PigeonEditor>();
        var inGameCodeEditor = codeEditor.GetComponentInChildren<InGameCodeEditor.CodeEditor>();
        pigeoneEditor.SaveCode(inGameCodeEditor.Text);
    }
}
