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

    private NPC activeNPC;

    private bool canQte = false;

    public override void _Ready()
    {
        MasterSignalBus.GetInstance.StartQteEvent += OnQteRequested;

        MasterSignalBus.GetInstance.OnQteCompleteEvent += OnQteCompleted;

        MasterSignalBus.GetInstance.StartGameEvent += OnNewGameStart;

        MasterSignalBus.GetInstance.OnDayOver += OnDayOver;

        qteCoolDownTimer.OneShot = true;

        qteCoolDownTimer.Timeout += OnQteTimerTimeout;

        qteCoolDownTimer.Start();
    }

    private void OnDayOver(bool obj)
    {
        if (!obj)
        {
            currentProgressBar.Value = 0;
        }
    }


    private void OnQteTimerTimeout()
    {
        canQte = true;
    }

    public override void _Process(double delta)
    {
        if (failurePlaceHolderBar.Value != GameRuntimeParameters.MaxTolerableSpread)
        {
            failurePlaceHolderBar.Value = GameRuntimeParameters.MaxTolerableSpread;
        }
    }

    private void OnNewGameStart()
    {
        failurePlaceHolderBar.Value = GameRuntimeParameters.MaxTolerableSpread;
        currentProgressBar.Value = 0;
    }

    private void OnQteCompleted(ECharacterType type, EQteCompleteState state)
    {
        canQte = false;
        if (state == EQteCompleteState.EQteFailed)
        {
            switch (type)
            {
                case ECharacterType.EGrey:
                    GameRuntimeParameters.GossipSpread += GameRuntimeParameters.GreyFailSpread + GameRuntimeParameters.GossipSpread;
                    currentProgressBar.Value += GameRuntimeParameters.GossipSpread;
                    if (activeNPC.GetECharacterType == ECharacterType.EColored &&
                        activeNPC.GetSpecialNPCStyle == GameManager.GetInstance.GetActiveObjective.SpecialNpcOfDay)
                    {
                        qteWindow.HideMiniGameWindows();
                        qteCoolDownTimer.Stop();
                        GameManager.GetInstance.GetPlayerRef.CurrentPlayerState = EPlayerState.EPlayerInMiniGame;
                        canQte = false;
                        qteWindow.Show(ECharacterType.EColored, activeNPC.GetQteDuration.Y, activeNPC);
                    }
                    else
                    {
                        QteFlowComplete();
                        activeNPC.TriggerGossipBurst();
                    }
                    break;
                case ECharacterType.EColored:
                    GameRuntimeParameters.GossipSpread += GameRuntimeParameters.ColorFailSpread + GameRuntimeParameters.GossipSpread;
                    currentProgressBar.Value += GameRuntimeParameters.GossipSpread;
                    QteFlowComplete();
                    activeNPC.TriggerGossipBurst();
                    break;
            }
        }
        else if (state == EQteCompleteState.EQteSuccess)
        {
            QteFlowComplete();
        }

        if (GameRuntimeParameters.GossipSpread > GameRuntimeParameters.MaxTolerableSpread)
        {
            MasterSignalBus.GetInstance.OnDayOver?.Invoke(false);
        }
    }

    private void QteFlowComplete()
    {
        GameManager.GetInstance.GetPlayerRef.CurrentPlayerState = EPlayerState.EPlayerWalking;
        qteCoolDownTimer.Stop();
        qteCoolDownTimer.Start();
        qteWindow.HideMiniGameWindows();
    }

    private void OnQteRequested(ECharacterType type, NPC npc)
    {
        if (canQte)
        {
            activeNPC = npc;
            GameManager.GetInstance.GetPlayerRef.CurrentPlayerState = EPlayerState.EPlayerInQTE;
            canQte = false;
            qteWindow.Show(ECharacterType.EGrey, npc.GetQteDuration.X, npc);
        }
    }
}