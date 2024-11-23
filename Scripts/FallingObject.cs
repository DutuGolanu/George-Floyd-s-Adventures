using Godot;
using System;

public partial class FallingObject : StaticBody2D
{
    Main main;
    [Export] float speed = 10;
    public override void _Ready()
    {
        main = GetTree().Root.GetNode<Main>("Main");
    }
    public override void _PhysicsProcess(double delta)
    {
        if(Main.gameRunning == true){
            Vector2 position = Position;
            position.Y += speed;
            if(position.Y>=1300)
            {
                Delete();
                if(!IsInGroup("bad")) main.GameOver();
            }
            Position = position;    
        }
    }
    public void Delete(){
        QueueFree();
        Main.objectsNumber--;
    }
}
