using UnityEngine;

public class Programmable : MonoBehaviour
{
    public GameObject codeEditor;
    public GameObject playButton;

    public void OpenEditor()
    {
        codeEditor.SetActive(true);
        var pigeoneEditor = codeEditor.GetComponentInChildren<PigeonEditor>();
        pigeoneEditor.LoadCode(name);
        playButton.SetActive(true);
    }
}
