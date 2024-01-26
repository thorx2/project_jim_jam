using System;
using CoreGame.GameSystems;
using CoreGame.GameSystems.EventManagement;
using Godot;

public partial class QteManager : Control
{
    [Export]
    private ProgressBar currentProgressBar;

    [Export]
    private ProgressBar failurePlaceHolderBar;

    [Export]
    private Timer qteCoolDownTimer;

    [Export]
    private QteWindow qteWindow;

    private bool canQte = false;

    public override void _Ready()
    {
        MasterSignalBus.GetInstance.StartQteEvent += OnQteRequested;

        MasterSignalBus.GetInstance.OnQteCompleteEvent += OnQteCompleted;

        MasterSignalBus.GetInstance.StartGameEvent += OnNewGameStart;

        qteCoolDownTimer.Timeout += OnQteTimerTimeout;

        qteCoolDownTimer.Start(1);
    }

    private void OnQteTimerTimeout()
    {
        canQte = true;
    }

    private void OnNewGameStart()
    {
        failurePlaceHolderBar.Value = 0;
        currentProgressBar.Value = 0;
    }

    private void OnQteCompleted(ECharacterType type, EQteCompleteState state)
    {
        if (state == EQteCompleteState.EQteFailed)
        {
            switch (type)
            {
                case ECharacterType.EGrey:
                    GameRuntimeParameters.GossipSpread += (0.02f + GameRuntimeParameters.GossipSpread * GameRuntimeParameters.GossipSpread);
                    currentProgressBar.Value += GameRuntimeParameters.GossipSpread;
                    break;
                case ECharacterType.EColored:
                    GameRuntimeParameters.GossipSpread += (0.15f + GameRuntimeParameters.GossipSpread * GameRuntimeParameters.GossipSpread);
                    currentProgressBar.Value += GameRuntimeParameters.GossipSpread;
                    break;
            }
        }
        GameManager.GetInstance.GetPlayerRef.CurrentPlayerState = EPlayerState.EPlayerWalking;
        qteCoolDownTimer.Start();
    }

    private void OnQteRequested(ECharacterType type, float qteTime)
    {
        if (canQte)
        {
            GameManager.GetInstance.GetPlayerRef.CurrentPlayerState = EPlayerState.EPlayerInQTE;
            canQte = false;
            qteWindow.Show(type, qteTime);
        }
    }
}