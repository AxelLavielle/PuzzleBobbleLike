using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUI : MonoBehaviour {
    Map map;
    bool start;

    // initialisation
    void Start () {
        map = FindObjectOfType(typeof(Map)) as Map;
        start = true;
    }

    public void Display(bool victory)
    {
        gameObject.SetActive(true);
        if (start) // if first time
        {
            start = false;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
        }
        else if (victory) // if victory
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else // if gameover
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    //starts the game
    public void StartGame()
    {
        map.reset(false);
        gameObject.SetActive(false);
    }

    // quits the game
    public void quitGame()
    {
        Application.Quit();
    }
}
