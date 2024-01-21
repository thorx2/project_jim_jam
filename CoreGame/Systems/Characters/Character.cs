using Godot;
using System;

public partial class Character : CharacterBody2D
{
    [Export]
    public float Speed = 300.0f;

    [Export]
    private AnimatedSprite2D characterVisual;

    private Vector2 spriteSize;

    public override void _Ready()
    {
        spriteSize = characterVisual.SpriteFrames.GetFrameTexture(characterVisual.Animation, 0).GetSize();
    }

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
        currentPos.X = Math.Clamp(currentPos.X, spriteSize.X / 2, GetViewportRect().Size.X - spriteSize.X / 2);
        currentPos.Y = Math.Clamp(currentPos.Y, spriteSize.Y / 2, GetViewportRect().Size.Y - spriteSize.Y / 2);
        Position = currentPos;
    }
}
