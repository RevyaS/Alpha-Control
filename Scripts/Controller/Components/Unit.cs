using Godot;
using System;
using GC = Godot.Collections;

public class Unit : KinematicBody2D
{
	const float speed = 300;
	Vector2 velocity = Vector2.Zero;

	// Pathway when issued to Move
	GC.Array<Vector2> vectorPath;
	Line2D pathGuide;
	
	int pointsReached;
	GC.Array<Vector2> guidePoints;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		vectorPath = new GC.Array<Vector2>();pathGuide = GetNode<Line2D>("PathGuide");
		SetProcess(false);
	}


	// Issue movement
	public void Move(Vector2[] path)
	{
		vectorPath = new GC.Array<Vector2>(path);
		//GD.Print(vectorPath.Count);
		SetProcess(true);
		for(int i = 0; i < path.Length; i++)
		{
			path[i] -= Position;
		}
		pathGuide.Points = path;
		pointsReached = 0;
	}
	
	public override void _Process(float delta)
	{
		// Sync speed for this frame
		float distance = speed * delta;

		moveToPath(distance);
	}


	//Amount of distance to be travelled this frame
	private void moveToPath(float distance)
	{
		if(pathGuide.Points.Length <= 1) return;
		Vector2 lastPosition = Position;
		
		while(distance > 0 && vectorPath.Count > 0)
		{
			//GD.Print(vectorPath.Count);
			float pointDistance = lastPosition.DistanceTo(vectorPath[0]);
			if(distance <= pointDistance)
			{
				MoveAndCollide( Position.DirectionTo(vectorPath[0]) * distance , false);
				//Position = lastPosition.LinearInterpolate(vectorPath[0], distance/pointDistance);
				break;
			}
			else if(distance < 0) {
				Position = vectorPath[0];
				SetProcess(false);
				break;
			}
			
			if(pointsReached > 0)
			{
				pathGuide.SetPointPosition(1, Vector2.Zero);
				pathGuide.RemovePoint(0);
			}

 			distance -= pointDistance;
 			lastPosition = vectorPath[0];
 			vectorPath.RemoveAt(0);
 			pointsReached++;
		}
		
 		for(int j = pathGuide.Points.Length - 1; j > 0; j--)
 		{
 			pathGuide.SetPointPosition(j, vectorPath[j-1] - Position );	
 		}
	}
}
