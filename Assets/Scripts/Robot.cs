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

		private Vector3 position;
		private Quaternion rotation;

		internal Target(Vector3 position, Quaternion rotation)
		{
			this.position = position;
			this.rotation = rotation;
		}

		internal bool Rotate(Robot robot)
		{
			if (rotated) return true;
			if (Quaternion.Angle(robot.transform.rotation, rotation) < 0.001f) rotated = true;
			robot.transform.rotation = Quaternion.RotateTowards(robot.transform.rotation, rotation, robot.rotationSpeed * Time.deltaTime);
			return rotated;
        }

		internal bool Move(Robot robot)
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
	private Target target;
	private LevelLoader levelLoader;

	private readonly Queue<Move> moves = new Queue<Move>();

	void Awake()
	{
		anim = gameObject.GetComponent<Animator>();
	}

	void Update()
	{

		if (target == null)
		{
			if (moves.Count > 0)
			{
				if (!IsNextMoveValid())
					throw new RuntimeException("Cannot move robot " + moves.Peek());

				var targetPos = levelLoader.GetTilePosition(NextPosition().y, NextPosition().x);
				var targetRot = Quaternion.LookRotation(targetPos - transform.position);
				targetPos.y = transform.position.y;
				target = new Target(targetPos, targetRot);
			}
		}
		else
        {
			anim.SetBool("Walk_Anim", true);
			if (target.Rotate(this))
            {
				if (target.Move(this))
                {
					anim.SetBool("Walk_Anim", false);
					c = NextPosition().x;
					r = NextPosition().y;
					moves.Dequeue();
					target = null;
                }
            }
        }

	}

	private Vector2Int NextPosition()
    {
		switch (moves.Peek())
        {
			case Move.Up:
				return new Vector2Int(c, r + 1);
			case Move.Down:
				return new Vector2Int(c, r - 1);
			case Move.Left:
				return new Vector2Int(c - 1, r);
			default:
			case Move.Right:
				return new Vector2Int(c + 1, r);
        }
    }

	
	bool IsNextMoveValid()
	{
		var nextPosition = NextPosition();
		return nextPosition.x > 0
			&& nextPosition.x <= levelLoader.Level.W - 1
			&& nextPosition.y > 0
			&& nextPosition.y <= levelLoader.Level.H - 1
			&& levelLoader.Level.Grid[nextPosition.y, nextPosition.x].prefab.name != "Wall";
    }

	private static Robot GetRobot(int idx)
    {
		return FindObjectOfType<LevelLoader>().Level.Robots[idx];
    }

	public static object MoveUp(object[] args)
	{
		GetRobot((int)args[0]).moves.Enqueue(Move.Up);
		return null;
	}

	public static object MoveDown(object[] args)
	{
		GetRobot((int)args[0]).moves.Enqueue(Move.Down);
		return null;
	}

	public static object MoveLeft(object[] args)
	{
		GetRobot((int)args[0]).moves.Enqueue(Move.Left);
		return null;
	}

	public static object MoveRight(object[] args)
	{
		GetRobot((int)args[0]).moves.Enqueue(Move.Right);
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
