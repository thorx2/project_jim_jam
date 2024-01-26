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

	private Player playerRef = null;

	public Player GetPlayerRef
	{
		get => playerRef;
		set => playerRef = value;
	}

	public Player CreatePlayerCharacter()
	{

		if (playerRef == null)
		{
			playerRef = playerScene.Instantiate() as Player;
			gameplayParent.AddChild(playerRef);
		}

		return playerRef;
	}
	#endregion

	#region Godot region
	public override void _Ready()
	{
		instance = this;
		MasterSignalBus.GetInstance.StartGameEvent += OnStartNewGame;

		MasterSignalBus.GetInstance.OnDayOver += OnDayCompleted;
	}
	#endregion

	#region Functional
	public void OnStartNewGame()
	{
		if (playerRef == null)
		{
			CreatePlayerCharacter();
		}
		GameRuntimeParameters.ResetGameParameters();
		StartNextDay();
	}
	#endregion

	#region Day Management

	[ExportGroup("Objectives")]
	[Export]
	private ObjectiveData[] possibleGameObjectives;

	[Export]
	private float[] perDaySpreadTolerance;

	private ObjectiveData activeObjective;

	public ObjectiveData GetActiveObjective
	{
		get => activeObjective;
	}

	private Random rnd = new();

	public void OnDayCompleted(bool isWin)
	{
		if (isWin)
		{
			StartNextDay();
		}
		else
		{
			GameRuntimeParameters.GossipSpread = 0;
		}
	}

	public void StartNextDay()
	{
		GameRuntimeParameters.GossipSpread = 0;
		GameRuntimeParameters.MaxTolerableSpread = perDaySpreadTolerance[GameRuntimeParameters.GameDay];
		GameRuntimeParameters.GameDay++;
		activeObjective = possibleGameObjectives[rnd.Next(0, possibleGameObjectives.Length - 1)];
	}

	#endregion
}
