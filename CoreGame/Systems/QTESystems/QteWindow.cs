using System;
using System.Runtime.InteropServices;
using CoreGame.GameSystems.EventManagement;
using Godot;

public partial class QteWindow : Control
{
    [Export]
    private Timer qteTimer;

    [Export]
    private AnimatedSprite2D quickActionImage;

    private ECharacterType lastCharacterType;

    private int randomGameKey = -1;

    private Random rnd = new Random();

    public override void _Ready()
    {
        qteTimer.Timeout += OnQteTimeout;
        qteTimer.OneShot = true;
    }



    private void OnQteTimeout()
    {
        MasterSignalBus.GetInstance.OnQteCompleteEvent(lastCharacterType, EQteCompleteState.EQteFailed);
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
                            MasterSignalBus.GetInstance.OnQteCompleteEvent(lastCharacterType, EQteCompleteState.EQteSuccess);
                            Visible = false;
                        }
                        qteTimer.Stop();
                        break;
                    case Key.A:
                        if (randomGameKey == 1)
                        {
                            MasterSignalBus.GetInstance.OnQteCompleteEvent(lastCharacterType, EQteCompleteState.EQteSuccess);
                            Visible = false;
                        }
                        qteTimer.Stop();
                        break;
                    case Key.S:
                        if (randomGameKey == 2)
                        {
                            MasterSignalBus.GetInstance.OnQteCompleteEvent(lastCharacterType, EQteCompleteState.EQteSuccess);
                            Visible = false;
                        }
                        qteTimer.Stop();
                        break;
                    case Key.D:
                        if (randomGameKey == 3)
                        {
                            MasterSignalBus.GetInstance.OnQteCompleteEvent(lastCharacterType, EQteCompleteState.EQteSuccess);
                            Visible = false;
                        }
                        qteTimer.Stop();
                        break;
                }
            }
        }
    }
}