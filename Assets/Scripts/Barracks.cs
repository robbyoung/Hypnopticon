using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barracks: MonoBehaviour {

    public List<CharacterHusk> units;
    private int index;
    public Text nameBox;
    public Text typeBox;


    // Use this for initialization
    void Start() {
        units = new List<CharacterHusk>();
        index = 0;
        for(int t = 0; t < Hypnopticon.unitCount.Count; t++) {
            for (int i = 0; i < Hypnopticon.unitCount[t]; i++) {
                units.Add(new CharacterHusk(t));
            }
        }

        if(units.Count == 0) {
            gameObject.SetActive(false);
        }else {
            nameBox.text = units[index].unitName;
            typeBox.text = units[index].type;
        }
        
    }

    public void next() {
        index = index + 1;
        if (index >= units.Count) {
            index = 0;
        }
        nameBox.text = units[index].unitName;
        typeBox.text = units[index].type;
    }

    public void previous() {
        index = index - 1;
        if (index < 0) {
            index = units.Count - 1;
        }
        nameBox.text = units[index].unitName;
        typeBox.text = units[index].type;
    }
}

public class CharacterHusk{

    public string type;
    public string unitName;

    public CharacterHusk(int t) {
        if(t == 0) {
            type = "Swordsman";
        }
        else if (t == 1) {
            type = "Man at Arms";
        }
        else{
            type = "Archer";
        }

        unitName = randomName();
    }

    public string randomName() {
        List<string> names = new List<string>();
        names.Add("Joseph");
        names.Add("Harald");
        names.Add("Gerard");
        names.Add("Minceboy");
        int r = (int)(Random.value * names.Count);
        return names[r];
    }
}
