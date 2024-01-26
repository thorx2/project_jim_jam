using CoreGame.GameSystems.EventManagement;
using Godot;
using System;

public partial class UIManager : Control
{
    [Export]
    private Control mainMenu;

    [Export]
    private Control inGameMenu;

    public override void _Ready()
    {
        MasterSignalBus.GetInstance.StartGameEvent += OnGameStart;
        mainMenu.Visible = true;
        inGameMenu.Visible = false;
    }

    private void OnGameStart()
    {
        mainMenu.Visible = false;
        inGameMenu.Visible = true;
    }

}
