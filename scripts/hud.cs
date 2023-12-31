using Godot;
using System;

public partial class hud : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();
	
	// Pending message
	public void ShowMessage(string text)
	{
		var message = GetNode<Label>("Message");
		message.Text = text;
		message.Show();
		
		GetNode<Timer>("MessageTimer").Start();
	}
	
	async public void ShowGameOver()
	{
		ShowMessage("Game Over");
		
		var messageTimer = GetNode<Timer>("MessageTimer");
		await ToSignal(messageTimer, Timer.SignalName.Timeout);
		
		
		var message = GetNode<Label>("Message");
		message.Text = "Dodge the Creeps!";
		message.Show();
		
		await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
		GetNode<Button>("StartButton").Show();
	}
	
	public void UpdateScore(int score)
	{
		GetNode<Label>("ScoreLabel").Text = score.ToString();
	}
	
	
	// Connections from other nodes
	private void _on_message_timer_timeout()
	{
		// Replace with function body.
		GetNode<Label>("Message").Hide();
	}


	private void _on_start_button_pressed()
	{
		// Replace with function body.
		GetNode<Button>("StartButton").Hide();
		EmitSignal(SignalName.StartGame);
	}
}

