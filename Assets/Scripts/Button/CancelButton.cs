using UnityEngine;

public class CancelButton : MonoBehaviour
{
    public GameObject codeEditor;

    public void OnClick()
    {
        codeEditor.SetActive(false);
    }
}
