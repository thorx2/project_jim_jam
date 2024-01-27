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
		else
		{
			animatedSprite2D.SpeedScale = 0;
			animatedSprite2D.Frame = 0;
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
			animatedSprite2D.SpeedScale = 1;
			if (tileSize < 0)
			{
				tileSize = (int)PathGenerator.GetPathGeneratorInstance.GetNavCellSize().X;
			}

			pathCheckCast.TargetPosition = direction * tileSize;
			pathCheckCast.ForceRaycastUpdate();
			var currentCell = PathGenerator.GetPathGeneratorInstance.GetMapPointForPosition(parentMovingNode.GlobalPosition);
			var targetTile = new Vector2I((int)(currentCell.X + direction.X), (int)(currentCell.Y + direction.Y));
			var tileData = PathGenerator.GetPathGeneratorInstance.GetTileData(0, targetTile);
			if (tileData != null && tileData.GetCustomData("Walkable").AsBool() && !pathCheckCast.IsColliding())
			{
				targetPosition = PathGenerator.GetPathGeneratorInstance.GetPointPositionCentered(targetTile);
				isMoving = (parentMovingNode.Position - targetPosition).LengthSquared() > 0.1f;
			}
			if (direction.X != 0)
			{
				animatedSprite2D.Play(direction.X < 0 ? "walk_left" : "walk_right");
			}
			else if (direction.Y != 0)
			{
				animatedSprite2D.Play(direction.Y < 0 ? "walk_up" : "walk_down");
			}
		}
		else if (direction == Vector2.Zero)
		{
			animatedSprite2D.SpeedScale = 0;
			animatedSprite2D.Frame = 0;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (isMoving)
		{
			if ((parentMovingNode.Position - targetPosition).LengthSquared() < 0.1)
			{
				isMoving = false;
				parentMovingNode.GlobalPosition = targetPosition;
			}
			else
			{
				parentMovingNode.GlobalPosition = parentMovingNode.GlobalPosition.MoveToward(targetPosition, Speed);
			}
		}
	}

	public void ForceSetTargetPosition(Vector2 pos)
	{
		targetPosition = pos;
	}
}
