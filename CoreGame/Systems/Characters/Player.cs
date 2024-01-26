using CoreGame.GameSystems.EventManagement;
using Godot;

public partial class Player : Character
{
    [Export]
    private AnimatedSprite2D characterVisual;

    public EPlayerState CurrentPlayerState;
    
    public override void _Ready()
    {
        base._Ready();
        MasterSignalBus.GetInstance.LoadMapEvent?.Invoke(0);
        CurrentPlayerState = EPlayerState.EPlayerWalking;
    }

    public void ResetGameCharacter()
    {
        
    }
}