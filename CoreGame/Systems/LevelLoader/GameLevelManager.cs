using CoreGame.GameSystems.EventManagement;
using CoreGame.Pathfinding;
using Godot;
using Godot.Collections;

namespace CoreGame.GameSystems;

public partial class GameLevelManager : SubViewport
{
    [Export]
    public Array<LevelData> gameMaps;

    private Character playerCharacter;

    [Export]
    private Node2D gameplayMasterParent;

    public override void _Ready()
    {
        MasterSignalBus.GetInstance.LevelLoadedEvent += OnNewLevelLoaded;
        MasterSignalBus.GetInstance.LoadMapEvent += LoadNewMap;
    }

    private void OnNewLevelLoaded(Vector2 newPos, TileMap map)
    {
        playerCharacter ??= GameManager.GetInstance.GetPlayerRef;
        PathGenerator.GetPathGeneratorInstance.SetupGameMap(map);
        playerCharacter.SnapCharacterToTileOnMap(newPos);
    }


    public void LoadNewMap(int map)
    {
        var loadedMap = gameMaps[map].LoadMap.Instantiate();
        gameplayMasterParent.AddChild(loadedMap);
    }
}
