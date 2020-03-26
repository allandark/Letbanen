using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Destination
{
    public string Name;
    public Vector2 Position;
}

[System.Serializable]
public class Track
{
    public TrackType Type;
    public Texture Texture;
}

[System.Serializable]
public class Conditional
{
    public string Title;
    public string Text;
}

[System.Serializable]
public class Goal
{
    public string Text;
    public string destination1Name, destination2Name;
}

public enum TrackType
{
    None = -1,
    Turn = 0,
    Cross = 1,
    Straight = 2,
    End = 3
}



[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public List<Destination> destinations = new List<Destination>();
    public List<Track> tracks = new List<Track>();
    public List<Conditional> conditions = new List<Conditional>();
    public List<Goal> goals = new List<Goal>();

}
