using UnityEngine;
using System.Collections;

public class CharacterAnimation : MonoBehaviour {

    private Sprite[] currentAnim;
    public Sprite[] moveRight;
    public Sprite[] moveLeft;
    public Sprite[] moveUp;
    public Sprite[] moveDown;
    public Sprite[] idleRight;
    public Sprite[] idleLeft;
    public Sprite[] idleUp;
    public Sprite[] idleDown;
    public Sprite[] attackDown;
    public Sprite[] attackRight;
    public Sprite[] attackLeft;
    public Sprite[] attackUp;
    public Sprite[] deathRight;
    public Sprite[] deathLeft;
    public Sprite[] deathUp;
    public Sprite[] deathDown;
    public Sprite[] turnUpLeft;
    public Sprite[] turnLeftDown;
    public Sprite[] turnDownRight;
    public Sprite[] turnRightUp;
    public Sprite[] turnUpRight;
    public Sprite[] turnRightDown;
    public Sprite[] turnDownLeft;
    public Sprite[] turnLeftUp;
    public Sprite[] malfunction;
    public Transform healthBarPrefab;
    public Transform staminaBarPrefab;
    private Transform healthBar;
    private Transform staminaBar;
    private string state;
    private HypnoScript hyp;
    private Character character;
    private float framesPerSecond;
    SpriteRenderer animRenderer;
    private float timeAtAnimStart;
    private bool animRunning = true;
    private int lastIndex;
    private bool idle;
    private bool moving;

    void Start() {
        timeAtAnimStart = -69;
        animRenderer = GetComponent<Renderer>() as SpriteRenderer;
        state = "idleRight";
        currentAnim = idleRight;
        framesPerSecond = GameMaster.fps;
        character = gameObject.GetComponent<Character>();
        hyp = gameObject.GetComponent<HypnoScript>();
        lastIndex = -1;

        //Remove these once animations have been added.
        moveLeft = idleLeft;
        moveRight = idleRight;
        moveUp = idleUp;
        moveDown = idleDown;
        //idleLeft = idleRight;
        //idleUp = idleRight;
        //idleDown = idleRight;
        //attackLeft = attackRight;
        attackUp = idleUp;
        attackDown = idleDown;
        attackLeft = idleLeft;
        attackRight = idleRight;
        //deathUp = deathRight;
        //deathDown = deathRight;
        turnUpRight = idleRight;
        turnRightDown = idleDown;
        turnDownLeft = idleLeft;
        turnLeftUp = idleUp;
        turnUpLeft = idleLeft;
        turnLeftDown = idleDown;
        turnDownRight = idleRight;
        turnRightUp = idleUp;
        idle = true;
        moving = false;
        animRunning = false;
        healthBar = Instantiate(healthBarPrefab);
        staminaBar = Instantiate(staminaBarPrefab);
        healthBar.transform.position = gameObject.transform.position;
        staminaBar.transform.position = gameObject.transform.position;
    }

