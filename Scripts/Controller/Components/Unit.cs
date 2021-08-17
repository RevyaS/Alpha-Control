using Godot;
using System;
using GC = Godot.Collections;
using UF = UtilityFunctions;

public class Unit : KinematicBody2D
{
	/// <summary>
	///The current Unit's state
	/// </summary>
	private enum StatesEnum { Walking, Aiming};

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		radius = GetNode<CollisionShape2D>("UnitRadius");
		tween = GetNode<Tween>("Tween");

		sprite = GetNode<Sprite>("Unit");
		vectorPath = new GC.Array<Vector2>();pathGuide = GetNode<Line2D>("PathGuide");
		SetProcess(false);

		// GD.Print(getReference(90, -275));
		// GD.Print(getReference(0, 240));
	}
	
	
	//Game loop in a different process
	public override void _Process(float delta)
	{
		// Sync speed for this frame
		float distance = speed * delta;

		moveToPath(distance);

		if(state == StatesEnum.Aiming)
			aim(target);

		rotate(delta);

		if(openFire)
			Shoot();
		atkCooldown -= delta;
	}


	// Unit detection
	private void presenceDetected(object body)
	{
		GC.Array enemies = GetTree().GetNodesInGroup("Enemies");
		
		if(!enemies.Contains(body)) return;

		GD.Print("Opp Detected");
		if(target != null) return;

		target = body as Unit;
		state = StatesEnum.Aiming;
	}
	
	
	// Unit exits
	private void presenceExited(object body)
	{
		if(body as Unit != target) return;
		target = null;
		state = StatesEnum.Walking;
	}


	//Rotates towards target
	private void rotate(float delta)
	{
		if(state == StatesEnum.Walking)
		{
			if(vectorPath.Count != 0)
			{
				aim(vectorPath[0]);
			}
		}

		float diff = targetAngle - sprite.Rotation;
		diff = UF.angleRadianReference(diff);
		float rotation = Mathf.Deg2Rad(rotationSpeed) * delta;
		rotation *= (diff < 0) ? -1 : 1;
		if(rotation > Mathf.Abs(diff))
			rotation = diff;

		sprite.Rotation += rotation;
	}


	// Updates the targetAngle to aim at target
	private void aim(Unit target)
	{
		// Get euclidian distance
		float deltaX = Position.x - target.Position.x;
		float deltaY = Position.y - target.Position.y;
		targetAngle = Mathf.Atan2(deltaY, deltaX) - (Mathf.Pi/2);
	}
	

	private void aim(Vector2 target)
	{
		// Get euclidian distance
		float deltaX = Position.x - target.x;
		float deltaY = Position.y - target.y;
		targetAngle = Mathf.Atan2(deltaY, deltaX) - (Mathf.Pi/2);
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
	

	//Shoot based on where they currently are facing
	private void Shoot()
	{
		if(atkCooldown > 0) return;
		GD.Print("Generating Bullets");
		Bullet b = SceneManager.GetSceneInstance(bulletScene) as Bullet;
		float rotation = sprite.Rotation - (Mathf.Pi/2);
		Vector2 velocity = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));
		velocity *= bulletSpeed;
		b.Init(range, velocity);
		Game.addBullet(Position, b);
		atkCooldown = atkSpeed;
	}


	//Amount of distance to be travelled this frame
	private void moveToPath(float distance)
	{
		if(state != StatesEnum.Aiming)
			state = StatesEnum.Walking;

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


	public float Radius{
		get{return (radius.Shape as CircleShape2D).Radius + 10; }
	}

	//Values
	const float speed = 300;
	const float range = 500;
	const float rotationSpeed = 160; //Rotation Speed in degrees
	const float bulletSpeed = 600;
	const float atkSpeed = 0.6f;
	float atkCooldown = 0;
	float targetAngle;
	StatesEnum state;
	public bool openFire = false;
	//Scene Reference
	const String bulletScene = "res://Objects/Controller/Components/Components/Bullet.tscn";
	//Components
	CollisionShape2D radius;
	Tween tween;
	// Pathway when issued to Move
	GC.Array<Vector2> vectorPath;
	Line2D pathGuide;
	int pointsReached;
	// Components
	Sprite sprite;
	// Targets
	Unit target;
}
