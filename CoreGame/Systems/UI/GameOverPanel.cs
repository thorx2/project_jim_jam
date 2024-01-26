using System;
using CoreGame.GameSystems;
using CoreGame.GameSystems.EventManagement;
using Godot;

public partial class GameOverPanel : Control
{
    [Export]
    private Button continueRetryButton;

    [Export]
    private Button quitButton;

    private bool isWinDay;

    public override void _Ready()
    {
        quitButton.Pressed += OnQuitPressed;
        continueRetryButton.Pressed += OnContinueQuitPressed;
    }

    private void OnContinueQuitPressed()
    {
        if (isWinDay)
        {
            MasterSignalBus.GetInstance.LoadMapEvent?.Invoke(GameRuntimeParameters.GameDay);
            Visible = false;
        }
        else
        {
            MasterSignalBus.GetInstance.LoadMapEvent?.Invoke(GameRuntimeParameters.GameDay);
            Visible = false;
        }
    }

    private void OnQuitPressed()
    {
        GetTree().Quit();
    }

    internal void ShowPanel(bool obj)
    {
        isWinDay = obj;
        continueRetryButton.Text = obj ? "Start next day" : "Retry Day";

        if (GameRuntimeParameters.GameDay == 5)
        {
            continueRetryButton.Text = "Exit To Menu";
            GD.Print("Game Ends here!!!!");
        }
    }
}