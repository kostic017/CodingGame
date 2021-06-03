using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{

	public float moveSpeed = 10f;
	public float rotationSpeed = 40f;
	
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

	private int r;
	private int c;
	private Animator anim;
	private LevelLoader levelLoader;

	private readonly Queue<Vector2Int> waypoints = new Queue<Vector2Int>();

	void Awake()
	{
		anim = gameObject.GetComponent<Animator>();
	}

	private Target target;

	void Update()
	{

		if (target == null)
		{
			CheckKey();

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

	void MoveUp()
	{
		if (IsInvalidMove(c, r + 1))
			throw new RuntimeException("Can't move robot Up");
		waypoints.Enqueue(new Vector2Int(c, r + 1));
	}

	void MoveDown()
	{
		if (IsInvalidMove(c, r - 1))
			throw new RuntimeException("Can't move robot Down");
		waypoints.Enqueue(new Vector2Int(c, r - 1));
	}

	void MoveLeft()
	{
		if (IsInvalidMove(c - 1, r))
			throw new RuntimeException("Can't move robot Left");
		waypoints.Enqueue(new Vector2Int(c - 1, r));
	}

	void MoveRight()
	{
		if (IsInvalidMove(c + 1, r))
			throw new RuntimeException("Can't move robot Right");
		waypoints.Enqueue(new Vector2Int(c + 1, r));
	}

	bool IsInvalidMove(int c, int r)
	{
		return c < 0 || c >= levelLoader.Level.W - 1 || r < 0 || r >= levelLoader.Level.H - 1 || levelLoader.Level.Grid[r, c].prefab.name == "Wall";
    }

	void CheckKey()
	{
        if (Input.GetKey(KeyCode.Keypad8))
			MoveUp();

		if (Input.GetKey(KeyCode.Keypad2))
			MoveDown();

        if (Input.GetKey(KeyCode.Keypad4))
			MoveLeft();

        if (Input.GetKey(KeyCode.Keypad6))
			MoveRight();
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
