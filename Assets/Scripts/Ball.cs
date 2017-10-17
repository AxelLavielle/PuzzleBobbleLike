using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private int color;
    private SpriteRenderer sprite;
    public Sprite[] spriteList;
    public bool isFixed = false;
    public float mSpeed;
    [SerializeField]
    GameObject particles;
    CircleCollider2D mCollider;
    Rigidbody2D mRigidBody2D;
    Vector2 direction;

    // Initialisation
    private void Awake()
    {
        isFixed = false;
        mRigidBody2D = GetComponent<Rigidbody2D>();
        mCollider = GetComponent<CircleCollider2D>();
        mRigidBody2D.constraints = RigidbodyConstraints2D.None;
    }

    // Destruction of the ball when it goes off the map
    private void Update()
    {
        if (transform.position.y < -135)
            destruct();
    }

    // Fire the ball in the direction given
    public void Fire(Vector2 directionInput)
    {
        direction = directionInput;
        mRigidBody2D.AddForce(direction * mSpeed);
    }

    // Returns the color of the ball
    public int getColor()
    {
        return (color);
    }

    // Sets the color of the ball
    public void setColor(int colorInput)
    {
        color = colorInput;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        sprite.sprite = spriteList[color];
    }

    // Destroy the ball without animation
    public void destruct()
    {
        Destroy(gameObject);
    }

    // Destroy the ball with animation
    public void destructParticle()
    {
        Destroy(gameObject);
        particles = Instantiate(particles, transform.position, Quaternion.identity) as GameObject;
    }

    // Drop the ball to the floor
    public void drop()
    {
        mCollider.isTrigger = true;
        mRigidBody2D.gravityScale = 20;
        mRigidBody2D.constraints = RigidbodyConstraints2D.None;
    }

    // Collision handler
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFixed) // check if already fixed, in that case we don't need collision anymore
        {
            if (collision.collider.CompareTag("SideWall")) // check if it goes into a sidewall, then make it bounce
            {
                direction.x = -direction.x;
                mRigidBody2D.AddForce(new Vector2(direction.x * mSpeed, 0));
            }
            else if (collision.collider.CompareTag("UpWall")) // check if it goes into the upper wall, to stick into it
            {
                isFixed = true;
                mRigidBody2D.velocity = Vector2.zero;
                mRigidBody2D.angularVelocity = 0;
                mRigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                transform.position = new Vector2(Mathf.Round((transform.position.x + 56) / 16) * 16 - 56, collision.collider.transform.position.y - 16 - 88);
            }
            else if (collision.collider.CompareTag("Ball")) // if it collides with a ball, we have to stick into it
            {
                isFixed = true;
                mRigidBody2D.velocity = Vector2.zero;
                mRigidBody2D.angularVelocity = 0;
                mRigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                if ((collision.collider.transform.position.x < transform.position.x && transform.position.x <= 56) || transform.position.x <= -56) // Ball is right to collided ball
                {
                    if (collision.collider.transform.position.y > transform.position.y + 5) // down
                        transform.position = new Vector2(collision.collider.transform.position.x + 8, collision.collider.transform.position.y - 16);
                    else if (collision.collider.transform.position.y < transform.position.y - 5) // up
                        transform.position = new Vector2(collision.collider.transform.position.x + 8, collision.collider.transform.position.y + 16);
                    else // center
                        transform.position = new Vector2(collision.collider.transform.position.x + 16, collision.collider.transform.position.y);
                }
                else // Ball is left to collided ball
                {
                    if (collision.collider.transform.position.y > transform.position.y + 5) // down
                        transform.position = new Vector2(collision.collider.transform.position.x - 8, collision.collider.transform.position.y - 16);
                    else if (collision.collider.transform.position.y < transform.position.y - 5) // up
                        transform.position = new Vector2(collision.collider.transform.position.x - 8, collision.collider.transform.position.y + 16);
                    else // center
                        transform.position = new Vector2(collision.collider.transform.position.x - 16, collision.collider.transform.position.y);
                }
            }
        }
    }
}