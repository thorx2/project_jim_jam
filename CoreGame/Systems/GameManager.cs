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

	[ExportCategory("Spread Factors")]
	[Export]
	private float greyCharacterFailSpreadValue;

	[Export]
	private float coloredCharacterFailedSpreadValue;

	private ObjectiveData activeObjective;

	public ObjectiveData GetActiveObjective
	{
		get => activeObjective;
	}

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
		if (GameRuntimeParameters.GameDay < 5)
		{
			GameRuntimeParameters.GossipSpread = 0;
			GameRuntimeParameters.ColorFailSpread = coloredCharacterFailedSpreadValue;
			GameRuntimeParameters.GreyFailSpread = greyCharacterFailSpreadValue;
			GameRuntimeParameters.MaxTolerableSpread = perDaySpreadTolerance[GameRuntimeParameters.GameDay];
			activeObjective = possibleGameObjectives[GameRuntimeParameters.GameDay];
		}
		GameRuntimeParameters.GameDay++;
	}

	#endregion
}
