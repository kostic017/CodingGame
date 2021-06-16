using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public Executor executor;
    public StopButton stopButton;

    public void OnClick()
    {
        executor.Run();
        gameObject.SetActive(false);
        stopButton.gameObject.SetActive(true);
    }
}
