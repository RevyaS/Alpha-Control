using Godot;
using System;

public class Game : Control
{
	Group controller;
	bool acceptInput = true;
	public static Map Map;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		timer.Connect("timeout", this, nameof(resetInputFlag));
		
		controller = GetNode<Group>("Control");
		Map = GetNode<Map>("Map");
	}

	
	// Input callback
	public override void _Input(InputEvent @event)
	{
		InputEvent ev = @event;
		if(ev is InputEventMouseButton)
		{
			InputEventMouseButton btn = ev as InputEventMouseButton;
			// Check for Right Click then generate path
			if(btn.ButtonIndex == 2) 
			{
				//Only allow 1 input for every separate time
				if(!acceptInput) return;
				
				acceptInput = false;
				timer.Start();

				GD.Print("Move Issued");
				controller.Move(btn.GlobalPosition);
			}
		}
	}


	
	//Resets input flag
	private void resetInputFlag()
	{
		acceptInput = true;
	}

	Timer timer;
}
