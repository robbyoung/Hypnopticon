using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour {

    public void startGame() {
        GameMaster.startGame();
    }

    public void startCycle() {
        GameMaster.startCycle();
    }

    public void loop() {
        if (GameMaster.loop) {
            GameMaster.loop = false;
        }else {
            GameMaster.loop = true;
        }
    }

    public void resetGame() {
        GameMaster.resetGame();
    }
}
