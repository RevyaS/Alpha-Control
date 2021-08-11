using Godot;
using System;

public class Game : Control
{
	Group controller;
	public static Map Map;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
				controller.Move(btn.GlobalPosition);
			}
		}
	}
	
}
