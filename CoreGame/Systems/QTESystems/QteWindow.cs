using System;
using System.Runtime.InteropServices;
using CoreGame.GameSystems.EventManagement;
using Godot;

public partial class QteWindow : Control
{
    [ExportGroup("Element reference")]
    [Export]
    private ProgressBar timerBar;
    [ExportGroup("Element reference")]
    [Export]
    private ProgressBar mashBar;
    [ExportGroup("Element reference")]
    [Export]
    private Timer qteTimer;
    [ExportGroup("Element reference")]
    [Export]
    private AnimatedSprite2D quickActionImage;

    [ExportGroup("Gameplay Configurations")]
    [Export]
    private float baseDecayRate;
    [ExportGroup("Gameplay Configurations")]
    [Export]
    private float baseGainRate;

    private Random rnd = new Random();
    private float mashProgress = 0.0f;
    private int randomGameKey = -1;
    private ECharacterType lastCharacterType;

    public override void _Ready()
    {
        qteTimer.Timeout += OnQteTimeout;
        qteTimer.OneShot = true;
        timerBar.MaxValue = 1;
        timerBar.Value = 0;
    }

    public override void _Process(double delta)
    {
        if (Visible)
        {
            if (mashProgress >= 0.98)
            {
                OnQTEComplete();
                MasterSignalBus.GetInstance.OnQteCompleteEvent(lastCharacterType, EQteCompleteState.EQteSuccess);
                qteTimer.Stop();
                Visible = false;
            }
            timerBar.Value = qteTimer.TimeLeft / qteTimer.WaitTime;
            mashBar.Value = mashProgress;
            mashProgress -= (baseDecayRate + GameRuntimeParameters.GossipSpread) * (float)delta;
            mashProgress = Math.Clamp(mashProgress, 0, 1);
        }
    }

    private void OnQTEComplete()
    {
        mashProgress = 0f;
        timerBar.Value = 0f;
        mashBar.Value = 0f;
    }



    private void OnQteTimeout()
    {
        MasterSignalBus.GetInstance.OnQteCompleteEvent(lastCharacterType, EQteCompleteState.EQteFailed);
        OnQTEComplete();
        Visible = false;
        qteTimer.Stop();
    }

    public void Show(ECharacterType type, float duration)
    {
        Visible = true;
        qteTimer.Stop();
        qteTimer.WaitTime = duration;
        qteTimer.Start();
        lastCharacterType = type;
        randomGameKey = rnd.Next(0, 3);
        quickActionImage.Frame = randomGameKey;
    }

    public override void _Input(InputEvent @event)
    {
        if (Visible)
        {
            if (@event is InputEventKey && @event.IsPressed())
            {
                var g = @event as InputEventKey;
                switch (g.Keycode)
                {
                    case Key.W:
                        if (randomGameKey == 0)
                        {
                            mashProgress += baseGainRate;
                        }
                        break;
                    case Key.A:
                        if (randomGameKey == 1)
                        {
                            mashProgress += baseGainRate;
                        }
                        break;
                    case Key.S:
                        if (randomGameKey == 2)
                        {
                            mashProgress += baseGainRate;
                        }
                        break;
                    case Key.D:
                        if (randomGameKey == 3)
                        {
                            mashProgress += baseGainRate;
                        }
                        break;
                }
            }
        }
    }
}