using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapSpace : MonoBehaviour {

    public Text display;
    public string locationName;
    public List<string> neighbours;
    public string story;
    public string field;
    public GameObject knife;
    public Sprite available;

	void Start () {
        if (Hypnopticon.location.Equals(locationName)) {
            select();
        }
        if (isNeighbour()) {
            //Image sr = GetComponent<Image>();
            //sr.sprite = available;
        }
	}
	
    public void select() {
        if (isNeighbour()) {
            display.text = name;
            Hypnopticon.nextLocation = locationName;
            Hypnopticon.nextBattle = field;
            Hypnopticon.nextConvo = story;
            Hypnopticon.nextNeighbours = neighbours;
            knife.transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f, 99);
        }
    }

    private bool isNeighbour() {
        if (locationName.Equals(Hypnopticon.location)) {
            return true;
        }else {
            for(int i = 0; i < Hypnopticon.neighbours.Count; i++) {
                if (locationName.Equals(Hypnopticon.neighbours[i])) {
                    return true;
                }
            }
        }
        return false;
    }

}
