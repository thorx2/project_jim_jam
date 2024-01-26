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
        dayText.Text = $"Day {GameRuntimeParameters.GameDay}";
        randomDayText.Text = randomListOfMessages[rnd.Next(0, randomListOfMessages.Length)];
    }

    private void OnDisplayTimeout()
    {
        Visible = false;
    }

}
