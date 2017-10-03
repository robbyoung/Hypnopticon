using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Obstacle : MonoBehaviour {

    public int type;

    public int getX() {
        return (int)Math.Round(transform.position.x);
    }

    public int getY() {
        return (int)Math.Round(transform.position.y);
    }

    public string export() {
        return "o\n" + type + "\n" + getX() + " " + getY() + "\n";
    }

    public void import(string s) {
        string[] info = s.Split(' ');
        transform.position = new Vector3(Convert.ToInt32(info[0]), Convert.ToInt32(info[1]) + 0.1f, transform.position.z);
    }

}