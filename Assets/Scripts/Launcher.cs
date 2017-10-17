using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public Vector2 angle;
    [SerializeField]
    GameObject ballObject;
    Ball ball;
    Ball ballWait;
    public bool inWaitForBall = false;
    Map map;
    private int numberOfShot = 0;
    public bool victory = false;
    private float autofire = 3;
    public bool idle = true;
    MyUI ui;
    level2 lvl;

    // initialization
    private void Start()
    {
        ui = FindObjectOfType(typeof(MyUI)) as MyUI;
        lvl = FindObjectOfType(typeof(level2)) as level2;
        map = transform.parent.GetComponent<Map>();
        GameObject tmp = Instantiate(ballObject, new Vector3(transform.position.x - 2, -94, transform.position.z), Quaternion.identity) as GameObject;
        ball = tmp.GetComponent<Ball>();
        ball.setColor(map.getColor());
        GameObject tmp2 = Instantiate(ballObject, new Vector3(30, -117, 0), Quaternion.identity) as GameObject;
        ballWait = tmp2.GetComponent<Ball>();
        ballWait.setColor(map.getColor());
        angle = new Vector2(0f, 1f);
        ui.Display(true);
    }

    // reinitialization
    public void reset()
    {
        if (idle == true)
            victory = false;
        idle = false;
        if (ball != null)
            ball.destruct();
        if (ballWait != null)
            ballWait.destruct();
        GameObject tmp = Instantiate(ballObject, new Vector3(transform.position.x - 2, -94, transform.position.z), Quaternion.identity) as GameObject;
        ball = tmp.GetComponent<Ball>();
        ball.setColor(map.getColor());
        GameObject tmp2 = Instantiate(ballObject, new Vector3(30, -117, 0), Quaternion.identity) as GameObject;
        ballWait = tmp2.GetComponent<Ball>();
        ballWait.setColor(map.getColor());
        angle = new Vector2(0f, 1f);
        inWaitForBall = false;
        numberOfShot = 0;
        autofire = 3;
    }

    // Update is called once per frame
    private void Update()
    {
        if (idle == false)
        {
            // angle of fire
            autofire -= Time.deltaTime;
            if (Input.GetButton("Left") && angle.x > -0.6)
            {
                angle.x -= 0.02f;
                angle.y = (angle.x < 0) ? (1 + angle.x) : (1 - angle.x);
            }
            if (Input.GetButton("Right") && angle.x < 0.6)
            {
                angle.x += 0.02f;
                angle.y = (angle.x < 0) ? (1 + angle.x) : (1 - angle.x);
            }

            if (!inWaitForBall && (Input.GetButtonDown("Fire") || autofire < 0)) // waiting for a fire
            {
                inWaitForBall = true;
                ball.Fire(angle);
                numberOfShot++;
            }
            else if (inWaitForBall && ball.isFixed) // updating the map after a fire
            {
                inWaitForBall = false;
                int ret = map.addBall(Mathf.RoundToInt(ball.transform.position.x + 56) / 16, (int)((map.GetComponentInChildren<BoxCollider2D>().transform.position.y - 88 - ball.transform.position.y) / 16 - 1), ball.getColor(), ball);
                if (ret == -1) // if game is not win
                {
                    numberOfShot = map.goDown(numberOfShot);
                    if (numberOfShot != -1) // shot was accepted and game continue
                    {
                        ballWait.transform.position = new Vector3(transform.position.x - 2, -94, transform.position.z);
                        ball = ballWait;
                        GameObject tmp = Instantiate(ballObject, new Vector3(30, -117, 1), Quaternion.identity) as GameObject;
                        ballWait = tmp.GetComponent<Ball>();
                        ballWait.setColor(map.getColor());
                    }
                    else // shot was not accepted, game is lost
                    {
                        victory = false;
                        ui.Display(false);
                        idle = true;
                    }
                }
                else if (ret == 0) // if game is lost
                {
                    victory = false;
                    ui.Display(false);
                    idle = true;
                }
                else // if game is win
                {
                    if (!victory) // if it was first level
                    {
                        lvl.Launch();
                        map.reset(true);
                        victory = true;
                    }
                    else // if it was level two
                    {
                        ui.Display(true);
                        idle = true;
                    }
                }
                autofire = 3;
            }
        }
    }
}