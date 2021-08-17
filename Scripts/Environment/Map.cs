using Godot;
using System;

public class Map : TextureRect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		navigator = GetNode<Navigation2D>("Nav");
        bulletList = GetNode<Node2D>("BulletList");
    }

	//Properties
	public Navigation2D Navigator
	{
		get{return navigator;}
	}

	public void addBullet(Vector2 pos, Bullet bullet)
	{
        bullet.Position = pos;
        bulletList.AddChild(bullet);
        GD.Print(bulletList.GetChildCount());
    }

	//Variables
	Navigation2D navigator;
    Node2D bulletList;
}
