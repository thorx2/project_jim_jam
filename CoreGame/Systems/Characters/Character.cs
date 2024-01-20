using Godot;
using System;

public partial class Character : CharacterBody2D
{
    [Export]
    public float Speed = 300.0f;

    [Export]
    private AnimatedSprite2D characterVisual;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        
        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
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
        var currentPos = Position;
        currentPos.X = Math.Clamp(currentPos.X, 0, GetViewportRect().Size.X);
        currentPos.Y = Math.Clamp(currentPos.Y, 0, GetViewportRect().Size.Y);
        Position = currentPos;
    }
}
