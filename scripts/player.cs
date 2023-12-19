using Godot;
using System;

public partial class player : Area2D
{
	// Signal to detect collissions
	[Signal]
	public delegate void HitEventHandler();
	
	// How fast the player will move in pixels / second
	[Export]
	public int Speed { get; set; } = 400;
	
	// Size of the game window
	public Vector2 ScreenSize;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		// Player is hidden at beginning of the game
		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity  = Vector2.Zero;
		// Check for input
		if( Input.IsActionPressed("move_right") )
		{
			velocity.X += 1;
		}
		if( Input.IsActionPressed("move_left") )
		{
			velocity.X += -1;
		}
		if( Input.IsActionPressed("move_down") )
		{
			velocity.Y += 1;
		}
		if( Input.IsActionPressed("move_up") )
		{
			velocity.Y -= 1;
		}
		
		// Play the appropriate animation
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if( velocity.Length() > 0 )
		{
			velocity = velocity.Normalized() * Speed;
			animatedSprite2D.Play();
		}
		else
		{
			animatedSprite2D.Stop();
		}
		
		// Move in the direction
		Position += velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);
		
		// Choose the appropriate animation
		if( velocity.X != 0 )
		{
			animatedSprite2D.Animation = "walk";
			animatedSprite2D.FlipV = false;
			animatedSprite2D.FlipH = velocity.X < 0;
		}
		else if ( velocity.Y != 0 )
		{
			animatedSprite2D.Animation = "up";
			animatedSprite2D.FlipV = velocity.Y > 0;
		}
	}
	
	private void _on_body_entered(Node2D body)
	{
		// Player dissapears after being hit
		Hide();
		EmitSignal(SignalName.Hit);
		
		// Must be deferred as we can't change physics properties on a physics 
		// callback. Need to disable so this is not called twice
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(
			CollisionShape2D.PropertyName.Disabled, true );
	}

	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
}
