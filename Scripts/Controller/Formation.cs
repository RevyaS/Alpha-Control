using Godot;
using System;
using GC = Godot.Collections;

public class Formation : Group
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		//Components
		unitContainer = GetNode<Node2D>("Units");
		
		calculateCentroid();
		drawCentroid = true;
	}
	

	public override void _Process(float delta)
	{
		base._Process(delta);
	}

	
	//Moves the Group
	public override void Move(Vector2 targetPosition)
	{
		if(moveGuide != null)
			moveGuide.QueueFree();

		// Generate path from unit to mouse position
		Navigation2D navigator = Game.Map.GetNode<Navigation2D>("Nav");
		GC.Array<Unit> units = new GC.Array<Unit>(unitContainer.GetChildren());
		GC.Array<Vector2> prefPos = new GC.Array<Vector2>();
		if(formationOn)
		{
			//Preffered positions
			prefPos = generateFormationLine(targetPosition);
			
			heuristicLineFormation(ref units, ref prefPos);
		}

		for(int i = 0; i < unitContainer.GetChildCount(); i++)
		{
			Unit unit = units[i];
			Vector2 targetPos = (formationOn) ? prefPos[i] : targetPosition;
			Vector2[] path = navigator.GetSimplePath(unit.Position, targetPos);
			unit.Move(path);
		}
	}


	//Generates a line at a Point, returns points for every unit
	private GC.Array<Vector2> generateFormationLine(Vector2 targetPosition)
	{
		//Formula is largestRadius * 2 * totalUnits, but the 2 will be 
		//cancelled out when divided into 2 to get the top and bottom position
		float totalLength = largestRadius * UnitCount;
		//Get reference angle from centroid to targetPosition
		float distance = centroid.DistanceTo(targetPosition);
		float angle = Mathf.Pi/2 - centroid.AngleToPoint(targetPosition);
		//Create vector of distance from targetPosition to an endpoint
		Vector2 delta = new Vector2(Mathf.Cos(angle), -Mathf.Sin(angle)) * totalLength;

		Vector2 top = targetPosition + delta;
		Vector2 bot = targetPosition - delta;
		moveGuide = new Line2D();
		
		moveGuide.AddPoint(top);
		moveGuide.AddPoint(bot);

		AddChild(moveGuide);

		//1st Pos (outer distance)
		Vector2 div = new Vector2(top.x - bot.x, top.y - bot.y)/UnitCount;
		GC.Array<Vector2> unitPos = new GC.Array<Vector2>();
		if(GetChildCount() == 1) {
			unitPos.Add(targetPosition);
			return unitPos;
		}
		bot += div/2;
		//Add 1st instance
		unitPos.Add(bot);
		for(int i = 1; i < UnitCount; i++)
		{
			bot += div;
			unitPos.Add(bot);
		}
		return unitPos;
	}


	//Sorts the array of units or positions to match each other
	private void heuristicLineFormation(ref GC.Array<Unit> units, ref GC.Array<Vector2> prefPositions)
	{
		//Sort units by lesser x & y
		units = new GC.Array<Unit>(DataManager.MergeSort((GC.Array)units, SortDelegates.UnitsPosAscending));
		prefPositions = new GC.Array<Vector2>(DataManager.MergeSort((GC.Array)prefPositions, SortDelegates.Vector2Ascending));
	}

	Line2D moveGuide;
	public bool formationOn = false;
}
