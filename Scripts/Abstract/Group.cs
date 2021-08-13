using Godot;
using System;
using GC = Godot.Collections;

public abstract class Group : Node2D
{
    public override void _Ready()
    {
		centroidTexture = GetNode<TextureRect>("Centroid");
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
    

    //Recalculates the centroid with the given children
	protected void calculateCentroid()
	{
        if(unitContainer == null) throw new Exception("unitContainer is null, please provide value");

		totalPos = Vector2.Zero;
		foreach(Unit child in unitContainer.GetChildren())
		{
			if(child.Radius > largestRadius) largestRadius = child.Radius;
			totalPos += child.Position;
		}
		centroid = totalPos/unitContainer.GetChildCount();
	}


    public abstract void Move(Vector2 targetPosition);


    //Properties
	public Vector2 Centroid {
		get {return centroid;}
	}

    //Properties
	public int UnitCount {
		get {return unitContainer.GetChildCount();}
	}

    protected float largestRadius;
    //Components
    protected Node2D unitContainer;
    protected TextureRect centroidTexture;
 
    protected Vector2 centroid;
    //For centroid calculation
	Vector2 totalPos;

	//Debugging
	protected bool drawCentroid = false;
	
}