using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;

    private int r;
    private int c;

    private Executor executor;
    private LevelLoader levelLoader;

    private readonly int MaxRobotCount = 5;

    void Start()
    {
        executor = FindObjectOfType<Executor>();
        levelLoader = FindObjectOfType<LevelLoader>();
    }

    void Update()
    {
        if (executor.IsRunning && levelLoader.Level.Robots.Count < MaxRobotCount)
        {
            foreach (var rob in levelLoader.Level.Robots)
                if (rob.R == r && rob.C == c)
                    return;
            var gameObject = Instantiate(prefab, transform.position + Vector3.up * transform.localScale.y * 0.5f, Quaternion.identity);
            var robot = gameObject.GetComponent<Robot>();
            robot.SetPosition(r, c);
            executor.StartExecution(robot);
            levelLoader.Level.Robots.Add(robot);
        }
    }

    internal void SetPosition(int r, int c)
    {
        this.r = r;
        this.c = c;
    }
}
