using Godot;
using System;
using GC = Godot.Collections;

/*  Handles anything related to Scenes or Scene Centric
    algorithms
*/
public class SceneManager : Node
{
    /// <summary>
    ///Returns the object Instance of a scene
    /// </summary>
    /// <param name="ScenePath">Path of .tscn file</param>
    /// <returns></returns>
    public static object GetSceneInstance(String ScenePath)
        => ((PackedScene)ResourceLoader.Load(ScenePath)).Instance();

}