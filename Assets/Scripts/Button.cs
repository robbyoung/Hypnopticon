using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


public class Button : MonoBehaviour {

    //public InputField saveText;
    public GameObject menuScreen;

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

    public void goToField() {
        SceneManager.LoadScene("Field");
    }

    public void goToStory() {
        Hypnopticon.storyMode = true;
        SceneManager.LoadScene("Conversation");
    }

    public void changeCharacterType(int index) {
        GameMaster.obstacleMenu = false;
        GameMaster.currentType = index;
    }

    public void changeObstacleType(int index) {
        GameMaster.obstacleMenu = true;
        GameMaster.currentObstacleType = index;
    } 

    public void clearField() {
        GameMaster.deleteAllCharacters();
        if (Hypnopticon.storyMode) {
            GameMaster.loadScenario(Hypnopticon.nextBattle);
        }
        resume();
    }

    public void quit() {
        Application.Quit();
    }

    public void resume() {
        menuScreen.SetActive(false);
    }

    public InputField saveText;
    public void SaveScenario() {
        if (Hypnopticon.storyMode) return;
        string fileName = "Assets/Scenarios/" + saveText.text + ".txt";
        if (File.Exists(fileName)) {
            Debug.Log("Deleting " + fileName);
            File.Delete(fileName);
        }
        var sr = File.CreateText(fileName);
        sr.WriteLine(GameMaster.exportScenario());
        sr.Close();
        Debug.Log("Saved " + fileName);
        resume();
    }


    public void LoadScenario() {
        if (Hypnopticon.storyMode) return;
        GameMaster.loadScenario(saveText.text);
        resume();
    }


}
