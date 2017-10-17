using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level2 : MonoBehaviour {
    Launcher launcher;
    bool entry = false;
    float time = 0;
	// Use this for initialization
	void Start () {
        launcher = transform.parent.GetComponent<Launcher>();
    }

    // Update is called once per frame
    void Update () {
        if (time > 0)
        {
            time -= Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        }
        else
            gameObject.SetActive(false);
    }

    // Display lvl 2
    public void Launch()
    {
        gameObject.SetActive(true);
        time = 2;
        transform.position = new Vector3(transform.position.x, 70, transform.position.z);
    }
}
