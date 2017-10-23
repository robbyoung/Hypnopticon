using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HypnoScript : MonoBehaviour {

    private List<List<string>> actions;
    private List<int> nest;
    private List<int> indices;
    private string direction;
    private string savedDirection;
    private Character character;
    private CharacterAnimation charAnim;
    private Character target;
    private bool broken;
    private bool weaponLoaded; //For archers
    private int loopCheck;
    public string[] moveset;
    private string characterCommand;
    private bool safeLeft, safeRight, safeFront, safeBack;

	// Use this for initialization
	void Start () {
        actions = new List<List<string>>();
        actions.Add(new List<string>());
        actions[0].Add("main:");
        indices = new List<int>();
        direction = "right";
        savedDirection = "right";
        character = GetComponent<Character>();
        charAnim = GetComponent<CharacterAnimation>();
        nest = new List<int>();
        target = null;
        broken = false;
        nest.Add(0);
        indices.Add(1);
        weaponLoaded = true;
        if (character.script != null) {
            addToScript(character.script);
        }
    }

    public void addToScript(string newAction) {
        if ( newAction == null || newAction.Equals("")) return;
        int subToAdd = 0;
        string[] newActions = newAction.Split(null);
        for (int i = 0; i < newActions.Length; i++) {
            if (!newActions[i].Equals("")) {
                if (newActions[i].Contains(":")) {
                    subToAdd = getSubNum(newActions[i]);
                    if (subToAdd == -1) {
                        actions.Add(new List<string>());
                        actions[actions.Count - 1].Add(newActions[i]);
                        subToAdd = actions.Count - 1;
                    }
                }else {
                    actions[subToAdd].Add(newActions[i]);
                }
            }
        }
    }

    private int getSubNum(string keyword) {
        for(int i = 0; i < actions.Count; i++) {
            if (keyword.Equals(actions[i][0])) {
                return i;
            }
        }
        return -1;
    }

    public void removeFromScript() {
        if (actions.Count >= 1) {
            actions.RemoveAt(actions.Count - 1);
        }
    }

    public void deleteScript() {
        actions = new List<List<string>>();
        actions.Add(new List<string>());
        actions[0].Add("main:");
    }
	
    //Wrapper class for nextAction to avoid infinite loops.
    public string getNextAction() {
        loopCheck = 0;
        characterCommand = "";
        return nextAction();
    }

    // This is where the next action is figured out from the script.
	private string nextAction() {
        loopCheck++;
        if(loopCheck > 100) {
            broken = true;
        }
        if(target != null && !target.alive) {
            target = null;
        }
        if(actions[0].Count == 1 || broken) {
            return "malfunction";
        }

        //Prevent stack overflows by limiting the nesting.
        if (nest.Count >= 1000 || nest.Count == 0) {
            broken = true;
            return "malfunction";
        }

        /*if(nest.Count == 0) {
            nest.Add(0);
            indices.Add(1);
        }*/

        if(indices[indices.Count -1] >= actions[nest[nest.Count - 1]].Count) {
            broken = true;
            return nextAction();
        }else {
            string command = actions[nest[nest.Count - 1]][indices[indices.Count -1]];
            if(command.ToUpper().Equals("RTN")) {
                nest.RemoveAt(nest.Count - 1);
                indices.RemoveAt(indices.Count - 1);
                return nextAction();
            }
            if (getSubNum(command + ":") > -1) {
                indices[indices.Count - 1]++;
                if (indices[indices.Count - 1] >= actions[nest[nest.Count - 1]].Count) {
                    nest.RemoveAt(nest.Count - 1);
                    indices.RemoveAt(indices.Count - 1);
                }
                nest.Add(getSubNum(command + ":"));
                indices.Add(1);
                return nextAction();
            }else if (!inMoveset(command)) {
                return "malfunction";
            }else if (isCondition(command)) {
                if (checkCondition(command)) {
                    indices[indices.Count - 1]++;
                    return nextAction();
                }else {
                    indices[indices.Count - 1] += 2;
                    return nextAction();
                }
            }else {
                string translated = translateCommand(command);
                indices[indices.Count - 1]++;
                return translated;
            }
        }
    }

    public string getCommands() {
        string commands = "";
        int i;
        for(i = 0; i < actions.Count; i++) {
            commands = commands + actions[i][0] + "\n";
            for (int j = 1; j < actions[i].Count; j++) {
                commands = commands + "  " + actions[i][j] + "\n";
            }
            commands = commands + "\n";
        }
        return commands;
    }

    public string getCommandString() {
        string commands = "";
        int i;
        for (i = 0; i < actions.Count; i++) {
            commands = commands + actions[i][0];
            for (int j = 1; j < actions[i].Count; j++) {
                commands = commands + " " + actions[i][j];
            }
            commands = commands + " ";
        }
        return commands;
    }

    public void resetIndex() {
        for(int i = 0; i < indices.Count; i++) {
            indices[i] = 1;
        }
        nest = new List<int>();
        nest.Add(0);
        indices.Add(1);
        direction = savedDirection;
        broken = false;
        weaponLoaded = true;
    }

    //Translate a command from what the player types to a state keyword.
    //Helpful for when the language is changed regarding syntax - this is the only place you'll have to change it.
    private string translateCommand(string command) {
        command = command.ToUpper();

        if (command.Equals("RDM")) {
            float r = GameMaster.random();
            if(r < 0.33) {
                command = "MOV";
            }else if(r < 0.66) {
                command = "LFT";
            }else{
                command = "RGT";
            }
        }

        if (command.Equals("RDA")) {
            float r = GameMaster.random();
            if (r < 0.143) {
                command = "MOV";
            }
            else if (r < 0.285) {
                command = "LFT";
            }
            else if (r < 0.428) {
                command = "RGT";
            }
            else if (r < 0.571) {
                command = "MVL";
            }
            else if (r < 0.714) {
                command = "MVR";
            }
            else if (r < 0.857) {
                command = "MVB";
            }
            else {
                command = "FLP";
            }
        }

            //take a step forward.
        if (command.Equals("MOV")) {
            if (direction.Equals("right")) {
                return "moveRight";
            } else if (direction.Equals("left")) {
                return "moveLeft";
            } else if (direction.Equals("up")) {
                return "moveUp";
            } else {
                return "moveDown";
            }
        }
        //attack
        else if (command.Equals("ATK")) {
            if (direction.Equals("right")) {
                return "attackRight";
            }
            else if (direction.Equals("left")) {
                return "attackLeft";
            }
            else if (direction.Equals("up")) {
                return "attackUp";
            }
            else {
                return "attackDown";
            }
        }
        //turn left
        else if (command.Equals("LFT")) {
            if (direction.Equals("right")) {
                direction = "up";
                return "turnRightUp";
            }
            else if (direction.Equals("left")) {
                direction = "down";
                return "turnLeftDown";
            }
            else if (direction.Equals("up")) {
                direction = "left";
                return "turnUpLeft";
            }
            else {
                direction = "right";
                return "turnDownRight";
            }
        }
        //turn right
        else if (command.Equals("RGT")) {
            if (direction.Equals("right")) {
                direction = "down";
                return "turnRightDown";
            }
            else if (direction.Equals("left")) {
                direction = "up";
                return "turnLeftUp";
            }
            else if (direction.Equals("up")) {
                direction = "right";
                return "turnUpRight";
            }
            else {
                direction = "left";
                return "turnDownLeft";
            }
        }
        //sit still
        else if (command.Equals("IDL")) {
            if (direction.Equals("right")) {
                return "idleRight";
            }
            else if (direction.Equals("left")) {
                return "idleLeft";
            }
            else if (direction.Equals("up")) {
                return "idleUp";
            }
            else {
                return "idleDown";
            }
        }
        else if (command.Equals("SEE")) {
            target = null;
            if (!checkCondition("ESP")) {
                return "malfunction";
            }else {
                int [] coords = getFront();
                while (GameMaster.withinField(coords[0], coords[1]) && target == null && !GameMaster.allyAtSpace(coords[0], coords[1], getTeam()) && !GameMaster.obstacleAtSpace(coords[0], coords[1])) {
                    if (GameMaster.enemyAtSpace(coords[0], coords[1], getTeam())) {
                            target = GameMaster.objectAtSpace(coords[0], coords[1]).GetComponent<Character>();
                    }else {
                        coords = getFront(coords);
                    }
                }
                GameMaster.animate("spotted", transform.position.x, transform.position.y, transform.position.z + 0.01f);
            }
            return translateCommand("IDL");
        }

        else if (command.Equals("FLP")) {
            translateCommand("LFT");
            return translateCommand("LFT");
        }

        else if (command.Equals("ATL")) {
            if (direction.Equals("right")) {
                characterCommand = "attackUp";
                return "attackRight";
            }
            else if (direction.Equals("left")) {
                characterCommand = "attackDown";
                return "attackLeft";
            }
            else if (direction.Equals("up")) {
                characterCommand = "attackLeft";
                return "attackUp";
            }
            else {
                characterCommand = "attackRight";
                return "attackDown";
            }
        }

        else if (command.Equals("ATR")) {
            if (direction.Equals("right")) {
                characterCommand = "attackDown";
                return "attackRight";
            }
            else if (direction.Equals("left")) {
                characterCommand = "attackUp";
                return "attackLeft";
            }
            else if (direction.Equals("up")) {
                characterCommand = "attackRight";
                return "attackUp";
            }
            else {
                characterCommand = "attackLeft";
                return "attackDown";
            }
        }

        else if (command.Equals("MVB")) {
            if (direction.Equals("right")) {
                characterCommand = "moveLeft";
                return "moveRight";
            }
            else if (direction.Equals("left")) {
                characterCommand = "moveRight";
                return "moveLeft";
            }
            else if (direction.Equals("up")) {
                characterCommand = "moveDown";
                return "moveUp";
            }
            else {
                characterCommand = "moveUp";
                return "moveDown";
            }
        }

        else if (command.Equals("MVL")) {
            if (direction.Equals("right")) {
                characterCommand = "moveUp";
                return "moveRight";
            }
            else if (direction.Equals("left")) {
                characterCommand = "moveDown";
                return "moveLeft";
            }
            else if (direction.Equals("up")) {
                characterCommand = "moveLeft";
                return "moveUp";
            }
            else {
                characterCommand = "moveRight";
                return "moveDown";
            }
        }

        else if (command.Equals("MVR")) {
            if (direction.Equals("right")) {
                characterCommand = "moveDown";
                return "moveRight";
            }
            else if (direction.Equals("left")) {
                characterCommand = "moveUp";
                return "moveLeft";
            }
            else if (direction.Equals("up")) {
                characterCommand = "moveRight";
                return "moveUp";
            }
            else {
                characterCommand = "moveLeft";
                return "moveDown";
            }
        }

        //Ranged attack
        else if (command.Equals("RAT")) {
            if (weaponLoaded) {
                
                int[] coords = getFront();
                while (GameMaster.withinField(coords[0], coords[1]) && weaponLoaded) {
                    if (GameMaster.enemyAtSpace(coords[0], coords[1], getTeam()) || GameMaster.allyAtSpace(coords[0], coords[1], getTeam())) {
                        GameMaster.attackSpace(GetComponent<Character>().attack, coords[0], coords[1]);
                        weaponLoaded = false;
                    }
                    else {
                        coords = getFront(coords);
                    }
                }
                weaponLoaded = false; //Keep this if you want the archer to shoot even if they don't shoot a character.

                if (direction.Equals("right")) {
                    characterCommand = "rangedRight";
                    return "attackRight";
                }
                else if (direction.Equals("left")) {
                    characterCommand = "rangedLeft";
                    return "attackLeft";
                }
                else if (direction.Equals("up")) {
                    characterCommand = "rangedUp";
                    return "attackUp";
                }
                else {
                    characterCommand = "rangedRight";
                    return "attackRight";
                }
            }
            else {
                return "malfunction";
            }
        }

        else if (command.Equals("RLD")) {
            weaponLoaded = true;
            return translateCommand("IDL");
        }

        else {
            return "malfunction";
        }
    }

    //Checks if it's worth putting a string through the "checkCondition" method
    public bool isCondition(string cond) {
        return cond.Equals("BLL") || cond.Equals("BLR") || cond.Equals("BLF") ||
            cond.Equals("ENL") || cond.Equals("ENR") || cond.Equals("ENF") || cond.Equals("ESP") ||
            cond.Equals("TAF") || cond.Equals("TAL") || cond.Equals("TAR") || cond.Equals("TAB");
    }

    //Used to check if the given condition is met, creating a form of control flow
    public bool checkCondition(string cond) {
        int[] coords;
        if (cond.Equals("BLL")) {
            coords = getLeft();
            return !GameMaster.withinField(coords[0], coords[1]);
        }
        else if (cond.Equals("BLR")) {
            coords = getRight();
            return !GameMaster.withinField(coords[0], coords[1]);
        }
        else if (cond.Equals("BLF")) {
            coords = getFront();
            return !GameMaster.withinField(coords[0], coords[1]);
        }
        else if (cond.Equals("ENL")) {
            coords = getLeft();
            return GameMaster.enemyAtSpace(coords[0], coords[1], getTeam());
        }
        else if (cond.Equals("ENR")) {
            coords = getRight();
            return GameMaster.enemyAtSpace(coords[0], coords[1], getTeam());
        }
        else if (cond.Equals("ENF")) {
            coords = getFront();
            return GameMaster.enemyAtSpace(coords[0], coords[1], getTeam());
        }
        else if (cond.Equals("ESP")) {
            //Returns if there is another character in the direction that a character is facing.
            coords = getFront();
            while (true) {
                if (GameMaster.withinField(coords[0], coords[1])) {
                    if (GameMaster.enemyAtSpace(coords[0], coords[1], getTeam())) {
                        return true;
                    }
                    else if (GameMaster.allyAtSpace(coords[0], coords[1], getTeam())) {
                        return false;
                    }
                    else {
                        coords = getFront(coords);
                    }
                }
                else {
                    return false;
                }
            }
        }
        else if (cond.Equals("TAR")) {
            if(target == null) {
                return false;
            }
            return ((direction == "up" && character.getX() < target.getX()) ||
                (direction == "down" && character.getX() > target.getX()) ||
                (direction == "left" && character.getY() < target.getY()) ||
                (direction == "right" && character.getY() > target.getY()));
        }
        else if (cond.Equals("TAL")) {
            if (target == null) {
                return false;
            }
            return ((direction == "up" && character.getX() > target.getX()) ||
                (direction == "down" && character.getX() < target.getX()) ||
                (direction == "left" && character.getY() > target.getY()) ||
                (direction == "right" && character.getY() < target.getY()));
        }
        else if (cond.Equals("TAF")) {
            if (target == null) {
                return false;
            }
            return ((direction == "left" && character.getX() > target.getX()) ||
                (direction == "right" && character.getX() < target.getX()) ||
                (direction == "up" && character.getY() < target.getY()) ||
                (direction == "down" && character.getY() > target.getY()));
        }
        else if (cond.Equals("TAB")) {
            if (target == null) {
                return false;
            }
            return ((direction == "left" && character.getX() > target.getX()) ||
                (direction == "right" && character.getX() < target.getX()) ||
                (direction == "up" && character.getY() < target.getY()) ||
                (direction == "down" && character.getY() < target.getY()));
        }
        else {
            return false;
        }
    }

    public void setDirection(string d) {
        direction = d;
        if (!GameMaster.started) {
            savedDirection = direction;
        }
    }

    public string getDirection() {
        return direction;
    }

    public int[] getLeft() {
        if (direction.Equals("up")) {
            return new int[2] { character.getX() - 1, character.getY() };
        }
        else if (direction.Equals("left")) {
            return new int[2] { character.getX(), character.getY() - 1 };
        }
        else if (direction.Equals("right")) {
            return new int[2] { character.getX(), character.getY() + 1 };
        }
        else { //down
            return new int[2] { character.getX() + 1, character.getY() };
        }
    }

    public int[] getRight() {
        if (direction.Equals("up")) {
            return new int[2] { character.getX() + 1, character.getY() };
        }
        else if (direction.Equals("left")) {
            return new int[2] { character.getX(), character.getY() + 1 };
        }
        else if (direction.Equals("right")) {
            return new int[2] { character.getX(), character.getY() - 1 };
        }
        else { //down
            return new int[2] { character.getX() - 1, character.getY() };
        }
    }

    public int[] getFront() {
        if (direction.Equals("up")) {
            return new int[2] { character.getX(), character.getY() + 1};
        }
        else if (direction.Equals("left")) {
            return new int[2] { character.getX() - 1, character.getY()};
        }
        else if (direction.Equals("right")) {
            return new int[2] { character.getX() + 1, character.getY()};
        }
        else{ //down
            return new int[2] { character.getX(), character.getY() - 1 };
        }
    }

    //Like the getFront method, but applies the coordinate change to a set of integer coordinates.
    public int[] getFront(int[] coords) {
        if (direction.Equals("up")) {
            return new int[2] { coords[0], coords[1] + 1 };
        }
        else if (direction.Equals("left")) {
            return new int[2] { coords[0] - 1, coords[1] };
        }
        else if (direction.Equals("right")) {
            return new int[2] { coords[0] + 1, coords[1] };
        }
        else { //down
            return new int[2] { coords[0], coords[1] - 1 };
        }
    }

    public void rotateRight() {
        if (direction.Equals("up")) {
            direction = "right";
        }else if (direction.Equals("right")) {
            direction = "down";
        }else if (direction.Equals("down")) {
            direction = "left";
        }else {
            direction = "up";
        }
        charAnim.setDirection(direction);
    }

    public void rotateLeft() {
        if (direction.Equals("up")) {
            direction = "left";
        }
        else if (direction.Equals("left")) {
            direction = "down";
        }
        else if (direction.Equals("down")) {
            direction = "right";
        }
        else {
            direction = "up";
        }
        charAnim.setDirection(direction);
    }

    //Turn the saved direction left.
    public void setLeft() {
        rotateLeft();
        savedDirection = direction;
    }

    //Turn the saved direction right.
    public void setRight() {
        rotateRight();
        savedDirection = direction;
    }

    public int getTeam() {
        return GetComponent<Character>().team;
    }

    //Deals with 'interrupts' such as when the character gets attacked and 'IHT' is set.
    public void interrupt(string keyword) {
        if(getSubNum(keyword+":") > -1) {
            nest.Add(getSubNum(keyword + ":"));
            indices.Add(1);
        }
    }

    //Check surroundings should be called at the end of a turn.
    public void postTurn() {
        //Debug.Log(character.getX() + "/" + character.getY() + " post-turn check.");
        safeFront = !checkCondition("ENF");
        safeLeft = !checkCondition("ENL");
        safeRight = !checkCondition("ENR");
        //safeBack = !checkCondition("ENB");
    }

    //This is called at the start of the next turn to respond to appeared enemies.
    public void preTurn() {
        //Debug.Log(character.getX() + "/" + character.getY() + " pre-turn check.");
        if (checkCondition("ENF") && safeFront) {
            interrupt("EAF");
        }
        else if (checkCondition("ENL") && safeLeft) {
            interrupt("EAL");
        }
        else if (checkCondition("ENR") && safeRight) {
            interrupt("EAR");
        }/*
        if (checkCondition("ENF") && safeFront) {
            interrupt("EAF");
        }*/
    }

    public bool inMoveset(string move) {
        move = move.ToUpper();
        for(int i = 0; i < moveset.Length; i++) {
            if (move.Equals(moveset[i])) {
                return true;
            }
        }
        return false;
    }

    public string getCharacterCommand() {
        return characterCommand;
    }
}
