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

	private Random rnd = new();

	private ESpecialNPC npcOfTheDay;

	public ESpecialNPC GetSpecialNPCOfDay
	{
		get => npcOfTheDay;
	}

	public void OnDayCompleted(bool isWin)
	{
		if (isWin)
		{
			StartNextDay();
		}
	}

	public void StartNextDay()
	{
		GameRuntimeParameters.GameDay++;
		npcOfTheDay = (ESpecialNPC)rnd.Next(0, 4);
	}

	#endregion
}
