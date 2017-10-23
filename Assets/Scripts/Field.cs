using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Field : MonoBehaviour {

    public Transform dirtPrefab;
    public Sprite[] terrainSprites;
    public int rows;
    public int columns;
    private int size = 1;
    private Transform[,] squares;
    public Transform[] characterPrefabs;
    public Transform[] obstaclePrefabs;

    void Start() {
        squares = new Transform[columns, rows];
        for (int y = -rows / 2; y < rows - rows / 2; y++) {
            for (int x = -columns / 2; x < columns - columns / 2; ++x) {
                Transform dirt = Instantiate(dirtPrefab);
                dirt.parent = transform;
                dirt.position = new Vector3(x * size, y, -1);
                squares[x + columns / 2, y + rows / 2] = dirt;
                int result = UnityEngine.Random.Range(0, terrainSprites.Length);
                dirt.GetComponent<SpriteRenderer>().sprite = terrainSprites[result];
            }
        }
    }

    void Update() {
        //Clean this up please
        int x = (int)Math.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
        int y = (int)Math.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        if (GameMaster.obstacleMenu && Input.GetKeyDown(KeyCode.Mouse1) && !GameMaster.started && !Hypnopticon.storyMode) {
            if (GameMaster.spaceIsClear(x, y)) {
                Transform obstacle = Instantiate(obstaclePrefabs[GameMaster.currentObstacleType]);
                obstacle.position = new Vector3(x, y + 0.1f, -2);
                GameMaster.addObstacle(obstacle.gameObject.GetComponent<Obstacle>());
            }
        }else if (Input.GetKeyDown(KeyCode.Mouse1) && !GameMaster.started && (!Hypnopticon.storyMode ||  y < 0)) {            
            if (GameMaster.spaceIsClear(x, y)) {
                if (!Hypnopticon.storyMode) {
                    Transform character = Instantiate(characterPrefabs[GameMaster.currentType]);
                    character.position = new Vector3(x, y + 0.1f, -2);
                    GameMaster.addCharacter(character.gameObject.GetComponent<Character>());
                }else if(Hypnopticon.units.Count > 0) {
                    Transform character = Instantiate(characterPrefabs[Hypnopticon.units[0].typeInt]);
                    character.gameObject.GetComponent<Character>().importHusk(Hypnopticon.units[0]);
                    character.position = new Vector3(x, y + 0.1f, -2);
                    GameMaster.addCharacter(character.gameObject.GetComponent<Character>());
                    Hypnopticon.units.RemoveAt(0);
                }
                //print(x + "-> X: " + character.GetComponent<Character>().getX() + ", " + (int)(y + 0.1f) + " -> Y: " + character.GetComponent<Character>().getY());
            }
        }
    }

}
