using System;
using CoreGame.GameSystems.EventManagement;
using CoreGame.Pathfinding;
using Godot;

public partial class Character : CharacterBody2D
{
    [Export]
    protected ECharacterType characterType;

    public ECharacterType GetECharacterType
    {
        get => characterType;
    }

    [Export]
    protected RayCast2D[] pathCheckCast;

    public void SnapCharacterToTileOnMap(Vector2 pos)
    {
        var pointPos = PathGenerator.GetPathGeneratorInstance.GetMapPointForPosition(pos);
        GlobalPosition = PathGenerator.GetPathGeneratorInstance.GetPointPositionCentered(pointPos);
    }
}
