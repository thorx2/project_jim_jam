using CoreGame.Pathfinding;
using Godot;

public partial class MovementSubsystem : Node
{
	[Export]
	private Player parentMovingNode;

	[Export]
	public float Speed = 300.0f;

	[Export]
	private AnimatedSprite2D animatedSprite2D;

	private bool isMoving = false;

	private bool wasMovingVertical = false;
	private Vector2 direction;

	private Vector2 targetPosition;

	[Export]
	private RayCast2D pathCheckCast;

	private int tileSize = -1;

	public override void _Process(double delta)
	{
		if (parentMovingNode.CurrentPlayerState == EPlayerState.EPlayerWalking)
		{
			PlayerMovement();
		}
	}

	private void PlayerMovement()
	{
		Vector2 inputDirection = new Vector2(Input.GetAxis("move_left", "move_right"), Input.GetAxis("move_up", "move_down"));

		{   // Fuckall way to negate diagonal movement
			if (direction.X == 0f && inputDirection.X != 0f)
				wasMovingVertical = false;

			if (direction.Y == 0f && inputDirection.Y != 0f)
				wasMovingVertical = true;

			direction = inputDirection;

			if (direction.X != 0f && direction.Y != 0f)
			{
				if (wasMovingVertical)
					direction = new Vector2(0f, inputDirection.Y);
				else
					direction = new Vector2(inputDirection.X, 0f);
			}
		}

		if (direction != Vector2.Zero && !isMoving && PathGenerator.GetPathGeneratorInstance != null)
		{
			if (tileSize < 0)
			{
				tileSize = (int)PathGenerator.GetPathGeneratorInstance.GetNavCellSize().X;
			}

			var currentCell = PathGenerator.GetPathGeneratorInstance.GetMapPointForPosition(parentMovingNode.GlobalPosition);
			var targetTile = new Vector2I((int)(currentCell.X + direction.X), (int)(currentCell.Y + direction.Y));
			var tileData = PathGenerator.GetPathGeneratorInstance.GetTileData(0, targetTile);

			pathCheckCast.TargetPosition = direction * tileSize;
			pathCheckCast.ForceRaycastUpdate();

			if (tileData != null && tileData.GetCustomData("Walkable").AsBool() && !pathCheckCast.IsColliding())
			{
				targetPosition = PathGenerator.GetPathGeneratorInstance.GetPointPositionCentered(targetTile);
				isMoving = true;

				if (direction.X != 0)
				{
					animatedSprite2D.Play(direction.X < 0 ? "walk_left" : "walk_right");
					animatedSprite2D.SpeedScale = 1;
				}
				else if (direction.Y != 0)
				{
					animatedSprite2D.Play(direction.Y < 0 ? "walk_up" : "walk_down");
					animatedSprite2D.SpeedScale = 1;
				}
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!isMoving)
		{
			return;
		}

		float sqrDistanceToTarget = (parentMovingNode.Position - targetPosition).LengthSquared();

		if (sqrDistanceToTarget < 0.01f)
		{
			parentMovingNode.GlobalPosition = targetPosition;
			StopCharacter();
			return;
		}

		parentMovingNode.GlobalPosition = parentMovingNode.GlobalPosition.MoveToward(targetPosition, Speed);
	}

	public void StopCharacter()
	{
		isMoving = false;

		// Pause animation on the first frame (that is treated as the idle animation)
		animatedSprite2D.SpeedScale = 0;
		animatedSprite2D.Frame = 0;
	}

	public void ForceSetTargetPosition(Vector2 pos)
	{
		targetPosition = pos;
	}
}
