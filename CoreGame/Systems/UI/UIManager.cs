using CoreGame.GameSystems.EventManagement;
using Godot;
using System;

public partial class UIManager : Control
{
    [Export]
    private Label dayLabel;

    [Export]
    private Label questLabel;

    [Export]
    private Control mainMenu;

    [Export]
    private Control gameplayUIRef;

    [Export]
    private GameOverPanel gameOverPanel;

    private int lastShownDay = -1;

    public override void _Ready()
    {
        MasterSignalBus.GetInstance.StartGameEvent += OnGameStart;

        MasterSignalBus.GetInstance.OnDayOver += OnDayOver;

        mainMenu.Visible = true;
        gameplayUIRef.Visible = false;
        gameOverPanel.Visible = false;
    }


    public override void _Process(double delta)
    {
        if (lastShownDay != GameRuntimeParameters.GameDay)
        {
            dayLabel.Text = $"Day {GameRuntimeParameters.GameDay}";
            lastShownDay = GameRuntimeParameters.GameDay;
        }
    }


    private void OnDayOver(bool obj)
    {
        gameOverPanel.Visible = true;
        gameOverPanel.ShowPanel(obj);
    }


    private void OnGameStart()
    {
        mainMenu.Visible = false;
        gameplayUIRef.Visible = true;
    }

}
