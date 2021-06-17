using UnityEngine;

public class Robot : MonoBehaviour
{
	enum Move
	{
		Up,
		Down,
		Left,
		Right
	}

	internal readonly float MoveSpeed = 10f;
	internal readonly float RotationSpeed = 100f;

	internal int R { get; private set; }
	internal int C { get; private set; }

	private Animator anim;
	private Executor executor;
	private LevelLoader levelLoader;

	private RobotTarget target;

	void Awake()
	{
		executor = FindObjectOfType<Executor>();
		anim = gameObject.GetComponent<Animator>();
		levelLoader = FindObjectOfType<LevelLoader>();
		anim.speed = 1.2f;
	}

	void Update()
	{
		if (C == levelLoader.Level.Exit.x && R == levelLoader.Level.Exit.y)
			Destroy(gameObject);

		if (target != null)
		{

			target.Calculate();
			anim.SetBool("Walk_Anim", true);

			if (target.Rotate())
			{
				if (target.Move())
				{
					C = target.C();
					R = target.R();
					target = null;
				}
			}

		}

	}

    void OnDestroy()
    {
        executor.StopExecution(this);
		levelLoader.Level.Robots.Remove(this);
    }

    private Vector2Int NextPosition(Move move)
	{
        return move switch
        {
            Move.Up => new Vector2Int(C, R + 1),
            Move.Down => new Vector2Int(C, R - 1),
            Move.Left => new Vector2Int(C - 1, R),
            _ => new Vector2Int(C + 1, R),
        };
    }

	private void SetTarget(Move move)
	{
		target = new RobotTarget(this, NextPosition(move), levelLoader);
	}

	void ValidateMove(Move move)
	{
		var nextPos = NextPosition(move);
		if (nextPos.x < 0 || nextPos.x >= levelLoader.Level.W  || nextPos.y < 0 || nextPos.y >= levelLoader.Level.H || levelLoader.Level.Tiles[nextPos.y, nextPos.x] == "Wall")
			throw new RuntimeException($"Invalid move {move}");
    }

	public object X(object[] _)
    {
		return C;
	}

	public object Y(object[] _)
    {
		return R;
    }

	public object MoveUp(object[] _)
	{
		ValidateMove(Move.Up);
		SetTarget(Move.Up);
		while (target != null);
		return null;
	}

	public object MoveDown(object[] _)
	{
		ValidateMove(Move.Down);
		SetTarget(Move.Down);
		while (target != null);
		return null;
	}

	public object MoveLeft(object[] _)
	{
		ValidateMove(Move.Left);
		SetTarget(Move.Left);
		while (target != null);
		return null;
	}

	public object MoveRight(object[] _)
	{
		ValidateMove(Move.Right);
		SetTarget(Move.Right);
		while (target != null);
		return null;
	}

	internal void SetPosition(int r, int c)
    {
		R = r;
		C = c;
    }
}
