using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Indiemation : MonoBehaviour {

    public Sprite[] myAnimation;
    private SpriteRenderer animRenderer;
    private float timeOfCreation;

    void Start() {
        animRenderer = GetComponent<Renderer>() as SpriteRenderer;
        animRenderer.sprite = myAnimation[0];
        timeOfCreation = Time.timeSinceLevelLoad;
    }

    void Update() {
        int framesPerSecond = GameMaster.fps;
        int frame = (int)((Time.timeSinceLevelLoad - timeOfCreation) * framesPerSecond);
        if (frame >= myAnimation.Length) {
            frame = 0;
            Destroy(gameObject);
        }
        animRenderer.sprite = myAnimation[frame];
    }


}