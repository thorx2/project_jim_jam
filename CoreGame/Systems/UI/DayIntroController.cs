using Godot;
using System;
using System.Collections.Generic;

public partial class DayIntroController : Control
{
    [Export]
    private Timer displayTimeoutTimer;

    [Export]
    private Label dayText;

    [Export]
    private Label randomDayText;

    [Export]
    private string[] randomListOfMessages;

    private Random rnd = new();

    public override void _Ready()
    {
        displayTimeoutTimer.OneShot = true;
        displayTimeoutTimer.Timeout += OnDisplayTimeout;
    }

    public void ShowDayTimer()
    {
        displayTimeoutTimer.Start();
        int x = GameRuntimeParameters.GameDay == 0 ? GameRuntimeParameters.GameDay + 1 : GameRuntimeParameters.GameDay;
        dayText.Text = $"Day {x}";
        randomDayText.Text = randomListOfMessages[x - 1];
    }

    private void OnDisplayTimeout()
    {
        Visible = false;
    }

}
