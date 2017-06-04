using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameMaster : MonoBehaviour {

    public GameObject fieldObject;
    private static List<Character> characters;
    private static Field myField;
    public static int fps;
    public static bool started;
    private static int currentPlayer;
    public static bool loop;

    // Use this for initialization
    void Start() {
        GameMaster.fps = 10;
        myField = fieldObject.GetComponent<Field>();
        characters = new List<Character>();
        started = false;
        loop = true;
    }

    // Update is called once per frame
    void Update() {

    }

    public static void addCharacter(Character newCharacter) {
        characters.Add(newCharacter);
    }

    public static GameObject objectAtSpace(int x, int y) {
        int i;
        for (i = 0; i < characters.Count; i++) {
            if (characters[i].getX() == x && characters[i].getY() == y) {
                return characters[i].gameObject;
            }
        }
        return null;
    }

    public static bool withinField(int x, int y) {
        return (x <= endColumn() && x >= startColumn() && y <= startRow() && y >= endRow());
    }

    public static bool playerAtSpace(int x, int y) {
        for (int i = 0; i < characters.Count; i++) {
            if (characters[i].getX() == x && characters[i].getY() == y && characters[i].alive) {
                return true;
            }
        }
        return false;
    }

    public static bool spaceIsClear(int x, int y) {
        int i;
        if (x > endColumn() || x < startColumn() || y > startRow() || y < endRow()) {
            return false;
        }
        for (i = 0; i < characters.Count; i++) {
            if(characters[i].getX() == x && characters[i].getY() == y && characters[i].alive) {
                return false;
            }
        }
        return true;
    }

    public static void attackSpace(int x, int y) {
        int i;
        for (i = 0; i < characters.Count; i++) {
            if(characters[i].getX() == x && characters[i].getY() == y) {
                if (characters[i].alive) {
                    characters[i].die();
                }else {
                    //Destroy(characters[i].gameObject);
                    //characters.RemoveAt(i);
                    //i--;
                }
            }
        }
    }

    public static void resetGame() {
        int i;
        for (i = 0; i < characters.Count; i++) {
            characters[i].resetCharacter();
        }
        started = false;
        currentPlayer = characters.Count;
    }

    public static int endRow() {
        return -myField.rows / 2;
    }

    public static int startRow() {
        return myField.rows - myField.rows / 2 - 1;
    }

    public static int startColumn() {
        return -myField.columns / 2;
    }

    public static int endColumn() {
        return myField.columns - myField.columns / 2 - 1;
    }

    public static void startGame() {
        if (!started) {
            started = true;
            int i;
            for (i = 0; i < characters.Count; i++) {
                characters[i].gameObject.GetComponent<CharacterAnimation>().setAnimRunning(true);
            }
            currentPlayer = characters.Count;
            startCycle();
        }
    }

    public static void startCycle() {
        if(currentPlayer >= characters.Count && started) {
            currentPlayer = 0;
            nextPlayer();
        }
    }

    public static void nextPlayer() {
        if(loop && currentPlayer >= characters.Count) {
            currentPlayer = 0;
        }
        if(currentPlayer < characters.Count) {
            currentPlayer++;
            characters[currentPlayer-1].gameObject.GetComponent<CharacterAnimation>().setIdle(false);
        }
    }

    public static List<GameObject> selectAllPlayers() {
        List<GameObject> allPlayers = new List<GameObject>();
        for(int i = 0; i < characters.Count; i++) {
            characters[i].Deselect();
            characters[i].Select();
            allPlayers.Add(characters[i].gameObject);
        }
        return allPlayers;
    }

    public static void deleteCharacters() {
        for(int i = characters.Count-1; i >= 0; i--) {
            if (characters[i].isSelected()) {
                Destroy(characters[i].gameObject);
                characters.RemoveAt(i);
            }
        }
    }
}
