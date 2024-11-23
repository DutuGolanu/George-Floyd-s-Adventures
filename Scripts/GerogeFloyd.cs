using Godot;
using System;
using System.Drawing;

public partial class GerogeFloyd : CharacterBody2D
{
    [Export]float speed = 1000;
    [Export]float accelerationDeceleration;
    Sprite2D sprite;
    float direction;
    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite2D");
    }
    public override void _PhysicsProcess(double delta)
    {
        Vector2 position = Position;
        if(Main.gameRunning == true){
            Vector2 velocity = Velocity;
            position.X = Mathf.Clamp(position.X,sprite.Texture.GetWidth()/2,1920-sprite.Texture.GetWidth()/2);
            velocity = Velocity.MoveToward(new Vector2(direction,0),accelerationDeceleration);
            if (Input.IsActionPressed("left"))
            {
                direction = -speed;
                accelerationDeceleration = 110;
            }
            else if (Input.IsActionPressed("right"))
            {
                direction = speed;
                accelerationDeceleration = 110;
            }
            else
            {
                direction = 0;
                accelerationDeceleration = 150;
            }
            Velocity = velocity;
            Position = position;
            MoveAndSlide();
        }
    }
}
