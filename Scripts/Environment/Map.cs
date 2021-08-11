using Godot;
using System;

public class Map : TextureRect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		navigator = GetNode<Navigation2D>("Nav");
	}

	//Properties
	public Navigation2D Navigator
	{
		get{return navigator;}
	}

	//Variables
	Navigation2D navigator;
}
