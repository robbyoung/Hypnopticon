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
    public static int currentObstacleType;
    public static Transform[] prefabs;
    public static Transform[] obstaclePrefabs;
    private static string[] loading;
    private static int seed;
    public static bool win;
    public static bool obstacleMenu;
    private static List<Obstacle> obstacles;

    // Use this for initialization
    void Start() {
        obstacleMenu = false;
        win = false;
        GameMaster.fps = 16;
        myField = fieldObject.GetComponent<Field>();
        obstacles = new List<Obstacle>();
        characters = new List<Character>();
        started = false;
        loop = true;
        BloodAnimation = BloodPrefab;
        SpottedAnimation = SpottedPrefab;
        currentType = 0;
        prefabs = myField.characterPrefabs;
        obstaclePrefabs = myField.obstaclePrefabs;
        loading = null;
        seed = (int)long.Parse(System.DateTime.Now.ToString("yyyyMMddHHmmss"));
        if (Hypnopticon.storyMode) {
            if (Hypnopticon.nextBattle.Equals("Random")) {
                randomScenario();
            }
            loadScenario(Hypnopticon.nextBattle);
        }
    }

    // Update is called once per frame
    void LateUpdate() {
        if(loading != null) {
            int obstacleCount = 0;
            for (int i = 0; i < loading.Length - 2; i += 3) {
                //This will be ok as long as obstacles are all listed after the characters.
                if (loading[i].Equals("o")) {
                    obstacles[obstacleCount].import(loading[i + 2]);
                    obstacleCount++;
                }
                else {
                    characters[i / 3].import(loading[i + 1]);
                    characters[i / 3].GetComponent<HypnoScript>().addToScript(loading[i + 2]);
                }
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

    public static void addObstacle(Obstacle newObstacle) {
        obstacles.Add(newObstacle);
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

    public static bool obstacleAtSpace(int x, int y) {
        for (int i = 0; i < obstacles.Count; i++) {
            if (obstacles[i].getX() == x && obstacles[i].getY() == y) {
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
        for (i = 0; i < obstacles.Count; i++) {
            if (obstacles[i].getX() == x && obstacles[i].getY() == y) {
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
                if (Hypnopticon.storyMode && characters[i].team == 1) {
                    Hypnopticon.unitCount[characters[i].type]++;
                }else if (Hypnopticon.storyMode) {
                    return;
                }
                characters[i].GetComponent<CharacterAnimation>().deleteBars();
                characters[i].Deselect();
                Destroy(characters[i].gameObject);
                characters.RemoveAt(i);
            }
        }
    }

    public static void deleteAllCharacters() {
        for (int i = characters.Count - 1; i >= 0; i--) {
            if (Hypnopticon.storyMode && characters[i].team == 1) {
                Hypnopticon.unitCount[characters[i].type]++;
            }
            characters[i].GetComponent<CharacterAnimation>().deleteBars();
            characters[i].Deselect();
            Destroy(characters[i].gameObject);
            characters.RemoveAt(i);
        }
        deleteAllObstacles();
    }

    public static void deleteAllObstacles() {
        for (int i = obstacles.Count - 1; i >= 0; i--) {
            Destroy(obstacles[i].gameObject);
            obstacles.RemoveAt(i);
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
        for(int i = 0; i < obstacles.Count; i++) {
            temp = temp + obstacles[i].export();
        }
        temp = temp + seed;
        return temp;
    }

    public static void importScenario(string scenario) {
        resetGame();
        deleteAllCharacters();
        deleteAllObstacles();
        string[] info = scenario.Split('\n');
        int i;
        for(i = 0; i < info.Length-2; i += 3) {
            if (info[i].Equals("o")) {
                Transform temp = Instantiate(obstaclePrefabs[int.Parse(info[i+1])]);
                obstacles.Add(temp.GetComponent<Obstacle>());
            }
            else {
                Transform temp = Instantiate(prefabs[int.Parse(info[i])]);
                characters.Add(temp.GetComponent<Character>());
            }
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

    public static void randomScenario() {
        List<string> scripts = new List<string>();
        int[,] grid = new int[9, 9];
        scripts.Add("main: ENF attack TAF seekFront TAB seekBack TAL seekLeft TAR seekRight ESP remember RDM main remember: SEE main attack: ATK main seekFront: MOV main seekLeft: LFT main seekRight: RGT main seekBack: FLP main IHT: ENF attack MOV RTN");
        scripts.Add("main: ENF attack ENL attack ENR attack TAF seekFront TAB seekBack TAL seekLeft TAR seekRight ESP remember RDA main attack: ENF ATK ENL ATL ENR ATR main remember: SEE main seekFront: MOV main seekBack: FLP main seekLeft: MVL main seekRight: MVR main IHT: ENF ENF RTN ENR RTN ENL RTN MOV RTN");
        scripts.Add("main: ESP attack BLR moveLeft MVR main attack: RAT RLD main moveLeft: BLL rand MVL main rand: RDM main");
        string fileName = "Assets/Scenarios/Random.txt";
        if (File.Exists(fileName)) {
            File.Delete(fileName);
        }
        var sr = File.CreateText(fileName);
        int quantity = (int)(random() * 3) + 2;
        for (int i = 0; i < quantity; i++) {
            int type = (int)(random() * 3);
            int x = (int)(random() * 9 - 4);
            int y = (int)(random() * 3 + 2);
            if (grid[x + 4, y + 4] == 0) {
                sr.WriteLine(type + "\n" + x + " " + y + " 2 down\n" + scripts[type]);
                grid[x + 4, y + 4]++;
            }else {
                i--;
            }
        }
        quantity = (int)(random() * 20) + 2;
        for (int i = 0; i < quantity; i++) {
            int x = (int)(random() * 9) - 4;
            int y = (int)(random() * 9) - 4;
            if(grid[x + 4, y + 4] == 0) {
                int type = (int)(random() * 2);
                sr.WriteLine("o\n" + type + "\n" + x + " " + y);
                grid[x + 4, y + 4]++;
            }else {
                i--;
            }
        }
        sr.WriteLine((int)long.Parse(System.DateTime.Now.ToString("yyyyMMddHHmmss")) + "");
        sr.Close();
    }

    public static void newRecruits() {
        for(int i = 0; i < characters.Count; i++) {
            if(characters[i].team != 1 || (characters[i].team == 1 && characters[i].alive)) {
                Hypnopticon.unitCount[characters[i].type]++;
            }
        }
    }
}