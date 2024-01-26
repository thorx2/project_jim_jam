using System;
using CoreGame.GameSystems.EventManagement;
using CoreGame.Pathfinding;
using Godot;
using Godot.Collections;

namespace CoreGame.GameSystems;

public partial class GameLevelManager : SubViewport
{
    [Export]
    public Array<LevelData> gameMaps;

    private Player playerCharacter;

    private Vector2 spawnPoint;

    private Node activeMap;

    [Export]
    private Node2D gameplayMasterParent;

    public override void _Ready()
    {
        MasterSignalBus.GetInstance.LevelLoadedEvent += OnNewLevelLoaded;
        MasterSignalBus.GetInstance.LoadMapEvent += LoadNewMap;
        MasterSignalBus.GetInstance.OnDayOver += OnDayOver;
    }

    private void OnDayOver(bool obj)
    {
        playerCharacter.SnapCharacterToTileOnMap(spawnPoint);
        playerCharacter.ResetGameCharacter(playerCharacter.GlobalPosition);
        activeMap.QueueFree();
    }


    private void OnNewLevelLoaded(Vector2 newPos, TileMap map)
    {
        playerCharacter ??= GameManager.GetInstance.GetPlayerRef;
        PathGenerator.GetPathGeneratorInstance.SetupGameMap(map);
        spawnPoint = newPos;
        playerCharacter.SnapCharacterToTileOnMap(newPos);
    }


    public void LoadNewMap(int map)
    {
        if (GameRuntimeParameters.GameDay > 5)
        {
            GameRuntimeParameters.ResetGameParameters();
            MasterSignalBus.HardResetSystem();
            playerCharacter = null;
        }
        else
        {
            map = Math.Clamp(map, 0, gameMaps.Count - 1);
            activeMap = gameMaps[map].LoadMap.Instantiate();
            gameplayMasterParent.AddChild(activeMap);
        }
    }
}
