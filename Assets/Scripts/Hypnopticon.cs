using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hypnopticon : MonoBehaviour {

    public static bool storyMode;
    public static string nextConvo;
    public static string location;
    public static string nextLocation;
    public static List<string> neighbours;
    public static List<string> nextNeighbours;
    public static string nextBattle;
    public static List<int> unitCount;
    public static List<string> presets;

	void Start () {
        unitCount = new List<int>();
        storyMode = false;
        nextConvo = "intro";
        nextBattle = "intro";
        nextLocation = "Village Outskirts";
        nextNeighbours = new List<string>();
        nextNeighbours.Add("Coastal Woods");
        nextNeighbours.Add("Village");
        unitCount.Add(1);
        unitCount.Add(0);
        unitCount.Add(0);
	}
}
