using CoreGame.GameSystems.EventManagement;
using Godot;
public partial class NPC : Character
{
    [Export]
    private Sprite2D visualSprite;

    [Export]
    private int characterFrame;


    public override void _Ready()
    {
        base._Ready();
        MasterSignalBus.GetInstance.LevelLoadedEvent += OnMapLoaded;
        visualSprite.Frame = characterFrame;
    }

    public override void _Process(double delta)
    {
        foreach (var ray in pathCheckCast)
        {
            if (ray.IsColliding())
            {
                var p = ray.GetCollider() as Player;
                if (p != null && p.CurrentPlayerState == EPlayerState.EPlayerWalking)
                {
                    MasterSignalBus.GetInstance.StartQteEvent?.Invoke(characterType);
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