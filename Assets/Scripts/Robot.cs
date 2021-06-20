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

	internal int Id { get; set; }
	internal int Row { get; private set; }
	internal int Col { get; private set; }
	internal Vector3 Positon { get; private set; }

	private Animator anim;
	private Executor executor;
	private LevelLoader levelLoader;

	private int hp = 3;
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
		Positon = transform.position;

		if (hp <= 0 || (Col == levelLoader.Level.Exit.x && Row == levelLoader.Level.Exit.y))
        {
			Destroy(gameObject);
			return;
        }

		if (target != null)
		{

			if (anim.GetBool("Open_Anim") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.7f)
				return;

			target.Calculate();
			anim.SetBool("Walk_Anim", true);

			if (target.Rotate())
			{
				if (target.Move())
				{
					Col = target.Col();
					Row = target.Row();
					target = null;
				}
			}

		}
	}

    internal void Damage()
    {
		--hp;
    }

    void OnDestroy()
    {
        executor.StopExecution(gameObject);
		levelLoader.Level.Remove(this);
    }

    private Vector2Int NextPosition(Move move)
	{
        return move switch
        {
            Move.Up => new Vector2Int(Col, Row + 1),
            Move.Down => new Vector2Int(Col, Row - 1),
            Move.Left => new Vector2Int(Col - 1, Row),
            _ => new Vector2Int(Col + 1, Row),
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

	public object C(object[] _)
    {
		return Col;
	}

	public object R(object[] _)
    {
		return Row;
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
		Row = r;
		Col = c;
    }
}
