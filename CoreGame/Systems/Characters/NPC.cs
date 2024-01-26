using System;
using CoreGame.GameSystems.EventManagement;
using Godot;
public partial class NPC : Character
{
    [ExportGroup("Reference")]
    [Export]
    private Sprite2D visualSprite;
    [ExportGroup("Reference")]
    [Export]
    private int characterFrame;
    [ExportGroup("Reference")]
    [Export]
    private Sprite2D speechBubble;

    [ExportGroup("Reference")]
    [Export]
    private Area2D gossipBubble;

    [ExportGroup("Reference")]
    [Export]
    private CollisionShape2D gossipSpreadCircle;

    [ExportCategory("Gameplay Configuration")]
    [Export]
    private float gossipBurstRadius;
    [Export]
    private float qteDuration;

    public float GetQteDuration
    {
        get => qteDuration;
    }


    public override void _Ready()
    {
        base._Ready();
        MasterSignalBus.GetInstance.LevelLoadedEvent += OnMapLoaded;
        visualSprite.Frame = characterFrame;

        gossipBubble.AreaEntered += OnGossipHitCharacter;
    }

    private void OnGossipHitCharacter(Area2D area)
    {
        speechBubble.Visible = true;

        foreach (var ray in pathCheckCast)
        {
            ray.Visible = false;
        }

        GameRuntimeParameters.GossipSpread += 0.05f;
    }

    public override void _Process(double delta)
    {
        if (!speechBubble.Visible)
        {
            foreach (var ray in pathCheckCast)
            {
                if (ray.IsColliding())
                {
                    var p = ray.GetCollider() as Player;
                    if (p != null && p.CurrentPlayerState == EPlayerState.EPlayerWalking)
                    {
                        MasterSignalBus.GetInstance.StartQteEvent?.Invoke(characterType, this);
                    }
                }
            }
        }
    }

    public override void _ExitTree()
    {
        MasterSignalBus.GetInstance.LevelLoadedEvent -= OnMapLoaded;
    }

    private void OnMapLoaded(Vector2 vector, TileMap map)
    {
        if (Visible)
        {
            SnapCharacterToTileOnMap(GlobalPosition);
        }
    }

    internal void TriggerGossipBurst()
    {
        speechBubble.Visible = true;
        foreach (var ray in pathCheckCast)
        {
            ray.Visible = false;
        }
        (gossipSpreadCircle.Shape as CircleShape2D).Radius = gossipBurstRadius;
    }
}