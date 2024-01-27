using System;
using CoreGame.GameSystems.EventManagement;
using Godot;

public partial class GameReloaderSystem : Node2D
{
	[Export]
	private PackedScene game;

	private Node2D instantiatedGame;

	public override void _Ready()
	{
		DisplayServer.WindowSetTitle("Hush Hush High");
		instantiatedGame = game.Instantiate() as Node2D;
		MasterSignalBus.GetInstance.GameHardReset += OnGameHardReset;
		AddChild(instantiatedGame);
	}

	private void OnGameHardReset()
	{
		instantiatedGame.QueueFree();
		instantiatedGame = game.Instantiate() as Node2D;
		MasterSignalBus.GetInstance.GameHardReset += OnGameHardReset;
		AddChild(instantiatedGame);
	}
}
