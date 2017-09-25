using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Conversation : MonoBehaviour {

    public TextAsset xmlFile;
    public Text dialogueBox;
    public Image leftPortrait;
    public Image rightPortrait;
    private int currentIndex;
    public List<Sprite> portraits;
    private List<string> dialogue;
    private List<int> portraitIndices;
    private List<string> portraitSides;
    private Color transparent;
    private Color opaque;

	// Use this for initialization
	void Start () {
        transparent = rightPortrait.color;
        opaque = rightPortrait.color;
        transparent.a = 0.6f;
        dialogue = new List<string>();
        portraitIndices = new List<int>();
        portraitSides = new List<string>();
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlFile.text);
        XmlNodeList xList = xmlDoc.GetElementsByTagName("speech");
        foreach(XmlNode pInfo in xList) {
            dialogue.Add(pInfo.InnerText);
        }
        xList = xmlDoc.GetElementsByTagName("pic");
        foreach (XmlNode pInfo in xList) {
            portraitIndices.Add(Convert.ToInt32(pInfo.InnerText));
        }
        xList = xmlDoc.GetElementsByTagName("side");
        foreach (XmlNode pInfo in xList) {
            portraitSides.Add(pInfo.InnerText);
        }

        currentIndex = 0;
        if (portraitSides[currentIndex].Equals("l")) {
            dialogueBox.alignment = TextAnchor.MiddleLeft;
            leftPortrait.sprite = portraits[portraitIndices[currentIndex]];
            leftPortrait.color = opaque;
            rightPortrait.color = transparent;
        }
        else {
            dialogueBox.alignment = TextAnchor.MiddleRight;
            rightPortrait.sprite = portraits[portraitIndices[currentIndex]];
            rightPortrait.color = opaque;
            leftPortrait.color = transparent;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            currentIndex++;
            if (currentIndex < dialogue.Count) {
                dialogueBox.text = dialogue[currentIndex];
                if (portraitSides[currentIndex].Equals("l")) {
                    dialogueBox.alignment = TextAnchor.MiddleLeft;
                    leftPortrait.sprite = portraits[portraitIndices[currentIndex]];
                    leftPortrait.color = opaque;
                    rightPortrait.color = transparent;
                }
                else {
                    dialogueBox.alignment = TextAnchor.MiddleRight;
                    rightPortrait.sprite = portraits[portraitIndices[currentIndex]];
                    rightPortrait.color = opaque;
                    leftPortrait.color = transparent;
                }
            }
            else {
                SceneManager.LoadScene("Field");
            }
        } 
	}
}
