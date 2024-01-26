using CoreGame.GameSystems.EventManagement;
using Godot;
using System;

public partial class MainMenuController : Control
{
	[Export]
	private Button startGameButton;

	[Export]
	private Button quitButton;

	public override void _Ready()
	{
		startGameButton.Pressed += OnStartGame;
		quitButton.Pressed += QuitGame;
	}

	private void QuitGame()
	{
		GetTree().Quit();
	}

	private void OnStartGame()
	{
		MasterSignalBus.GetInstance.StartGameEvent?.Invoke();
	}
}
