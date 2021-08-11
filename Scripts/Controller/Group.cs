using Godot;
using System;

public class Group : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	
	
	//Moves the Group
	public void Move(Vector2 targetPosition)
	{
		foreach(Unit unit in GetChildren())
		{
			// Generate path from unit to mouse position
			Navigation2D navigator = Game.Map.GetNode<Navigation2D>("Nav");
			Vector2[] path = navigator.GetSimplePath(unit.Position, targetPosition);
			unit.Move(path);
		}
	}
}
