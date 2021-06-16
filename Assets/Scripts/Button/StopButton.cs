using UnityEngine;

public class StopButton : MonoBehaviour
{
    public Executor executor;
    public PlayButton playButton;

    public void OnClick()
    {
        executor.Stop();
        gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
    }
}
