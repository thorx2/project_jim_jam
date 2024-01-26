using System;
using CoreGame.GameSystems.EventManagement;
using Godot;

public partial class QteManager : Control
{
    [Export]
    private ProgressBar currentProgressBar;

    [Export]
    private ProgressBar failurePlaceHolderBar;

    public override void _Ready()
    {
        MasterSignalBus.GetInstance.StartQteEvent += OnQteRequested;

        MasterSignalBus.GetInstance.OnQteCompleteEvent += OnQteCompleted;

        MasterSignalBus.GetInstance.StartGameEvent += OnNewGameStart;
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
                    break;
                case ECharacterType.EColored:
                    break;
            }
        }
    }

    private void OnQteRequested(ECharacterType type)
    {

    }
}