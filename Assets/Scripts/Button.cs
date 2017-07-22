﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;


public class Button : MonoBehaviour {

    public InputField saveText;

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

    public void changeCharacterType(int index) {
        GameMaster.currentType = index;
    }

    public void SaveScenario() {
        string fileName = "Assets/Scenarios/" + saveText.text + ".txt";
        if (File.Exists(fileName)) {
            Debug.Log(fileName + " already exists.");
            return;
        }
        var sr = File.CreateText(fileName);
        sr.WriteLine(GameMaster.exportScenario());
        sr.Close();
    }

    public void LoadScenario() {
        string fileName = "Assets/Scenarios/" + saveText.text + ".txt";
        if (File.Exists(fileName)) {
            StreamReader reader = new StreamReader(fileName);
            GameMaster.importScenario(reader.ReadToEnd());
            reader.Close();
        }else {
            Debug.Log(fileName + " doesn't exist.");
        }
    }
}
