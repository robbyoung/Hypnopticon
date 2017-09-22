using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

    public Sprite[] titleAnim;
    private SpriteRenderer animRenderer;
    public int framesPerSecond;
    public GameObject storyButton;
    public GameObject sandboxButton;
    public int loop;

    void Start () {
        animRenderer = GetComponent<Renderer>() as SpriteRenderer;
        animRenderer.sprite = titleAnim[0];
        storyButton.SetActive(false);
        sandboxButton.SetActive(false);
        loop = 0;
    }
	
	void Update () {
        int frame = (int)(Time.timeSinceLevelLoad * framesPerSecond - loop);
        if(frame >= titleAnim.Length) {
            loop += 5;
            frame = 18;
            storyButton.SetActive(true);
            sandboxButton.SetActive(true);
        }
        animRenderer.sprite = titleAnim[frame];
    }

    
}
