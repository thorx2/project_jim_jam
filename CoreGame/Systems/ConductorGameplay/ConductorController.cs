using System;
using System.Collections.Generic;
using CoreGame.GameSystems.EventManagement;
using Godot;

public partial class ConductorController : Control
{
    [ExportCategory("Reference")]
    [Export]
    private Marker2D[] dropSpawnLocation;

    [Export]
    private PackedScene tileTemplate;

    [Export]
    private Timer tileSpawnTimer;

    private List<RhythmTile> tilePool;
    private List<RhythmTile> activeTiles;

    Random rnd = new();

    private int maxLives = 3;

    private int currentLives;

    private float currentRoundDuration;
    private ECharacterType lastCharacterType;
    private float gameDuration;

    private bool isRunningGame;

    public override void _Ready()
    {
        tileSpawnTimer.Stop();
        tileSpawnTimer.OneShot = false;
        tileSpawnTimer.Timeout += SpawnRandomTile;
        tilePool = new();
        activeTiles = new();
        for (int i = 0; i < 10; i++)
        {
            var tile = tileTemplate.Instantiate() as RhythmTile;
            tilePool.Add(tile);
            tile.Visible = false;

            tile.TileInteractionHappened += ProcessTileInteraction;
            AddChild(tile);
        }
        SetProcess(false);
        SetProcessInput(false);
    }

    private void SpawnRandomTile()
    {
        int key = rnd.Next(0, 3);
        if (tilePool.Count > 0)
        {
            var tile = tilePool[0];
            tilePool.RemoveAt(0);
            activeTiles.Add(tile);
            tile.InitTileMovement(dropSpawnLocation[key].GlobalPosition, key);
        }
        else
        {
            var tile = tileTemplate.Instantiate() as RhythmTile;
            tile.TileInteractionHappened += ProcessTileInteraction;
            AddChild(tile);
            tile.InitTileMovement(dropSpawnLocation[key].GlobalPosition, key);
            activeTiles.Add(tile);
        }
    }

    public override void _Process(double delta)
    {
        if (isRunningGame)
        {
            currentRoundDuration += (float)delta;
            if (currentRoundDuration > gameDuration)
            {
                MasterSignalBus.GetInstance.OnQteCompleteEvent(lastCharacterType, EQteCompleteState.EQteSuccess);
                MiniGameEndCleanup();
            }
        }
    }

    private void ProcessTileInteraction(bool arg1, RhythmTile tile)
    {
        if (!arg1)
        {
            currentLives -= 1;
        }
        tile.Visible = false;
        tilePool.Add(tile);

        if (currentLives <= 0)
        {
            MasterSignalBus.GetInstance.OnQteCompleteEvent(lastCharacterType, EQteCompleteState.EQteFailed);
            MiniGameEndCleanup();
        }
    }

    private void MiniGameEndCleanup()
    {
        isRunningGame = false;
        tileSpawnTimer.Stop();
        SetProcess(false);
        SetProcessInput(false);
    }

    public void Show(ECharacterType type, float duration)
    {
        lastCharacterType = type;
        gameDuration = duration;
        tileSpawnTimer.Start();
        Visible = true;
        currentLives = maxLives;
        isRunningGame = true;
        currentRoundDuration = 0;
    }
}