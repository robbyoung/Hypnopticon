using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class GameMaster : MonoBehaviour {

    public GameObject fieldObject;
    private static List<Character> characters;
    private static Field myField;
    public static int fps;
    public static bool started;
    private static int currentPlayer;
    public static bool loop;
    public Transform BloodPrefab;
    public Transform SpottedPrefab;
    public static Transform BloodAnimation;
    public static Transform SpottedAnimation;
    public static int currentType;
    public static List<Transform> prefabs;
    public List<Transform> characterPrefabs;
    private static string[] loading;
    private static int seed;
    public static bool win;

    // Use this for initialization
    void Start() {
        win = false;
        GameMaster.fps = 16;
        myField = fieldObject.GetComponent<Field>();
        characters = new List<Character>();
        started = false;
        loop = true;
        BloodAnimation = BloodPrefab;
        SpottedAnimation = SpottedPrefab;
        currentType = 0;
        prefabs = characterPrefabs;
        loading = null;
        seed = (int)long.Parse(System.DateTime.Now.ToString("yyyyMMddHHmmss"));
        if (Hypnopticon.storyMode) {
            loadScenario(Hypnopticon.nextBattle);
        }
    }

    // Update is called once per frame
    void LateUpdate() {
        if(loading != null) {
            for (int i = 0; i < loading.Length - 2; i += 3) {
                characters[i/3].import(loading[i + 1]);
                characters[i/3].GetComponent<HypnoScript>().addToScript(loading[i + 2]);
            }
            loading = null;
        }
    }

    public static int characterCount() {
        return characters.Count;
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

    public static bool enemyAtSpace(int x, int y, int team) {
        for (int i = 0; i < characters.Count; i++) {
            if (characters[i].getX() == x && characters[i].getY() == y && characters[i].alive && characters[i].team != team) {
                return true;
            }
        }
        return false;
    }

    public static bool allyAtSpace(int x, int y, int team) {
        for (int i = 0; i < characters.Count; i++) {
            if (characters[i].getX() == x && characters[i].getY() == y && characters[i].alive && characters[i].team == team) {
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

    public static void attackSpace(int attack, int x, int y) {
        int i;
        for (i = 0; i < characters.Count; i++) {
            if(characters[i].getX() == x && characters[i].getY() == y) {
                
                if (characters[i].alive) {
                    characters[i].die(attack, true);
                    if (characters[i].alive) {
                        characters[i].GetComponent<HypnoScript>().interrupt("IHT");
                    }
                }
                else {
                    //Destroy(characters[i].gameObject);
                    //characters.RemoveAt(i);
                    //i--;
                }
            }
        }
    }

    public static void resetGame() {
        win = false;
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
            UnityEngine.Random.InitState(seed);
            started = true;
            int i;
            //sort characters based on speed
            List<Character> sorted = new List<Character>();
            int max;
            while(characters.Count > 0) {
                max = 0;
                for(i = 0; i < characters.Count; i++) {
                    if (characters[i].getSpeed() < characters[max].getSpeed()) {
                        max = i;
                    }
                }
                sorted.Add(characters[max]);
                characters.RemoveAt(max);
            }
            characters = sorted;

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
        bool allDead = true;
        win = true;
        for(int i = 0; i < characters.Count; i++) {
            if (characters[i].alive) {
                allDead = false;
                if(characters[i].team != 1) {
                    win = false;
                }
            }
        }
        if (allDead) return;
        if(loop && currentPlayer >= characters.Count) {
            currentPlayer = 0;
        }
        if(currentPlayer < characters.Count) {
            currentPlayer++;
            if (characters[currentPlayer - 1].willMove()) {
                characters[currentPlayer - 1].gameObject.GetComponent<CharacterAnimation>().setIdle(false);
            }else {
                nextPlayer();
            }
        }
    }

    public static List<GameObject> selectAllPlayers() {
        List<GameObject> allPlayers = new List<GameObject>();
        for(int i = 0; i < characters.Count; i++) {
            if (characters[i].alive) {
                characters[i].Select();
                allPlayers.Add(characters[i].gameObject);
            }
        }
        return allPlayers;
    }

    public static List<GameObject> selectTeam(int t) {
        List<GameObject> teamPlayers = new List<GameObject>();
        for (int i = 0; i < characters.Count; i++) {
            if (characters[i].alive && characters[i].team == t) {
                characters[i].Select();
                teamPlayers.Add(characters[i].gameObject);
            }
        }
        return teamPlayers;
    }

    public static void deleteCharacters() {
        for(int i = characters.Count-1; i >= 0; i--) {
            if (characters[i].isSelected()) {
                characters[i].GetComponent<CharacterAnimation>().deleteBars();
                characters[i].Deselect();
                Destroy(characters[i].gameObject);
                characters.RemoveAt(i);
            }
        }
    }

    public static void deleteAllCharacters() {
        for (int i = characters.Count - 1; i >= 0; i--) {
            characters[i].GetComponent<CharacterAnimation>().deleteBars();
            characters[i].Deselect();
            Destroy(characters[i].gameObject);
            characters.RemoveAt(i);
        }
    }

    public static void animate(string type, float x, float y, float z) {
        Transform temp;
        if (type.Equals("blood")) {
            temp = Instantiate(BloodAnimation);
            temp.position = new Vector3(x, y, z);
        }
        else if (type.Equals("spotted")) {
            temp = Instantiate(SpottedAnimation);
            temp.position = new Vector3(x, y, z);
        }
    }

    public static int numPlayers() {
        return characters.Count;
    }

    public static string exportScenario() {
        string temp = "";
        for(int i = 0; i < characters.Count; i++) {
            temp = temp + characters[i].export();
        }
        temp = temp + seed;
        return temp;
    }

    public static void importScenario(string scenario) {
        resetGame();
        deleteAllCharacters();
        string[] info = scenario.Split('\n');
        int i;
        for(i = 0; i < info.Length-2; i += 3) {
            Transform temp = Instantiate(prefabs[int.Parse(info[i])]);
            characters.Add(temp.GetComponent<Character>());
        }
        seed = int.Parse(info[i]);
        loading = info;
    }

    public static float random() {
        return UnityEngine.Random.value;
    }

    public static void loadScenario(string filename) {
        string fileName = "Assets/Scenarios/" + filename + ".txt";
        if (File.Exists(fileName)) {
            StreamReader reader = new StreamReader(fileName);
            GameMaster.importScenario(reader.ReadToEnd());
            reader.Close();
            Debug.Log("Loaded " + fileName);
        }
        else {
            Debug.Log(fileName + " doesn't exist.");
        }
    }

    public static void newRecruits() {
        for(int i = 0; i < characters.Count; i++) {
            if(characters[i].team != 1 || (characters[i].team == 1 && characters[i].alive)) {
                Hypnopticon.unitCount[characters[i].type]++;
            }
        }
    }
}