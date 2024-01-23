
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

    public Action<Vector2> LevelLoadedEvent;

    public Action<PackedScene> LoadMapEvent;
}