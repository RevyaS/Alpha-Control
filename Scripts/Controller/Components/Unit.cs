using Godot;
using System;
using GC = Godot.Collections;

public class Unit : KinematicBody2D
{
	//Values
	const float speed = 300;

	// Pathway when issued to Move
	GC.Array<Vector2> vectorPath;
	Line2D pathGuide;
	int pointsReached;
	// Components
	Sprite sprite;
	// Targets
	Unit target;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = GetNode<Sprite>("Unit");
		vectorPath = new GC.Array<Vector2>();pathGuide = GetNode<Line2D>("PathGuide");
		SetProcess(false);
	}
	
	
	//Game loop in a different process
	public override void _Process(float delta)
	{
		// Sync speed for this frame
		float distance = speed * delta;

		moveToPath(distance);

		if(target != null)
			aim(target);
	}


	// Unit detection
	private void presenceDetected(object body)
	{
		GC.Array enemies = GetTree().GetNodesInGroup("Enemies");
		
		if(!enemies.Contains(body)) return;

		GD.Print("Opp Detected");
		if(target == null)
			target = body as Unit;
	}
	
	
	// Unit exits
	private void presenceExited(object body)
	{
		if(body as Unit != target) return;
		
		target = null;
		sprite.Rotation = 0;
	}


	// Aims at target duh
	private void aim(Unit target)
	{
		// Get euclidian distance
		float deltaX = Position.x - target.Position.x;
		float deltaY = Position.y - target.Position.y;
		float angle = Mathf.Atan2(deltaY, deltaX) - (Mathf.Pi/2);
		GD.Print(Mathf.Rad2Deg(angle));
		sprite.Rotation = angle;
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
