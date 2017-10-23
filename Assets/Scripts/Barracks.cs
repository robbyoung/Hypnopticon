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
        units = Hypnopticon.units;
        index = 0;
        

        if(units.Count == 0 || !Hypnopticon.storyMode) {
            gameObject.SetActive(false);
            gameObject.transform.position = new Vector3(100, 100, 100);
        }else {
            nameBox.text = units[index].unitName;
            typeBox.text = units[index].unitInfo();
        }
        
    }

    public void next() {
        index = index + 1;
        if (index >= units.Count) {
            index = 0;
        }
        nameBox.text = units[index].unitName;
        typeBox.text = units[index].unitInfo();
    }

    public void previous() {
        index = index - 1;
        if (index < 0) {
            index = units.Count - 1;
        }
        nameBox.text = units[index].unitName;
        typeBox.text = units[index].unitInfo();
    }
}

public class CharacterHusk{
    public string type;
    public int attack;
    public int defense;
    public int speed;
    public string unitName;
    public string script;
    public int typeInt;

    public CharacterHusk(string nname, int t) {
        unitName = nname;
        if (nname.Equals("?")) {
            unitName = randomName();
        }
        typeInt = t;

        if (t == 0) {
            type = "Swordsman";

        }
        else if (t == 1) {
            type = "Man at Arms";
        }
        else {
            type = "Archer";
        }

        attack = 1;
        defense = 1;
        speed = 1;
        script = "";
        //Debug.Log("HUSK CREATED! NAME: " + unitName + " TYPE: " + type);
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

    public string unitInfo() {
        return type + "\n\n" + attack + "\n\n" + defense + "\n\n" + speed;
    }
}
