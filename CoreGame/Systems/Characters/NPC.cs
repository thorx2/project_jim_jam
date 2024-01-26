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
        if (pathCheckCast.IsColliding())
        {
            var p = pathCheckCast.GetCollider() as Player;
            if (p != null)
            {
                switch (characterType)
                {
                    case ECharacterType.EGrey:
                    MasterSignalBus.GetInstance.StartQteEvent?.Invoke(characterType);
                        break;
                    case ECharacterType.EColored:
                        break;
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