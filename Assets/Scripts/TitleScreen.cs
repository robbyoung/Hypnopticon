using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

    public Sprite[] titleAnim;
    private SpriteRenderer animRenderer;
    public int framesPerSecond;
    public GameObject startButton;
	
	void Start () {
        animRenderer = GetComponent<Renderer>() as SpriteRenderer;
        animRenderer.sprite = titleAnim[0];
        startButton.SetActive(false);
	}
	
	void Update () {
        int frame = (int)(Time.timeSinceLevelLoad * framesPerSecond);
        if(frame >= titleAnim.Length) {
            frame = titleAnim.Length - 1;
            startButton.SetActive(true);
        }
        animRenderer.sprite = titleAnim[frame];
    }

    
}
