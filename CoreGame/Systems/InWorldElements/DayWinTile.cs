using CoreGame.GameSystems.EventManagement;
using Godot;
using System;

public partial class DayWinTile : Area2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        BodyEntered += OnBodyEnterArea;
    }

    private void OnBodyEnterArea(Node2D body)
    {
        var p = body as Player;

        if (p != null)
        {
            MasterSignalBus.GetInstance.OnDayOver?.Invoke(true);
        }
    }

}
