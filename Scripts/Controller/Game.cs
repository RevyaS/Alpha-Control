using Godot;
using System;

public class Game : Control
{
	Node2D controller;
	TextureRect map;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		controller = GetNode<Node2D>("Control");
		map = GetNode<TextureRect>("Map");
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
				foreach(Unit unit in controller.GetChildren())
				{
					// Generate path from unit to mouse position
					Navigation2D navigator = map.GetNode<Navigation2D>("Nav");
					Vector2[] path = navigator.GetSimplePath(unit.Position, btn.GlobalPosition);
					//GD.Print(path == null);
					unit.Move(path);
				}

			}
		}
	}
	
}
