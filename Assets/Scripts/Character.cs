using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : MonoBehaviour {

    private int frame;
    public int maxHealth;
    private int health;
    public int attack;
    public int defense;
    private int stamina;
    public int maxStamina;
    public int speed;
    private int speedstack;
    public bool alive;
    public string action;
    public Transform selectionPrefab1;
    public Transform selectionPrefab2;
    public Transform selectionPrefab3;
    public Transform selectionPrefab4;
    private Transform selectionCircle;
    float displacedX, displacedY;
    public int team;
    public int type;

    private int originalX;
    private int originalY;

	void Start () {
        selectionCircle = null;
        team = 1;
        health = maxHealth;
        stamina = maxStamina;
        speedstack = speed;
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
            selectionCircle.position = new Vector3(transform.position.x, transform.position.y, transform.position.z+0.05f);
        }
    }

    public string nextState() {
        string newState = GetComponent<HypnoScript>().getNextAction();
        setAction(newState);
        return newState;
    }

    public void setAction(string state) {
        frame = 0;
        action = "nothing";
        if (!GetComponent<HypnoScript>().getCharacterCommand().Equals("")) {
            state = GetComponent<HypnoScript>().getCharacterCommand();
        }
        if (state.Equals("moveRight")) {

            if (canMove(1, 0)) {
                action = "moveRight";
            }
            else {
                action = "blockedRight";
            }
        }
        else if (state.Equals("moveLeft")) {
            if (canMove(-1, 0)) {
                action = "moveLeft";
            }
            else {
                action = "blockedLeft";
            }
        }
        else if (state.Equals("moveUp")) {
            if (canMove(0, 1)) {
                action = "moveUp";
            }
            else {
                action = "blockedUp";
            }
        }
        else if (state.Equals("moveDown")) {
            if (canMove(0, -1)) {
                action = "moveDown";
            }
            else {
                action = "blockedDown";
            }
        }
        else if (state.Equals("attackRight")) {
            GameMaster.attackSpace(attack, getX() + 1, getY());
            action = "attackRight";
        }
        else if (state.Equals("attackLeft")) {
            GameMaster.attackSpace(attack, getX() - 1, getY());
            action = "attackLeft";
        }
        else if (state.Equals("attackUp")) {
            GameMaster.attackSpace(attack, getX(), getY() + 1);
            action = "attackUp";
        }
        else if (state.Equals("attackDown")) {
            GameMaster.attackSpace(attack, getX(), getY() - 1);
            action = "attackDown";
        }else if (state.Equals("rangedRight")) {
            action = "attackRight";
        }
        else if (state.Equals("rangedLeft")) {
            action = "attackLeft";
        }
        else if (state.Equals("rangedUp")) {
            action = "attackUp";
        }
        else if (state.Equals("rangedDown")) {
            action = "attackDown";
        }
        else if (state.Equals("malfunction")) {
            die(1, false);
            if (alive) {
                action = "malfunction";
            }
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
    
    public void die(int damage, bool inclDef) {
        if (inclDef) {
            damage -= defense;
            if(damage < 1) {
                damage = 1;
            }
        }
        if (damage >= health) {
            health = 0;
            stamina = 0;
            alive = false;
            Deselect();
        }else {
            health -= damage; 
        }
        GameMaster.animate("blood", transform.position.x, transform.position.y, transform.position.z - 0.01f);
    }

    public void resetCharacter() {
        transform.position = new Vector3(originalX, originalY + 0.1f, transform.position.z);
        GetComponent<HypnoScript>().resetIndex();
        alive = true;
        GetComponent<CharacterAnimation>().setAnimRunning(false);
        frame = 0;
        action = "nothing";
        health = maxHealth;
        stamina = maxStamina;
        speedstack = speed;
    }

    public void Select() {
        Deselect();
        if (team == 1) {
            selectionCircle = Instantiate(selectionPrefab1);
        }
        else if(team == 2) {
            selectionCircle = Instantiate(selectionPrefab2);
        }
        else if(team == 3) {
            selectionCircle = Instantiate(selectionPrefab3);
        }
        else if (team == 4) {
            selectionCircle = Instantiate(selectionPrefab4);
        }
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

    public bool willMove() {
        speedstack -= 1;
        if(speedstack < 0) {
            speedstack = speed;
            return true;
        }
        return false;
    }

    public int getSpeed() {
        return speed;
    }

    public float percentHealth() {
        return health * 1.0f / maxHealth;
    }

    public float percentStamina() {
        return stamina * 1.0f / maxStamina;
    }

    public void setTeam(int t) {
        team = t;
        Select();
    }

    public void import(string s) {
        string[] info = s.Split(' ');
        originalX = Convert.ToInt32(info[0]);
        originalY = Convert.ToInt32(info[1]);
        transform.position = new Vector3(originalX, originalY + 0.1f, transform.position.z);
        team = Convert.ToInt32(info[2]);
        GetComponent<HypnoScript>().setDirection(info[3]);
        GetComponent<CharacterAnimation>().setDirection(info[3]);
    }

    public string export() {
        string exp = type + "\n" + getX() + " " + getY() + " " + team + " " + GetComponent<HypnoScript>().getDirection() + "\n" ;
        exp = exp + GetComponent<HypnoScript>().getCommandString() + "\n";
        return exp;
    }
}