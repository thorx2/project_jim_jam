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

    [ExportCategory("Gameplay Configuration")]
    [Export]
    private float qteDuration;


    public override void _Ready()
    {
        base._Ready();
        MasterSignalBus.GetInstance.LevelLoadedEvent += OnMapLoaded;
        visualSprite.Frame = characterFrame;
    }

    public override void _Process(double delta)
    {
        // if (!speechBubble.Visible)
        {
            foreach (var ray in pathCheckCast)
            {
                if (ray.IsColliding())
                {
                    var p = ray.GetCollider() as Player;
                    if (p != null && p.CurrentPlayerState == EPlayerState.EPlayerWalking)
                    {
                        MasterSignalBus.GetInstance.StartQteEvent?.Invoke(characterType, qteDuration);
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
}