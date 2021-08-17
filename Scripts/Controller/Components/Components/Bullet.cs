using Godot;
using System;

public class Bullet : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}


	public override void _Process(float delta)
	{
		float distance = velocity.Length() * delta;
		Vector2 travelled = Vector2.Zero;
		if(range > distance)
		{
			travelled = velocity * delta;
		} else
			travelled = velocity * (range /velocity.Length());

		Position += travelled;
		range -= travelled.Length();
		if(range <= 0)
			QueueFree();
	}


	///<summary>
	///Initialize required parameters, failure to do so will throw an exception
	///</summary>     
	///<param name="range">Maximum distance the bullet can travel</param>
	///<param name="velocity">Speed of bullet per second in Vector form</param>
	public void Init(float range, Vector2 velocity)
	{
		this.range = range;
		this.velocity = velocity;
	}

	private float range;
	private Vector2 velocity;
}
