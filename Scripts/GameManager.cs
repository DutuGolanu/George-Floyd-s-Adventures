using Godot;
using System;

public partial class GameManager : Node
{
    static string SavePath = "user://GeorgeFloydSaveData.dat";
    public static void Save(){
		using var file = Godot.FileAccess.Open(SavePath,Godot.FileAccess.ModeFlags.Write);
		file.StoreVar(Main.highScore);
	}
	public static void Load(){
		using var file = Godot.FileAccess.Open(SavePath,Godot.FileAccess.ModeFlags.Read);
		if (Godot.FileAccess.FileExists(SavePath)){
			Main.highScore = (int)file.GetVar();
	    }
    }
}
