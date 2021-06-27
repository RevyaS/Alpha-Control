using Godot;
using System;
using GC = Godot.Collections;

public class Unit : Sprite
{
	float speed = 300;
	// Pathway when issued to Move
	GC.Array<Vector2> vectorPath;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		vectorPath = new GC.Array<Vector2>();
	}


	// Issue movement
	public void Move(Vector2[] path)
	{
		vectorPath = new GC.Array<Vector2>(path);

	}
	
	public override void _Process(float delta)
	{
		// Sync speed to frame
		float speed = this.speed * delta;

		//Move while vectorPath is not empty
		while(speed > 0 && vectorPath.Count > 0)
		{
			//Update position if destination not reached
			float distance = Position.DistanceTo(vectorPath[0]);
			
			if(speed <= distance) 
				Position += Position.DirectionTo(vectorPath[0]) * speed;
			else {
				Position = vectorPath[0];
				vectorPath.RemoveAt(0);
			}
			speed -= distance;
		}
	}

}
