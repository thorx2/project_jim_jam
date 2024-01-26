using System;
using Godot;

public partial class RhythmTile : Area2D
{
    [Export]
    private float tileSpeed;

    [Export]
    private VisibleOnScreenNotifier2D onScreenNotifier;

    [Export]
    private AnimatedSprite2D visualSprite;

    public Action<bool, RhythmTile> TileInteractionHappened;

    private bool sensor;

    private int tileKey;

    private bool processDone;

    public override void _Ready()
    {
        sensor = false;
        onScreenNotifier.ScreenExited += OnScreenExit;

        AreaShapeEntered += OnAShapedEntered;
        AreaShapeExited += OnAShapedExited;
    }

    private void OnAShapedExited(Rid areaRid, Area2D area, long areaShapeIndex, long localShapeIndex)
    {
        sensor = false;
    }

    private void OnAShapedEntered(Rid areaRid, Area2D area, long areaShapeIndex, long localShapeIndex)
    {
        sensor = true;
    }

    public override void _Input(InputEvent @event)
    {
        if (sensor && @event is InputEventKey && @event.IsPressed())
        {
            var g = @event as InputEventKey;
            switch (g.Keycode)
            {
                case Key.W:
                    if (tileKey == 0)
                    {
                        if (!processDone)
                        {
                            processDone = true;
                            TileInteractionHappened?.Invoke(true, this);
                        }
                    }
                    break;
                case Key.A:
                    if (tileKey == 1)
                    {
                        if (!processDone)
                        {
                            TileInteractionHappened?.Invoke(true, this);
                            processDone = true;
                        }
                    }
                    break;
                case Key.S:
                    if (tileKey == 2)
                    {
                        if (!processDone)
                        {
                            TileInteractionHappened?.Invoke(true, this);
                            processDone = true;
                        }
                    }
                    break;
                case Key.D:
                    if (tileKey == 3)
                    {
                        if (!processDone)
                        {
                            TileInteractionHappened?.Invoke(true, this);
                            processDone = true;
                        }
                    }
                    break;
            }
        }
    }

    private void OnScreenExit()
    {
        if (!processDone)
        {
            TileInteractionHappened?.Invoke(false, this);
            processDone = true;
        }
    }

    public override void _Process(double delta)
    {
        if (Visible)
        {
            var currPos = GlobalPosition;
            currPos.Y += tileSpeed * (float)delta;

            GlobalPosition = currPos;
        }
    }

    public void InitTileMovement(Vector2 pos, int key)
    {
        processDone = false;
        GlobalPosition = pos;
        Visible = true;
        tileKey = key;
        visualSprite.Frame = key;
    }
}