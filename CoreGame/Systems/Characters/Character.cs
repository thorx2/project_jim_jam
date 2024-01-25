using CoreGame.Pathfinding;
using Godot;
using System;

public partial class Character : CharacterBody2D
{
    [Export]
    public float Speed = 300.0f;

    [Export]
    private AnimatedSprite2D characterVisual;

    [Export]
    private bool isPlayer;

    [Export]
    private int tileSize;

    private bool isMoving = false;

    private Vector2 targetPosition;

    [Export]
    private RayCast2D pathCheckCast;

    public override void _Ready()
    {
        //Just one side as this is a square cell
        if (PathGenerator.GetPathGeneratorInstance != null)
        {
            tileSize = (int)PathGenerator.GetPathGeneratorInstance.GetNavCellSize().X;
        }
    }

    public override void _Process(double delta)
    {
        Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        // If any input
        if (direction != Vector2.Zero && !isMoving)
        {
            pathCheckCast.TargetPosition = direction * 16;
            pathCheckCast.ForceRaycastUpdate();

            var currentCell = PathGenerator.GetPathGeneratorInstance.GetMapPointForPosition(GlobalPosition);
            var targetTile = new Vector2I((int)(currentCell.X + direction.X), (int)(currentCell.Y + direction.Y));
            var tileData = PathGenerator.GetPathGeneratorInstance.GetTileData(0, targetTile);
            if (tileData != null && tileData.GetCustomData("Walkable").AsBool() && !pathCheckCast.IsColliding())
            {
                targetPosition = PathGenerator.GetPathGeneratorInstance.GetPointPositionCentered(targetTile);
                isMoving = (Position - targetPosition).LengthSquared() > 0.1f;
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (isMoving)
        {
            if ((Position - targetPosition).LengthSquared() < 0.1)
            {
                isMoving = false;
                Position = targetPosition;
            }
            else
            {
                Position = Position.MoveToward(targetPosition, Speed);
            }
        }
    }
}
