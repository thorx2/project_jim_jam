using CoreGame.GameSystems.EventManagement;
using Godot;

public partial class Player : Character
{
	[Export]
	private AnimatedSprite2D characterVisual;
	[ExportSubgroup("Debug Display Data")]
	[Export]
	public EPlayerState CurrentPlayerState;

	[Export]
	private MovementSubsystem movementSubsystem;
	
	public override void _Ready()
	{
		base._Ready();
		MasterSignalBus.GetInstance.LoadMapEvent?.Invoke(0);
		CurrentPlayerState = EPlayerState.EPlayerWalking;
	}

	public void ResetGameCharacter(Vector2 atPosition)
	{
		GlobalPosition = atPosition;
		movementSubsystem.SetPhysicsProcess(false);
		movementSubsystem.SetProcess(false);
		movementSubsystem.ForceSetTargetPosition(atPosition);
	}

	public override void SnapCharacterToTileOnMap(Vector2 pos)
	{
		movementSubsystem.SetPhysicsProcess(true);
		movementSubsystem.SetProcess(true);
		base.SnapCharacterToTileOnMap(pos);
	}
}
