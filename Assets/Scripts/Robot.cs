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

	public float moveSpeed = 10f;
	public float rotationSpeed = 40f;

	internal int R { get; private set; }
	internal int C { get; private set; }

	private Animator anim;
	private LevelLoader levelLoader;

	private RobotTarget target;

	void Awake()
	{
		anim = gameObject.GetComponent<Animator>();
		levelLoader = FindObjectOfType<LevelLoader>();
	}

	void Update()
	{

		if (target != null)
		{

			target.Calculate();
			anim.SetBool("Walk_Anim", true);

			if (target.Rotate())
			{
				if (target.Move())
				{
					anim.SetBool("Walk_Anim", false);
					C = target.C();
					R = target.R();
					target = null;
				}
			}

		}

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
