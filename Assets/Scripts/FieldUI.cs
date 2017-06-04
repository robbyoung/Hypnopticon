using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI : MonoBehaviour {

    public Slider fpsSlider;
    public Text fpsText;
    private List<GameObject> selected;
    public InputField input;
    public GameObject scriptBlock;
    public Text commandList;
    private bool scriptingEnabled;

	// Use this for initialization
	void Start() {
        scriptingEnabled = false;
        selected = new List<GameObject>();
    }

    // Update is called once per frame
    void Update() {
        GameMaster.fps = (int)Math.Round(fpsSlider.value);
        fpsText.text = (int)Math.Round(fpsSlider.value) + " FPS";
        if(selected.Count > 0) {
            commandList.text = selected[selected.Count - 1].GetComponent<HypnoScript>().getCommands();
            //selection.transform.position = selected.transform.position;
        }else {
            commandList.text = "";
        }
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            int x = (int)Math.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
            int y = (int)Math.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            if (GameMaster.withinField(x, y)){
                GameObject newSelection = GameMaster.objectAtSpace(x, y);
                if (newSelection != null /*&& !GameMaster.started*/) {
                    if (Input.GetKey(KeyCode.LeftControl)) {
                        selected.Add(newSelection);
                        newSelection.GetComponent<Character>().Select();
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
                } else {
                    for(int i = 0; i < selected.Count; i++) {
                        selected[i].GetComponent<Character>().Deselect();
                    }
                    selected = new List<GameObject>();
                    hideScripting();
                }
            }
        }else if (GameMaster.started) {
            hideScripting();
        }
        if (Input.GetKeyDown(KeyCode.Delete) && selected.Count > 0) {
            if(GameMaster.started) {
                for (int i = 0; i < selected.Count; i++) {
                    selected[i].GetComponent<Character>().die();
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

        if (Input.GetKeyDown(KeyCode.Q) && !GameMaster.started) {
            rotateCharacterLeft((int)Math.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), (int)Math.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
        }else if (Input.GetKeyDown(KeyCode.E) && !GameMaster.started) {
            rotateCharacterRight((int)Math.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), (int)Math.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
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

    public void rotateCharacterLeft(int x, int y) {
        HypnoScript character;
        GameObject obj = GameMaster.objectAtSpace(x, y);
        if(obj != null) {
            character = obj.GetComponent<HypnoScript>();
            character.rotateLeft();
        }
    }

    public void rotateCharacterRight(int x, int y) {
        HypnoScript character;
        GameObject obj = GameMaster.objectAtSpace(x, y);
        if (obj != null) {
            character = obj.GetComponent<HypnoScript>();
            character.rotateRight();
        }
    }
}
