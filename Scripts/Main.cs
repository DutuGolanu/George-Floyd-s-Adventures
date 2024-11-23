using Godot;
using System;
using System.Collections;
using System.Net.Sockets;

public partial class Main : Control
{
    [Export] Panel gameOverPanel;
    [Export] Panel panel;
    [Export] Timer spawnTimer;
    [Export] Label scoreLabel,highScoreLabel;
    AudioStreamPlayer2D coinPickupSound;
    [Export] Button retry,exit;
    public static bool gameRunning = false;
    bool ReadyToSpawn = true;
    public static int objectsNumber = 0;
    int objectsLimit = 1,score = 0,i = 20;
    public static int highScore = 0;
    string[] paths = {
        "res://Scenes/friedChicken.tscn",
        "res://Scenes/watermelon.tscn",
        "res://Scenes/police_officer.tscn"
    };
    public override void _Ready()
    {
        gameRunning = false;
        objectsNumber = 0;
        objectsLimit = 1;
        GameManager.Load();
        panel.Show();
        coinPickupSound = GetNode<AudioStreamPlayer2D>("CoinSound");
        gameOverPanel.Hide();
        spawnTimer.Stop();
    }
    public override void _Process(double delta)
    {
        if(gameRunning && objectsNumber<objectsLimit && ReadyToSpawn){
            SpawnObject();
        }
    }
    void SpawnObject(){
        objectsNumber++;
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();
        int randomIndex = rng.RandiRange(0,2);
        PackedScene fallingObject = (PackedScene)ResourceLoader.Load(paths[randomIndex]);
        StaticBody2D fallingObjectInstantiated = (StaticBody2D)fallingObject.Instantiate();
        AddChild(fallingObjectInstantiated);
        float objectWidth = fallingObjectInstantiated.GetNode<Sprite2D>("Sprite2D").Texture.GetWidth() * fallingObjectInstantiated.Scale.X;
        fallingObjectInstantiated.Position = new Vector2(rng.RandiRange((int)(objectWidth/2),(int)(1920-objectWidth/2)),-200);
        if(randomIndex == 2){
            fallingObjectInstantiated.AddToGroup("bad");
        }
        ReadyToSpawn = false;
        spawnTimer.Start();
    }
    public void BodyEntered(FallingObject body){
        body.Delete();
        if(body.IsInGroup("bad")){
            GameOver();
        }
        else{
            score++;
            if(highScore < score) highScore = score;
            if(score == 10) objectsLimit = 2;
            if(score == i) {
                spawnTimer.WaitTime = spawnTimer.WaitTime-0.10;
                i += 15; 
            }
            scoreLabel.Text = score.ToString();
            coinPickupSound.Play();
        }
    }
    public void GameOver(){
        gameRunning = false;
        gameOverPanel.Show();
        highScoreLabel.Text = "HighScore: " +  highScore;
        GameManager.Save();
    }
    void OnExitPressed(){
        GetTree().Quit();
    }
    void OnRetryPressed(){
        GetTree().ReloadCurrentScene();
    }
    void Timeout(){
        ReadyToSpawn = true;
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton && @event.IsPressed() && !gameRunning && !gameOverPanel.IsVisibleInTree()) {
            gameRunning = true;
            panel.Hide();
        }
        base._Input(@event);
    }
}

