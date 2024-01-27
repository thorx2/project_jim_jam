using System;
using System.Collections.Generic;
using System.Linq;
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

    private NPC activeNPC;

    [Export]
    private HBoxContainer keyTileContainer;
    private Node keyTileTempParent;

    private List<Node> keyTileNodes = new List<Node>();

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

        keyTileNodes = keyTileContainer.GetChildren().ToList();
        keyTileTempParent = keyTileContainer.GetParent();
    }

    private void SpawnRandomTile()
    {
        int key = rnd.Next(0, 4);   // Max Value is excluded that's why 'D' was never being picked

        if (activeNPC.GetSpecialNPCStyle != ESpecialNPC.EBully)
        {
            // Just spawn one tile for non bullies
            SpawnTileAtKey(key, GetAnimationForActiveNPC(), true);
            return;
        }
        else
        {
            // Spawn at every tile but the key tile is invisible
            for (int i = 0; i < 4; i++)
            {
                string animation = (i == key) ? "Invisible" : GetAnimationForActiveNPC();
                SpawnTileAtKey(i, animation, i == key);
            }
        }
    }

    private void SpawnTileAtKey(int key, string animation, bool canReduceLives)
    {
        if (tilePool.Count > 0)
        {
            var tile = tilePool[0];
            tilePool.RemoveAt(0);
            activeTiles.Add(tile);
            tile.InitTileMovement(dropSpawnLocation[key].GlobalPosition, key, animation, canReduceLives);
        }
        else
        {
            var tile = tileTemplate.Instantiate() as RhythmTile;
            tile.TileInteractionHappened += ProcessTileInteraction;
            AddChild(tile);
            tile.InitTileMovement(dropSpawnLocation[key].GlobalPosition, key, animation, canReduceLives);
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
        if (!arg1 && tile.canReduceLives)
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

    public void Show(ECharacterType type, float duration, NPC npc)
    {
        lastCharacterType = type;
        gameDuration = duration;
        tileSpawnTimer.Start();
        Visible = true;
        currentLives = maxLives;
        isRunningGame = true;
        currentRoundDuration = 0;
        activeNPC = npc;

        if (activeNPC.GetSpecialNPCStyle == ESpecialNPC.EQuiet)
        {
            // Reparent the keys after shuffling for the quiet kid
            Node[] shuffledKeys = ShuffledList(keyTileNodes);

            for (int i = 0; i < shuffledKeys.Length; i++)
            {
                shuffledKeys[i].Reparent(keyTileTempParent, true);
            }

            for (int i = 0; i < shuffledKeys.Length; i++)
            {
                shuffledKeys[i].Reparent(keyTileContainer, true);
            }
        }
        else
        {
            // Reparent the keys normally for the others
            for (int i = 0; i < keyTileNodes.Count; i++)
            {
                keyTileNodes[i].Reparent(keyTileTempParent, true);
            }

            for (int i = 0; i < keyTileNodes.Count; i++)
            {
                keyTileNodes[i].Reparent(keyTileContainer, true);
            }
        }
    }

    // GC happens but who cares!
    private Node[] ShuffledList(List<Node> nodes)
    {
        Node[] arr = new Node[nodes.Count];
        nodes.CopyTo(arr);

        for (int i = 0; i < arr.Length; i++)
        {
            int swapIndex = rnd.Next(0, arr.Length);
            Node temp = arr[i];
            arr[i] = arr[swapIndex];
            arr[swapIndex] = temp;
        }

        return arr;
    }

    private string GetAnimationForActiveNPC()
    {
        switch (activeNPC.GetSpecialNPCStyle)
        {
            case ESpecialNPC.ESocial:    return "Influenza";
            case ESpecialNPC.ETwins:     return "Twins";
            case ESpecialNPC.EBully:     return "Bully";
            case ESpecialNPC.EQuiet:     return "Quiet";
            case ESpecialNPC.EProfessor: return "Professor";
        }

        return "default";
    }
}