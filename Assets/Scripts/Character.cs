using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : MonoBehaviour {

    public int frame;
    public int health;
    public int damage;
    public bool alive;
    public string action;
    public Transform selectionPrefab;
    private Transform selectionCircle;
    float displacedX, displacedY;

    private int originalX;
    private int originalY;

	void Start () {
        action = "nothing";
        alive = true;
        frame = 0;
        originalX = getX();
        originalY = getY();
        selectionCircle = null;
        displacedX = 0; displacedY = 0;
    }

	void Update () {
        if(selectionCircle != null) {
            selectionCircle.position = transform.position;
        }
    }

    public string nextState() {
        string newState = GetComponent<HypnoScript>().nextAction();
        setAction(newState);
        return newState;
    }

    public void setAction(string state) {
        frame = 0;
        action = "nothing";
        if (state.Equals("moveRight")) {
            if (canMove(1, 0)) {
                action = "moveRight";
            }else {
                action = "blockedRight";
            }
        }else if (state.Equals("moveLeft")) {
            if (canMove(-1, 0)) {
                action = "moveLeft";
            }else {
                action = "blockedLeft";
            }
        }else if (state.Equals("moveUp")) {
            if (canMove(0, 1)) {
                action = "moveUp";
            }else {
                action = "blockedUp";
            }
        }else if (state.Equals("moveDown")) {
            if (canMove(0, -1)) {
                action = "moveDown";
            }else {
                action = "blockedDown";
            }
        }
        else if (state.Equals("attackRight")) {
            GameMaster.attackSpace(getX() + 1, getY());
            action = "attackRight";
        }
        else if (state.Equals("attackLeft")) {
            GameMaster.attackSpace(getX() - 1, getY());
            action = "attackLeft";
        }
        else if (state.Equals("attackUp")) {
            GameMaster.attackSpace(getX(), getY() + 1);
            action = "attackUp";
        }
        else if (state.Equals("attackDown")) {
            GameMaster.attackSpace(getX(), getY() - 1);
            action = "attackDown";
        }else if (state.Equals("malfunction")) {
            action = "malfunction";
        }
    }

    public void performAction(int frameToMeet) {
        if(frameToMeet <= frame) {
            return;
        }

        if (action.Equals("moveRight")) {
            transform.Translate(new Vector3(0.1f, 0, 0));
        }else if (action.Equals("moveLeft")) {
            transform.Translate(new Vector3(-0.1f, 0, 0));
        }else if (action.Equals("moveUp")) {
            transform.Translate(new Vector3(0, 0.1f, 0));
        }else if (action.Equals("moveDown")) {
            transform.Translate(new Vector3(0, -0.1f, 0));
        }else if (action.Equals("attackRight")) {
            if (frame < 3) {
                transform.Translate(new Vector3(0.14f, 0, 0));
            }else {
                transform.Translate(new Vector3(-0.06f, 0, 0));
            }
        }else if (action.Equals("attackLeft")) {
            if (frame < 3) {
                transform.Translate(new Vector3(-0.14f, 0, 0));
            }
            else {
                transform.Translate(new Vector3(0.06f, 0, 0));
            }
        }else if (action.Equals("attackUp")) {
            if (frame < 3) {
                transform.Translate(new Vector3(0, 0.14f, 0));
            }
            else {
                transform.Translate(new Vector3(0, -0.06f, 0));
            }
        }else if (action.Equals("attackDown")) {
            if (frame < 3) {
                transform.Translate(new Vector3(0, -0.14f, 0));
            }
            else {
                transform.Translate(new Vector3(0, 0.06f, 0));
            }
        }else if (action.Equals("blockedRight")) {
            if (frame < 2) {
                transform.Translate(new Vector3(0.1f, 0, 0));
            }
            else {
                transform.Translate(new Vector3(-0.025f, 0, 0));
            }
        }else if (action.Equals("blockedLeft")) {
            if (frame < 2) {
                transform.Translate(new Vector3(-0.1f, 0, 0));
            }
            else {
                transform.Translate(new Vector3(0.025f, 0, 0));
            }
        }else if (action.Equals("blockedUp")) {
            if (frame < 2) {
                transform.Translate(new Vector3(0, 0.1f, 0));
            }
            else {
                transform.Translate(new Vector3(0, -0.025f, 0));
            }
        }else if (action.Equals("blockedDown")) {
            if (frame < 2) {
                transform.Translate(new Vector3(0, -0.1f, 0));
            }
            else {
                transform.Translate(new Vector3(0, 0.025f, 0));
            }
        }else if (action.Equals("malfunction")) {
            if(frame % 2 == 0) {
                displacedX += UnityEngine.Random.Range(-5, 5) * 0.05f;
                displacedY += UnityEngine.Random.Range(-5, 5) * 0.05f;
                transform.Translate(new Vector3(displacedX, displacedY, 0));
            }
            else {
                transform.Translate(new Vector3(-displacedX, -displacedY, 0));
                displacedX = 0;
                displacedY = 0;
            }
        }

        frame++;
        if(frame < frameToMeet) {
            performAction(frameToMeet);
        }
    }

    public int getX() {
        return (int)Math.Round(transform.position.x);
    }

    public int getY() {
        return (int)Math.Round(transform.position.y);
    }

    private bool canMove(float dx, float dy) {
        return GameMaster.spaceIsClear((int)(getX() + dx), (int)(getY() + dy));
        //return (GameMaster.spaceIsClear((int)(transform.position.x + dx), (int)(transform.position.y - 0.1 + dy)) || ((int)(transform.position.x + dx) == getX() && (int)(transform.position.y - 0.1 + dy) == getY()));
    }
    
    public void die() {
        alive = false;
        Deselect();
    }

    public void resetCharacter() {
        transform.position = new Vector3(originalX, originalY + 0.1f, transform.position.z);
        GetComponent<HypnoScript>().resetIndex();
        alive = true;
        GetComponent<CharacterAnimation>().setAnimRunning(false);
        frame = 0;
        action = "nothing";
    }

    public void Select() {
        selectionCircle = Instantiate(selectionPrefab);
    }

    public void Deselect() {
        if (selectionCircle != null) {
            Destroy(selectionCircle.gameObject);
            selectionCircle = null;
        }
    }

    public Boolean isSelected() {
        return (selectionCircle != null);
    }
}