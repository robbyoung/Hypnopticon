using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hypnopticon : MonoBehaviour {

    public static bool storyMode;
    public static string nextConvo;
    public static string nextBattle;
    public static List<int> unitCount;

	void Start () {
        unitCount = new List<int>();
        storyMode = false;
        nextConvo = "Random";
        nextBattle = "Random";
        unitCount.Add(0);
        unitCount.Add(0);
        unitCount.Add(0);
	}
}
