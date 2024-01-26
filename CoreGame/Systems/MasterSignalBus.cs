
using System;
using Godot;

namespace CoreGame.GameSystems.EventManagement;

/// <summary>
/// Singleton global class to be used as master signal bus for cross systems notification
/// </summary>
public class MasterSignalBus
{
    private static MasterSignalBus instance;

    public static MasterSignalBus GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = new();
            }

            return instance;
        }
    }

    private MasterSignalBus() { }

    /// <summary>
    /// Event fired from the Level onces it is loaded to place the player character in the map.
    /// </summary>
    public Action<Vector2, TileMap> LevelLoadedEvent;

    /// <summary>
    /// Event fired to load a particular level index.
    /// </summary>
    public Action<int> LoadMapEvent;

    /// <summary>
    /// Trigger to reset and start a new game, systems and configurations will reset and begin from day 1
    /// </summary>
    public Action StartGameEvent;


    public Action<ECharacterType, NPC> StartQteEvent;

    public Action<ECharacterType, EQteCompleteState> OnQteCompleteEvent;

    public Action<bool> OnDayOver;

    public Action GameHardReset;

    public static void HardResetSystem()
    {
        instance.LevelLoadedEvent = null;
        instance.LoadMapEvent = null;
        instance.StartGameEvent = null;
        instance.StartQteEvent = null;
        instance.OnQteCompleteEvent = null;
        instance.OnDayOver = null;
        instance.GameHardReset?.Invoke();
    }
}