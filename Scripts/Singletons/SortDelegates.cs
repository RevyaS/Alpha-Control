/*  Class dedicated for Customizable Sorting methods to be used for 
    DataManager's Sort functions.
    Key to adding functions: 
    1. requires only 2 object parameters and returns an 
    object parameter based on specific conditions
    2. add a static SortDelegate variable and connect the function
    to the variable via _Ready() function
*/
using Godot;
using System;

public class SortDelegates : Node
{
    //List of delegates
    public static SortDelegate IntDescending, IntAscending, 
                               Vector2XAscending, Vector2YAscending, 
                               UnitsPosXAscending, UnitsPosYAscending;

    public override void _Ready()
    {
        IntAscending = Delegate.CreateDelegate(typeof(SortDelegate), this, nameof(smallerInt)) as SortDelegate;
        IntDescending = Delegate.CreateDelegate(typeof(SortDelegate), this, nameof(largerInt)) as SortDelegate;
        Vector2XAscending = Delegate.CreateDelegate(typeof(SortDelegate), this, nameof(smallerXVector2)) as SortDelegate;
        Vector2YAscending = Delegate.CreateDelegate(typeof(SortDelegate), this, nameof(smallerYVector2)) as SortDelegate;
        UnitsPosXAscending = Delegate.CreateDelegate(typeof(SortDelegate), this, nameof(leftmostUnit)) as SortDelegate;
        UnitsPosYAscending = Delegate.CreateDelegate(typeof(SortDelegate), this, nameof(bottommostUnit)) as SortDelegate;
    }

//SORT FUNCTION DELEGATES
    public delegate object SortDelegate(object a, object b);

    //Int
    private object largerInt(object oa, object ob)
    {
        int a = Convert.ToInt32(oa);
		int b = Convert.ToInt32(ob); 
		int output = (a > b) ? a : b;
		return output;
    }

    private object smallerInt(object oa, object ob)
    {
        int a = Convert.ToInt32(oa);
		int b = Convert.ToInt32(ob); 
		int output = (a < b) ? a : b;
		return output;
    }

    //Vector2
    private object smallerXVector2(object oa, object ob)
	{
		Vector2 a = (Vector2)oa;
		Vector2 b = (Vector2)ob;
		if(a.x != b.x)
			return (a.x < b.x) ? a : b;
        return a;
    }


    private object smallerYVector2(object oa, object ob)
    {
		Vector2 a = (Vector2)oa;
		Vector2 b = (Vector2)ob;
		if(a.y != b.y)
			return (a.y < b.y) ? a : b;
        return a;
    }

    //Units
    private object leftmostUnit(object oa, object ob)
    {
        Unit a = (Unit)oa;
        Unit b = (Unit)ob;
        if((Vector2)smallerXVector2(a.Position, b.Position) == a.Position) return a;
        else return b;
    }

    private object bottommostUnit(object oa, object ob)
    {
        Unit a = (Unit)oa;
        Unit b = (Unit)ob;
        if((Vector2)smallerYVector2(a.Position, b.Position) == a.Position) return a;
        else return b;
    }
}