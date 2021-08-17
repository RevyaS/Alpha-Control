using Godot;
using System;

public class UtilityFunctions : TextureRect
{
    //ANGLE FUNCTIONS
    /// <summary>
    ///Returns the version of the radian Angle closest to 0
    /// </summary>
    /// <param name="angle">Angle in radians</param>
    public static float angleRadianReference(float angle)
    {
        float degAngle = Mathf.Rad2Deg(angle);
        float oppAngle = degAngle;
        oppAngle += (degAngle < 0) ? 360 : -360;

        degAngle += (degAngle < -180) ? 360 : (degAngle > 180) ? -360 : 0;
        oppAngle += (oppAngle < -180) ? 360 : (oppAngle > 180) ? -360 : 0;
        
        if(Math.Abs(oppAngle) < Mathf.Abs(degAngle))
            degAngle = oppAngle;

        return Mathf.Deg2Rad(degAngle);
    }
}