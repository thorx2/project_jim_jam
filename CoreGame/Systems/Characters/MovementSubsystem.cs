using CoreGame.Pathfinding;
using Godot;

public partial class MovementSubsystem : Node
{
    [Export]
    private Node2D parentMovingNode;

    [Export]
    public float Speed = 300.0f;

    private bool isMoving = false;

    private Vector2 targetPosition;

    [Export]
    private RayCast2D pathCheckCast;

    private int tileSize = -1;


    public override void _Process(double delta)
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        if (direction != Vector2.Zero && !isMoving && PathGenerator.GetPathGeneratorInstance != null)
        {
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
}