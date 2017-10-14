using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FieldUI : MonoBehaviour {

    public List<Text> unitCounts;
    public GameObject winText;
    public Text fpsText;
    private List<GameObject> selected;
    public InputField input;
    public GameObject scriptBlock;
    public GameObject menu;
    public Text commandList;
    private bool scriptingEnabled;
    private int[] shiftCoords;

	// Use this for initialization
	void Start() {
        menu.SetActive(false);
        scriptingEnabled = false;
        selected = new List<GameObject>();
        shiftCoords = new int[] { 0, 0 };
    }

    // Update is called once per frame
    void Update() {
        
        if(GameMaster.characterCount() == 0) {
            selected = new List<GameObject>();
        }
        fpsText.text = GameMaster.fps + "FPS";
        if(selected.Count > 0) {
            commandList.text = selected[selected.Count - 1].GetComponent<HypnoScript>().getCommands();
            //selection.transform.position = selected.transform.position;
        }else {
            commandList.text = "";
        }
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            int x = (int)Math.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
            int y = (int)Math.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            if (GameMaster.withinField(x, y)) {
                if (Input.GetKey(KeyCode.LeftShift)) {
                    shiftClick(Math.Min(x, shiftCoords[0]), Math.Min(y, shiftCoords[1]), Math.Max(x, shiftCoords[0]), Math.Max(y, shiftCoords[1]));
                } else {
                    GameObject newSelection = GameMaster.objectAtSpace(x, y);
                    if (newSelection != null /*&& !GameMaster.started*/) {
                        if (Input.GetKey(KeyCode.LeftControl)) {
                            bool repeated = false;
                            for (int i = 0; i < selected.Count; i++) {
                                if (newSelection == selected[i]) {
                                    selected.RemoveAt(i);
                                    newSelection.GetComponent<Character>().Deselect();
                                    repeated = true;
                                }
                            }
                            if (!repeated) {
                                selected.Add(newSelection);
                                newSelection.GetComponent<Character>().Select();
                            }
                        }
                        else {
                            for (int i = 0; i < selected.Count; i++) {
                                selected[i].GetComponent<Character>().Deselect();
                            }
                            selected = new List<GameObject>();
                            selected.Add(newSelection);
                            newSelection.GetComponent<Character>().Select();
                        }
                        if (!GameMaster.started) {
                            showScripting();
                        }
                    }
                    else if (!Input.GetKey(KeyCode.LeftControl)) { 
                        for (int i = 0; i < selected.Count; i++) {
                            selected[i].GetComponent<Character>().Deselect();
                        }
                        selected = new List<GameObject>();
                        hideScripting();
                    }
                    shiftCoords = new int[] { x, y };
                }
            }
        }else if (GameMaster.started) {
            hideScripting();
        }

        if (Input.GetKeyDown(KeyCode.Delete) && selected.Count > 0) {
            if(GameMaster.started && !Hypnopticon.storyMode) {
                for (int i = 0; i < selected.Count; i++) {
                    selected[i].GetComponent<Character>().die(10000, false);
                }
            }else {
                GameMaster.deleteCharacters();
                for (int i = 0; i < selected.Count; i++) {
                    selected[i].GetComponent<Character>().Deselect();
                }
                selected = new List<GameObject>();
                hideScripting();
            }
        }

        if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.LeftControl)) {
            selected = GameMaster.selectAllPlayers();
            showScripting();
        }

        if (Input.GetKeyDown(KeyCode.Q) && !GameMaster.started && scriptingEnabled) {
            for (int i = 0; i < selected.Count; i++) {
                selected[i].GetComponent<HypnoScript>().setLeft();
            }
        }else if (Input.GetKeyDown(KeyCode.E) && !GameMaster.started && scriptingEnabled) {
            for (int i = 0; i < selected.Count; i++) {
                selected[i].GetComponent<HypnoScript>().setRight();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && GameMaster.fps > 1) {
            GameMaster.fps /= 2;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && GameMaster.fps < 512) {
            GameMaster.fps *= 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            if (selected.Count == 0) {
                selected = GameMaster.selectTeam(1);
                showScripting();
            }
            else if (!GameMaster.started && !Hypnopticon.storyMode) {
                for (int i = 0; i < selected.Count; i++) {
                    selected[i].GetComponent<Character>().setTeam(1);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            if (selected.Count == 0) {
                selected = GameMaster.selectTeam(2);
                if (!Hypnopticon.storyMode) {
                    showScripting();
                }
            }
            else if(!GameMaster.started && !Hypnopticon.storyMode) {
                for (int i = 0; i < selected.Count; i++) {
                    selected[i].GetComponent<Character>().setTeam(2);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            if (selected.Count == 0) {
                selected = GameMaster.selectTeam(3);
                if (!Hypnopticon.storyMode) {
                    showScripting();
                }
            }
            else if (!GameMaster.started && !Hypnopticon.storyMode) {
                for (int i = 0; i < selected.Count; i++) {
                    selected[i].GetComponent<Character>().setTeam(3);
                }
            }
        }

        //For testing Random scenario gen
        /*if (Input.GetKeyDown(KeyCode.R)) {
            for (int i = 0; i < selected.Count; i++) {
                selected[i].GetComponent<Character>().Deselect();
            }
            selected = new List<GameObject>();
            GameMaster.randomScenario();
            GameMaster.loadScenario("Random");
        }*/
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            if (selected.Count == 0) {
                selected = GameMaster.selectTeam(4);
                if (!Hypnopticon.storyMode) {
                    showScripting();
                }
            }
            else if (!GameMaster.started && !Hypnopticon.storyMode) {
                for (int i = 0; i < selected.Count; i++) {
                    selected[i].GetComponent<Character>().setTeam(4);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            GameMaster.resetGame();
            menu.SetActive(true);
        }

        //Disallow the player from changing enemy scripts.
        if (Hypnopticon.storyMode) {
            for (int i = 0; i < selected.Count; i++) {
                if (selected[i].GetComponent<Character>().team != 1) {
                    hideScripting();
                }
            }
        }
        if (GameMaster.win && Hypnopticon.storyMode) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                GameMaster.newRecruits();
                Hypnopticon.nextBattle = Hypnopticon.nextConvo;
                SceneManager.LoadScene("Map");
            }
            winText.SetActive(true);
        }
        else {
            winText.SetActive(false);
        }

        if (Hypnopticon.storyMode) {
            for (int i = 0; i < unitCounts.Count; i++) {
                unitCounts[i].text = "" + Hypnopticon.unitCount[i];
            }
        }
    }

    public void addCommand() {
        input.Select();
        for (int i = 0; i < selected.Count; i++) {
            selected[i].GetComponent<HypnoScript>().addToScript(input.text);
        }
        input.text = "";
    }

    public void removeCommand() {
        for (int i = 0; i < selected.Count; i++) {
            selected[i].GetComponent<HypnoScript>().removeFromScript();
        }
    }

    public void showScripting() {
        scriptBlock.SetActive(true);
        scriptingEnabled = true;
    }

    public void hideScripting() {
        scriptBlock.SetActive(false);
        scriptingEnabled = false;
    }

    public void shiftClick(int minX, int minY, int maxX, int maxY) {
        for(int x = minX; x <= maxX; x++) {
            for(int y = minY; y <= maxY; y++) {
                GameObject c = GameMaster.objectAtSpace(x, y);
                if(c != null) {
                    c.GetComponent<Character>().Select();
                    selected.Add(c);
                }
            }
        }
    }

    
}
