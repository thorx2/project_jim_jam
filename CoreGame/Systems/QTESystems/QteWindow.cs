using System;
using System.Runtime.InteropServices;
using CoreGame.GameSystems.EventManagement;
using Godot;

public partial class QteWindow : Control
{
    [ExportGroup("Game Windows")]
    [Export]
    private GreyManGame greyGame;
    [ExportGroup("Game Windows")]
    [Export]
    private ConductorController conductorGame;

    [Export]
    private Control bg;

    public override void _Ready()
    {
        greyGame.Visible = false;
        conductorGame.Visible = false;
        bg.Visible = false;
    }

    public void HideMiniGameWindows()
    {
        greyGame.Visible = false;
        conductorGame.Visible = false;
        bg.Visible = false;
    }

    public void Show(ECharacterType type, float duration)
    {
        Visible = true;
        switch (type)
        {
            case ECharacterType.EGrey:
                bg.Visible = true;
                greyGame.Show(type, duration);
                greyGame.SetProcessInput(true);
                greyGame.SetProcess(true);
                break;
            case ECharacterType.EColored:
                bg.Visible = true;
                conductorGame.Show(type, duration);
                conductorGame.SetProcessInput(true);
                conductorGame.SetProcess(true);
                break;
        }
    }
}