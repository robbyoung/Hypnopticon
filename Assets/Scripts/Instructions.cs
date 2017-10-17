using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instructions : MonoBehaviour {

    public List<Info> info;
    private int index;
    public Text title;
    public Text content;


    // Use this for initialization
    void Start () {
        index = 0;
        info = new List<Info>();
        info.Add(new Info("HYPNOPTICON", "Control the minds of your friends and family!"));
        info.Add(new Info("ACTIONS", "These are actions that the unit will perform. They can perform one per turn."));
        info.Add(new Info("MOV", "Move Forward - the unit will move one space in the direction they are facing."));
        info.Add(new Info("MVB", "Move Backward - the unit will move to the space behind them."));
        info.Add(new Info("MVL", "Move Left - the unit will move one space to their left."));
        info.Add(new Info("MVR", "Move Right - the unit will move one space to their right."));
        info.Add(new Info("LFT", "Turn Left - the unit will turn 90 degrees to their left."));
        info.Add(new Info("RGT", "Turn Right - the unit will turn 90 degrees to their right."));
        info.Add(new Info("FLP", "Flip - the unit will turn 180 degrees."));
        info.Add(new Info("RDM", "Random Movement - the unit has a chance to move forward, turn left, or turn right."));
        info.Add(new Info("RDA", "Random Advanced - the unit has a chance to move forward, move left, move right, move backward, turn left, turn right, or flip."));
        info.Add(new Info("ATK", "Attack Forward - the unit will attack the space directly in front of it."));
        info.Add(new Info("ATL", "Attack Left - the unit will attack the space to its left."));
        info.Add(new Info("ATR", "Attack Right - the unit will attack the space to its right."));
        info.Add(new Info("RAT", "Ranged Attack - the unit will fire its ranged weapon in the direction they are facing if they are able to."));
        info.Add(new Info("RLD", "Reload - the unit will reload its weapon if it needs to."));
        info.Add(new Info("SEE", "Target - the until will keep track of the enemy in front of it."));
        info.Add(new Info("SENSES", "These are conditions that the unit will check to decide what it should do next. They do not take up the unit's turn."));

        title.text = info[index].title;
        content.text = info[index].content;
	}
	
	public void next() {
        index = index + 1;
        if(index >= info.Count) {
            index = 0;
        }
        title.text = info[index].title;
        content.text = info[index].content;
    }

    public void previous() {
        index = index - 1;
        if (index < 0) {
            index = info.Count - 1;
        }
        title.text = info[index].title;
        content.text = info[index].content;
    }


}

public class Info {
    public string title;
    public string content;

    public Info(string t, string c) {
        title = t;
        content = c;
    }
}
