using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{

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

	private readonly Queue<Vector2Int> waypoints = new Queue<Vector2Int>();

	void Awake()
	{
		anim = gameObject.GetComponent<Animator>();
	}

	void Update()
	{

		if (target == null)
		{
			if (waypoints.Count > 0)
			{
				var targetPos = levelLoader.GetTilePosition(waypoints.Peek().y, waypoints.Peek().x);
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
					var waypoint = waypoints.Dequeue();
					c = waypoint.x;
					r = waypoint.y;
					target = null;
                }
            }
        }

	}

	private static Robot GetRobot(int idx)
    {
		return LevelLoader.Level.Robots[idx];
    }

	public static object MoveUp(object[] args)
	{
		var robot = GetRobot((int)args[0]);
		if (robot.IsInvalidMove(robot.c, robot.r + 1))
			throw new RuntimeException("Can't move robot Up");
		robot.waypoints.Enqueue(new Vector2Int(robot.c, robot.r + 1));
		return null;
	}

	public static object MoveDown(object[] args)
	{
		var robot = GetRobot((int)args[0]);
		if (robot.IsInvalidMove(robot.c, robot.r - 1))
			throw new RuntimeException("Can't move robot Down");
		robot.waypoints.Enqueue(new Vector2Int(robot.c, robot.r - 1));
		return null;
	}

	public static object MoveLeft(object[] args)
	{
		var robot = GetRobot((int)args[0]);
		if (robot.IsInvalidMove(robot.c - 1, robot.r))
			throw new RuntimeException("Can't move robot Left");
		robot.waypoints.Enqueue(new Vector2Int(robot.c - 1, robot.r));
		return null;
	}

	public static object MoveRight(object[] args)
	{
		var robot = GetRobot((int)args[0]);
		if (robot.IsInvalidMove(robot.c + 1, robot.r))
			throw new RuntimeException("Can't move robot Right");
		robot.waypoints.Enqueue(new Vector2Int(robot.c + 1, robot.r));
		return null;
	}

	bool IsInvalidMove(int c, int r)
	{
		return c < 0 || c >= LevelLoader.Level.W - 1 || r < 0 || r >= LevelLoader.Level.H - 1 || LevelLoader.Level.Grid[r, c].prefab.name == "Wall";
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
