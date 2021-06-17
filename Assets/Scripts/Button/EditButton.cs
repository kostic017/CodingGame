using UnityEngine;

public class EditButton : MonoBehaviour
{
    public GameObject codeEditor;

    public void OnClick()
    {
        codeEditor.SetActive(true);
        codeEditor.GetComponentInChildren<PigeonEditor>().LoadCode(name);
    }
}
