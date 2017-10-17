using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {//script needed only for animation boolean
    Launcher launcher;
    [SerializeField]
    bool fire = false;
    Animator mAnimator;

    // Use this for initialization
    void Start () {
        launcher = transform.parent.GetComponent<Launcher>();
        mAnimator = GetComponent<Animator>();
        mAnimator.SetBool("Fire", fire);
    }

    // Update is called once per frame
    void Update () {
        fire = launcher.inWaitForBall;
        mAnimator.SetBool("Fire", fire);
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(launcher.angle.y, launcher.angle.x) * Mathf.Rad2Deg - 90, Vector3.forward); ;
    }
}
