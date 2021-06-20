using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;

    internal int Row { get; private set; }
    internal int Col { get; private set; }

    private Executor executor;
    private LevelLoader levelLoader;

    private readonly int MaxRobotCount = 3;

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
                if (rob.Row == Row && rob.Col == Col)
                    return;
            var gameObject = Instantiate(prefab, transform.position + Vector3.up * transform.localScale.y * 0.5f, Quaternion.identity);
            var robot = gameObject.GetComponent<Robot>();
            robot.SetPosition(Row, Col);
            levelLoader.Level.Add(robot);
            executor.StartExecution(robot);
        }
    }

    internal void SetPosition(int r, int c)
    {
        Row = r;
        Col = c;
    }
}
