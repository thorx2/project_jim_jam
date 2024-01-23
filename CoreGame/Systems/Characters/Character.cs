using Godot;
using System;

public partial class Character : CharacterBody2D
{
    [Export]
    public float Speed = 300.0f;

    [Export]
    private AnimatedSprite2D characterVisual;

    [Export]
    private int tileSize;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Y = direction.Y * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}
