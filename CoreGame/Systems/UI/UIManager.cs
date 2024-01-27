using CoreGame.GameSystems.EventManagement;
using Godot;
using System;

public partial class UIManager : Control
{
	[Export]
	private Label dayLabel;

	[Export]
	private Label questLabel;

	[Export]
	private Control mainMenu;

	[Export]
	private Control gameplayUIRef;

	[Export]
	private GameOverPanel gameOverPanel;

	[Export]
	private DayIntroController dayController;

	private int lastShownDay = -1;

	public override void _Ready()
	{
		MasterSignalBus.GetInstance.StartGameEvent += OnGameStart;

		MasterSignalBus.GetInstance.OnDayOver += OnDayOver;

		MasterSignalBus.GetInstance.LoadMapEvent += OnLoadMap;

		MasterSignalBus.GetInstance.StartGameEvent += StartGameEvent;

		mainMenu.Visible = true;
		gameplayUIRef.Visible = false;
		gameOverPanel.Visible = false;
		dayController.Visible = false;
		questLabel.Text = "";
	}

	private void StartGameEvent()
	{
		dayController.Visible = true;
		dayController.ShowDayTimer();
	}


	private void OnLoadMap(int obj)
	{
		if (GameRuntimeParameters.GameDay < 5)
		{
			dayController.Visible = true;
			dayController.ShowDayTimer();
		}
	}

	public override void _Process(double delta)
	{
		if (lastShownDay != GameRuntimeParameters.GameDay)
		{
			dayLabel.Text = $"Day {GameRuntimeParameters.GameDay}";
			lastShownDay = GameRuntimeParameters.GameDay;
		}
	}


	private void OnDayOver(bool obj)
	{
		gameOverPanel.Visible = true;
		gameOverPanel.ShowPanel(obj);
	}


	private void OnGameStart()
	{
		mainMenu.Visible = false;
		gameplayUIRef.Visible = true;
	}

}
