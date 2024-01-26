
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

    public Action StartGame;
}