using CoreGame.GameSystems.EventManagement;

public partial class Player : Character
{
    public override void _Ready()
    {
        base._Ready();
        MasterSignalBus.GetInstance.LoadMapEvent?.Invoke(0);
    }
}