    void Update() {
        framesPerSecond = GameMaster.fps;
        if (animRunning) {
            float timeSinceAnimStart = Time.timeSinceLevelLoad - timeAtAnimStart;
            int frameIndex = (int)(timeSinceAnimStart * framesPerSecond);
            if (frameIndex < currentAnim.Length && timeAtAnimStart != -69) {
                if (moving && character.alive) {
                    GameMaster.nextPlayer();
                    moving = false;
                }
                if(framesPerSecond <= 200) {
                    animRenderer.sprite = currentAnim[frameIndex];
                }
            }else {
                character.performAction(10);
                if(!idle && character.alive) {
                    setState();
                }
                if (!idle && character.alive) {
                    //setState();
                    if (state.Equals("idleRight")) {
                        currentAnim = idleRight;
                    }
                    else if (state.Equals("idleLeft")) {
                        currentAnim = idleLeft;
                    }
                    else if (state.Equals("idleDown")) {
                        currentAnim = idleDown;
                    }
                    else if (state.Equals("idleUp")) {
                        currentAnim = idleUp;
                    }
                    else if (state.Equals("moveRight")) {
                        currentAnim = moveRight;
                    }
                    else if (state.Equals("moveLeft")) {
                        currentAnim = moveLeft;
                    }
                    else if (state.Equals("moveUp")) {
                        currentAnim = moveUp;
                    }
                    else if (state.Equals("moveDown")) {
                        currentAnim = moveDown;
                    }
                    else if (state.Equals("attackRight")) {
                        currentAnim = attackRight;
                    }
                    else if (state.Equals("attackLeft")) {
                        currentAnim = attackLeft;
                    }
                    else if (state.Equals("attackUp")) {
                        currentAnim = attackUp;
                    }
                    else if (state.Equals("attackDown")) {
                        currentAnim = attackDown;
                    }
                    else if (state.Equals("turnUpLeft")) {
                        currentAnim = turnUpLeft;
                    }
                    else if (state.Equals("turnLeftDown")) {
                        currentAnim = turnLeftDown;
                    }
                    else if (state.Equals("turnDownRight")) {
                        currentAnim = turnDownRight;
                    }
                    else if (state.Equals("turnRightUp")) {
                        currentAnim = turnRightUp;
                    }
                    else if (state.Equals("turnUpRight")) {
                        currentAnim = turnUpRight;
                    }
                    else if (state.Equals("turnRightDown")) {
                        currentAnim = turnRightDown;
                    }
                    else if (state.Equals("turnDownLeft")) {
                        currentAnim = turnDownLeft;
                    }
                    else if (state.Equals("turnLeftUp")) {
                        currentAnim = turnLeftUp;
                    }else {
                        shuffleMalfunction();
                        currentAnim = malfunction;
                    }
                    moving = true;
                    idle = true;
                }else if (!character.alive) {
                    if (state.Equals("deathright") || state.Equals("deathleft") || state.Equals("deathdown") || state.Equals("deathup")){
                        if (state.Equals("deathleft")) {
                            animRenderer.sprite = deathLeft[9];
                        }
                        else if (state.Equals("deathup")) {
                            animRenderer.sprite = deathUp[9];
                        }
                        else if (state.Equals("deathright")) {
                            animRenderer.sprite = deathRight[9];
                        }
                        else if (state.Equals("deathdown")) {
                            animRenderer.sprite = deathDown[9];
                        }
                        animRunning = false;
                        return;
                    }
                    state = "death" + hyp.getDirection();

                    if (state.Equals("deathleft")) {
                        currentAnim = deathLeft;
                    }
                    else if (state.Equals("deathup")) {
                        currentAnim = deathUp;
                    }
                    else if (state.Equals("deathright")) {
                        currentAnim = deathRight;
                    }
                    else if (state.Equals("deathdown")) {
                        currentAnim = deathDown;
                    }

                    if (!idle) {
                        GameMaster.nextPlayer();
                    }
                    moving = true;
                    idle = true;
                }else{
                    state = "idle" + hyp.getDirection();

                    if (state.Equals("idleleft")) {
                        currentAnim = idleLeft;
                    }
                    else if (state.Equals("idleup")) {
                        currentAnim = idleUp;
                    }
                    else if (state.Equals("idleright")) {
                        currentAnim = idleRight;
                    }
                    else if (state.Equals("idledown")) {
                        currentAnim = idleDown;
                    }
                }
                animRenderer.sprite = currentAnim[0];
                timeAtAnimStart = Time.timeSinceLevelLoad;
                frameIndex = 0;
                //character.setAction(state);
            }
            if(frameIndex != lastIndex) {
                character.performAction(frameIndex);
            }
            lastIndex = frameIndex;
            
        }
        healthBar.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 0.05f);
        staminaBar.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 0.05f);
        healthBar.transform.localScale = new Vector3(gameObject.GetComponent<Character>().percentHealth(), 1, 1);
        staminaBar.transform.localScale = new Vector3(gameObject.GetComponent<Character>().percentStamina(), 1, 1);
    }

    public void setState() {
        state = character.nextState();
    }

    public string getState() {
        return state;
    }

    public void setAnimRunning(bool isRunning) {
        animRunning = isRunning;
        timeAtAnimStart = -69;
        if(isRunning == false) {
            setDirection(hyp.getDirection());
            moving = false;
            idle = true;
        }
    }

    public bool isAlive() {
        return animRunning;
    }

    public void setIdle(bool isIdle) {
        if (character.alive) {
            idle = isIdle;
            if (!isIdle) {
            }
        }else {
            GameMaster.nextPlayer();
        }
        
    }

    public void setDirection(string direction) {
        if (direction.Equals("up")) {
            currentAnim = idleUp;
            animRenderer.sprite = idleUp[0];
            state = "idleUp";
        }
        else if (direction.Equals("left")) {
            currentAnim = idleLeft;
            animRenderer.sprite = idleLeft[0];
            state = "idleLeft";
        }
        else if (direction.Equals("down")) {
            currentAnim = idleDown;
            animRenderer.sprite = idleDown[0];
            state = "idleDown";
        }
        else {
            currentAnim = idleRight;
            animRenderer.sprite = idleRight[0];
            state = "idleRight";
        }
    }

    //Creates a new lot of malfunction sprites to make the malfunctioning animation look more random.
    public void shuffleMalfunction() {
        malfunction = new Sprite[10];
        malfunction[0] = idleRight[UnityEngine.Random.Range(0, 10)];
        malfunction[1] = attackUp[UnityEngine.Random.Range(0, 10)];
        malfunction[2] = idleDown[UnityEngine.Random.Range(0, 10)];
        malfunction[3] = idleLeft[UnityEngine.Random.Range(0, 10)];
        malfunction[4] = idleRight[UnityEngine.Random.Range(0, 10)];
        malfunction[5] = idleUp[UnityEngine.Random.Range(0, 10)];
        malfunction[6] = idleDown[UnityEngine.Random.Range(0, 10)];
        malfunction[7] = idleLeft[UnityEngine.Random.Range(0, 10)];
        malfunction[8] = idleRight[UnityEngine.Random.Range(0, 10)];
        malfunction[9] = idleUp[UnityEngine.Random.Range(0, 10)];
    }

    public void deleteBars() {
        GameObject.Destroy(healthBar.gameObject);
        GameObject.Destroy(staminaBar.gameObject);
    }
}
