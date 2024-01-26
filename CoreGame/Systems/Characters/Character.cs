using System;
using CoreGame.GameSystems.EventManagement;
using CoreGame.Pathfinding;
using Godot;

public partial class Character : CharacterBody2D
{
    [Export]
    private AnimatedSprite2D characterVisual;

    [Export]
    private ECharacterType characterType;

    [Export]
    private RayCast2D pathCheckCast;

    public override void _Ready()
    {
        if (characterType != ECharacterType.EPlayer)
        {
            MasterSignalBus.GetInstance.LevelLoadedEvent += OnMapLoaded;
        }
    }

    public override void _ExitTree()
    {
        if (characterType != ECharacterType.EPlayer)
        {
            MasterSignalBus.GetInstance.LevelLoadedEvent -= OnMapLoaded;
        }
    }

    private void OnMapLoaded(Vector2 vector, TileMap map)
    {
        if (Visible)
        {
            SnapCharacterToTileOnMap(GlobalPosition);
        }
    }

    public void SnapCharacterToTileOnMap(Vector2 pos)
    {
        var pointPos = PathGenerator.GetPathGeneratorInstance.GetMapPointForPosition(pos);
        GlobalPosition = PathGenerator.GetPathGeneratorInstance.GetPointPositionCentered(pointPos);
    }


    public override void _Process(double delta)
    {
        if (pathCheckCast.IsColliding())
        {
            var p = pathCheckCast.GetCollider() as Player;
            if (p != null)
            {
                GD.Print("Player Found");
            }
            switch (characterType)
            {
                case ECharacterType.EGrey:
                    break;
                case ECharacterType.EColored:
                    break;
            }
        }
    }
}
