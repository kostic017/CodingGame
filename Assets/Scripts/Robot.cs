using System.Collections.Generic;
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

	class Target
	{
		private bool moved;
		private bool rotated;
		private bool calculated;

		private Robot robot;
		private Vector2Int tile;
		private Vector3 position;
		private Quaternion rotation;

		internal Target(Vector2Int tile, Robot robot)
		{
			this.tile = tile;
			this.robot = robot;
		}

		internal int C()
        {
			return tile.x;
        }

		internal int R()
        {
			return tile.y;
        }

		internal void Calculate()
        {
			if (calculated) return;
			position = robot.levelLoader.GetTilePosition(tile.y, tile.x);
			rotation = Quaternion.LookRotation(position - robot.transform.position);
			position.y = robot.transform.position.y;
			calculated = true;
        }

		internal bool Rotate()
		{
			if (rotated) return true;
			if (Quaternion.Angle(robot.transform.rotation, rotation) < 0.001f) rotated = true;
			robot.transform.rotation = Quaternion.RotateTowards(robot.transform.rotation, rotation, robot.rotationSpeed * Time.deltaTime);
			return rotated;
		}

		internal bool Move()
		{
			if (moved) return true;
			if (Vector3.Distance(robot.transform.position, position) < 0.001f) moved = true;
			robot.transform.position = Vector3.MoveTowards(robot.transform.position, position, robot.moveSpeed * Time.deltaTime);
			return moved;
		}
	}

	public float moveSpeed = 10f;
	public float rotationSpeed = 40f;

	private int r;
	private int c;

	private Animator anim;
	private LevelLoader levelLoader;

	private Target target;

	void Awake()
	{
		anim = gameObject.GetComponent<Animator>();
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
					c = target.C();
					r = target.R();
					target = null;
				}
			}

		}

	}

	private Vector2Int NextPosition(Move move)
	{
        return move switch
        {
            Move.Up => new Vector2Int(c, r + 1),
            Move.Down => new Vector2Int(c, r - 1),
            Move.Left => new Vector2Int(c - 1, r),
            _ => new Vector2Int(c + 1, r),
        };
    }

	private void SetTarget(Move move)
	{
		target = new Target(NextPosition(move), this);
	}

	void ValidateMove(Move move)
	{
		var nextPos = NextPosition(move);
		if (nextPos.x < 0 || nextPos.x >= levelLoader.Level.W - 1 || nextPos.y < 0 || nextPos.y >= levelLoader.Level.H - 1 || levelLoader.Level.Tiles[nextPos.y, nextPos.x] == "Wall")
			throw new RuntimeException($"Invalid move {move}");
    }

	public object GetX(object[] _)
    {
		return c;
	}

	public object GetY(object[] _)
    {
		return r;
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
		this.r = r;
		this.c = c;
    }

	internal void SetLevelLoader(LevelLoader levelLoader)
    {
		this.levelLoader = levelLoader;
    }
}
