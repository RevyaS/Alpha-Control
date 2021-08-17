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
		{
			moveGuide.QueueFree();
			moveGuide = null;
		}

		// Generate path from unit to mouse position
		Navigation2D navigator = Game.Map.GetNode<Navigation2D>("Nav");
		GC.Array<Unit> units = new GC.Array<Unit>(unitContainer.GetChildren());
		GC.Array<Vector2> prefPos = new GC.Array<Vector2>();
		if(formationOn)
		{
			//Preffered positions
			prefPos = generateFormationLine(targetPosition);
			float minAngle = Mathf.Abs(UtilityFunctions.angleRadianReference(formationLineAngle));
			bool isHorizontal = (Mathf.Deg2Rad(45) < minAngle && minAngle < Mathf.Deg2Rad(135)) ? false :
				((Mathf.Deg2Rad(225) < minAngle && minAngle < Mathf.Deg2Rad(315)) ? false : true);
			GD.Print(Mathf.Rad2Deg(minAngle));
			GD.Print((Mathf.Deg2Rad(45) < minAngle && minAngle < Mathf.Deg2Rad(135)));
			GD.Print("Horizontal? " + isHorizontal);
			heuristicLineFormation(ref units, ref prefPos, isHorizontal);
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
		formationLineAngle = Mathf.Pi/2 - centroid.AngleToPoint(targetPosition);
		//Create vector of distance from targetPosition to an endpoint
		Vector2 delta = new Vector2(Mathf.Cos(formationLineAngle), -Mathf.Sin(formationLineAngle)) * totalLength;
		Vector2 top = targetPosition + delta;
		Vector2 bot = targetPosition - delta;
		moveGuide = new Line2D();
		
		moveGuide.AddPoint(top);
		moveGuide.AddPoint(bot);

		AddChild(moveGuide);

		//1st Pos (outer distance)
		//Vector2 div = new Vector2(top.x - bot.x, top.y - bot.y)/UnitCount;
		Vector2 div = (top-bot)/UnitCount;
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
	private void heuristicLineFormation(ref GC.Array<Unit> units, ref GC.Array<Vector2> prefPositions, bool isHorizontal)
	{
		//Sort units by lesser x & y
		SortDelegates.SortDelegate unitSort = SortDelegates.UnitsPosYAscending, 
								   positionSort = SortDelegates.Vector2YAscending;
		if(isHorizontal)
		{
			unitSort = SortDelegates.UnitsPosXAscending;
			positionSort = SortDelegates.Vector2XAscending;
		}

		units = new GC.Array<Unit>(DataManager.MergeSort((GC.Array)units, unitSort));
		prefPositions = new GC.Array<Vector2>(DataManager.MergeSort((GC.Array)prefPositions, positionSort));
	}

	public bool OpenFire {
		set
		{
			openFire = value;
			foreach (Unit unit in unitContainer.GetChildren())
			{
				unit.openFire = value;
			}
		}
		get { return openFire; }
	}

	Line2D moveGuide;
	float formationLineAngle = 0;

	public bool formationOn = false;
	bool openFire = false;
}
