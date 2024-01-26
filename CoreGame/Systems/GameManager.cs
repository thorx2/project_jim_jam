using CoreGame.GameSystems.EventManagement;
using Godot;
using System;

namespace CoreGame.GameSystems;

public partial class GameManager : Node2D
{
    #region Singleton Access
    private static GameManager instance;

    public static GameManager GetInstance
    {
        get => instance;
    }
    #endregion

    #region Player Spawner

    [Export]
    private PackedScene playerScene;

    [Export]
    private Node2D gameplayParent;

    private Character playerRef = null;

    public Character GetPlayerRef
    {
        get => playerRef;
    }

    public Character CreatePlayerCharacter()
    {

        if (playerRef == null)
        {
            playerRef = playerScene.Instantiate() as Character;
            gameplayParent.AddChild(playerRef);
        }

        return playerRef;
    }
    #endregion

    #region Godot region
    public override void _Ready()
    {
        instance = this;
        MasterSignalBus.GetInstance.StartGame += OnStartNewGame;
    }
    #endregion

    #region Functional
    public void OnStartNewGame()
    {
        CreatePlayerCharacter();
    }
    #endregion
}
