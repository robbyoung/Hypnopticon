using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Textbox : MonoBehaviour {
    public string type;

	void Update () {
        Text t = GetComponent<Text>();
        if(type.Equals("loop")) {
            if (GameMaster.loop) {
                t.text = "Loop: ON";
            }else {
                t.text = "Loop: OFF";
            }
        }
	}
}
