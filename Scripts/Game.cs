using Godot;
using System;

public class Game : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{		
		controller = GetNode<Formation>("Control");
		Map = GetNode<Map>("Map");

		//HUD
		lineFormationToggle = GetNode<TextureButton>("HUD/HBoxContainer/SquareFormation");
	}

	private enum ButtonTypes {FormationButton, AttackToggle};

	//Button Triggered
	private void buttonToggled(bool toggle, ButtonTypes buttonType)
	{
		switch(buttonType)
		{
			case ButtonTypes.FormationButton:
				controller.formationOn = toggle;
				break;
			case ButtonTypes.AttackToggle:
				controller.OpenFire = toggle;
				break;
		}
	}
	
	// Input call
	public override void _Input(InputEvent @event)
	{
		InputEvent ev = @event;
		if(ev is InputEventMouseButton)
		{
			InputEventMouseButton btn = ev as InputEventMouseButton;
			// Check for Right Click then generate path
			if(btn.ButtonIndex == 2) 
			{
				if(ev.IsPressed()) return;

				GD.Print("Move Issued");
				controller.Move(btn.GlobalPosition);
			}
		}
	}

	public static void addBullet(Vector2 pos, Bullet bullet)
	{
		Map.addBullet(pos, bullet);
	}


	Formation controller;
	/// <summary>
	///Current Map used in game
	/// </summary>
	public static Map Map;
	//HUD
	TextureButton lineFormationToggle;
}
