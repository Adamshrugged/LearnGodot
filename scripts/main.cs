using Godot;
using System;

public partial class main : Node
{
	[Export]
	public PackedScene MobScene { get; set; }
	private int _score;
		
	public void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		
		GetNode<hud>("HUD").ShowGameOver();
		GD.Print("test");
	}
	
	public void NewGame()
	{
		_score = 0;
		var player = GetNode<player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);
		
		GetNode<Timer>("StartTimer").Start();
		
		// Update HUD
		var hud = GetNode<hud>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");
	}

	private void _on_mob_timer_timeout()
	{
		// Create a mob instance
		Mob mob = MobScene.Instantiate<Mob>();
		
		// Choose a random location along the edge of the screen
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();
		
		// Set the mob's direction perpendicular to the path direction
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;
		
		// Set the mob's position to a random location
		mob.Position = mobSpawnLocation.Position;
		
		// Add some randomness to the direction
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = direction;
		
		// Choose the velocity
		var velocity = new Vector2( (float)GD.RandRange(150.0, 250.0), 0 );
		mob.LinearVelocity = velocity.Rotated(direction);
		
		// Spawn in the mob (Add to the main scene)
		AddChild(mob);
	}

	private void _on_score_timer_timeout()
	{
		// Replace with function body.
		_score++;
		GetNode<hud>("HUD").UpdateScore(_score);
	}

	private void _on_start_timer_timeout()
	{
		// Replace with function body.
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

}
