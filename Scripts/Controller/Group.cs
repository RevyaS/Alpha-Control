using Godot;
using System;
using GC = Godot.Collections;

public class Group : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//FOR DEBUGGING
		centroidTexture = GetNode<TextureRect>("Centroid");

		//Components
		unitContainer = GetNode<Node2D>("Units");
		
		calculateCentroid();
		drawCentroid = true;
	}
	

	public override void _Process(float delta)
	{
		if(drawCentroid)
		{
			if(!centroidTexture.Visible) centroidTexture.Show();

			calculateCentroid();
			centroidTexture.Show();
			centroidTexture.SetPosition(centroid - centroidTexture.RectSize/2);
			//GD.Print(centroid);
		}
	}

	
	//Moves the Group
	public void Move(Vector2 targetPosition)
	{
		//Preffered positions
		GC.Array<Vector2> prefPos = generateFormationLine(targetPosition);
		// Generate path from unit to mouse position
		Navigation2D navigator = Game.Map.GetNode<Navigation2D>("Nav");
		for(int i = 0; i < unitContainer.GetChildCount(); i++)
		{
			Unit unit = unitContainer.GetChild(i) as Unit;
			Vector2[] path = navigator.GetSimplePath(unit.Position, prefPos[i]);
			unit.Move(path);
		}
	}


	//Generates a line at a Point, returns points for every unit
	private GC.Array<Vector2> generateFormationLine(Vector2 targetPosition)
	{
		if(moveGuide != null)
			moveGuide.QueueFree();

		//Formula is largestRadius * 2 * totalUnits, but the 2 will be 
		//cancelled out when divided into 2 to get the top and bottom position
		float totalLength = largestRadius * UnitCount;
		//Get reference angle from centroid to targetPosition
		float distance = centroid.DistanceTo(targetPosition);
		float angle = Mathf.Pi/2 - centroid.AngleToPoint(targetPosition);
		//Create vector of distance from targetPosition to an endpoint
		Vector2 delta = new Vector2(Mathf.Cos(angle), -Mathf.Sin(angle)) * totalLength;
//		GD.Print(Mathf.Rad2Deg(angle) + " : " + delta);
		//Vector2 top = new Vector2(0, totalLength/2);
		//top += targetPosition;
		Vector2 top = targetPosition + delta;
//		Vector2 bot = new Vector2(targetPosition.x, targetPosition.y - totalLength);
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

	//Recalculates the centroid with the given children
	private void calculateCentroid()
	{
		totalPos = Vector2.Zero;
		foreach(Unit child in unitContainer.GetChildren())
		{
			if(child.Radius > largestRadius) largestRadius = child.Radius;
			totalPos += child.Position;
		}
		centroid = totalPos/unitContainer.GetChildCount();
	}

	//Properties
	public Vector2 Centroid {
		get {return centroid;}
	}

	public int UnitCount {
		get {return unitContainer.GetChildCount();}
	}

	Vector2 centroid;
	float largestRadius;
	//Components
	TextureRect centroidTexture;
	Node2D unitContainer;
	Line2D moveGuide;
	//For centroid calculation
	Vector2 totalPos;

	//Debugging
	bool drawCentroid = false;
	
}
