using CoreGame.GameSystems.EventManagement;
using Godot;

public partial class LevelController : Node2D
{
    [Export]
    private Marker2D playerSpawnPoint;

    [Export]
    private TileMap levelMapReference;

    public override void _Ready()
    {
        MasterSignalBus.GetInstance.LevelLoadedEvent?.Invoke(playerSpawnPoint.GlobalPosition, levelMapReference);
    }
}
