using CoreGame.GameSystems.EventManagement;
using Godot;
using Godot.Collections;

namespace CoreGame.GameSystems;

public partial class GameLevelManager : SubViewport
{
    [Export]
    public Array<LevelData> gameMaps;

    [Export]
    private Character playerCharacter;

    [Export]
    private Node2D gameplayMasterParent;

    public override void _Ready()
    {
        MasterSignalBus.GetInstance.LevelLoadedEvent += OnNewLevelLoaded;
        MasterSignalBus.GetInstance.LoadMapEvent += LoadNewMap;

        if (gameMaps.Count > 0)
        {
            var map = gameMaps[0].LoadMap.Instantiate();
            gameplayMasterParent.AddChild(map);
        }
        else
        {
            GD.PrintErr("Missing levels for the game!!");
        }
    }

    private void OnNewLevelLoaded(Vector2 newPos)
    {
        playerCharacter.GlobalPosition = newPos;
    }


    public void LoadNewMap(PackedScene map)
    {
        
    }
}
