using CoreGame.GameSystems.EventManagement;
using Godot;
using System;

public partial class MainMenuController : Control
{
    [Export]
    private Button startGameButton;

    public override void _Ready()
    {
        startGameButton.Pressed += OnStartGame;
    }

    private void OnStartGame()
    {
        MasterSignalBus.GetInstance.StartGame?.Invoke();
    }
}
