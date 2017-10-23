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
    public static List<string> presets;
    public static List<CharacterHusk> units;

	void Start () {
        storyMode = false;
        nextConvo = "intro";
        nextBattle = "intro";
        nextLocation = "Village Outskirts";
        nextNeighbours = new List<string>();
        nextNeighbours.Add("Coastal Woods");
        nextNeighbours.Add("Village");
        location = nextLocation;
        neighbours = nextNeighbours;

        units = new List<CharacterHusk>();
        units.Add(new CharacterHusk("Gordon", 0));
        units[0].attack = 30;
        units[0].speed = 1;
        units[0].defense = 3;
	}
}
