using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    
    private int r;
    private int c;
    private LevelLoader levelLoader;

    void Start()
    {
        var gameObject = Instantiate(prefab, transform.position + Vector3.up * transform.localScale.y * 0.5f, Quaternion.identity);
        var robot = gameObject.GetComponent<Robot>();
        robot.SetPosition(r, c);
        robot.SetLevelLoader(levelLoader);
    }

    internal void SetPosition(int r, int c)
    {
        this.r = r;
        this.c = c;
    }

    internal void SetLevelLoader(LevelLoader levelLoader)
    {
        this.levelLoader = levelLoader;
    }
}
