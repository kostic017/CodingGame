using UnityEngine;

public class RobotTarget
{
    private bool moved;
	private bool rotated;
	private bool calculated;

	private Vector2Int tile;
	private Vector3 position;
	private Quaternion rotation;

	private readonly Robot robot;
	private readonly LevelLoader levelLoader;

	internal RobotTarget(Robot robot, Vector2Int tile, LevelLoader levelLoader)
	{
		this.tile = tile;
		this.robot = robot;
		this.levelLoader = levelLoader;
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
		position = levelLoader.GetTilePosition(tile.y, tile.x);
		rotation = Quaternion.LookRotation(position - robot.transform.position);
		position.y = robot.transform.position.y;
		calculated = true;
    }

	internal bool Rotate()
	{
		if (rotated) return true;
		if (Quaternion.Angle(robot.transform.rotation, rotation) < 0.001f) rotated = true;
		robot.transform.rotation = Quaternion.RotateTowards(robot.transform.rotation, rotation, robot.RotationSpeed * Time.deltaTime);
		return rotated;
	}

	internal bool Move()
	{
		if (moved) return true;
		if (Vector3.Distance(robot.transform.position, position) < 0.001f) moved = true;
		robot.transform.position = Vector3.MoveTowards(robot.transform.position, position, robot.MoveSpeed * Time.deltaTime);
		return moved;
	}
}
