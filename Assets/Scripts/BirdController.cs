using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BirdController : MonoBehaviour
{

    [SerializeField]
    private float gravity;

    private bool jumpPressed;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float rotationUpLimit;

    [SerializeField]
    private float rotationDownStartingVelocity;

    [SerializeField]
    private float rotationUpSpeed;

    [SerializeField]
    private float rotationDownSpeed;

    [SerializeField]
    private float rotationDownLimit;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private float groungHeight;

    [SerializeField]
    private HoverDetect hoverDetect;

    private Vector2 pausedVelocity;

    private bool actualVelocitySet;

    private bool obstacleHit;

    private bool onFloor;

    private float flatAngle = 180f;

    void Start()
    {
        jumpPressed = false;

        obstacleHit = false;

        onFloor = false;

        actualVelocitySet = true;
    }


    void Update()
    {
        if (!obstacleHit && !gameManager.IsPaused() && !jumpPressed)
        {
           
            jumpPressed = Input.GetKeyDown(KeyCode.Space) || (!hoverDetect.IsCursorOverButton() && Input.GetMouseButtonDown(0));

        }
    }


    private void FixedUpdate()
    {

        if (gameManager.IsPaused() && actualVelocitySet)
        {
            pausedVelocity = rb.velocity;
            rb.velocity = Vector2.zero;
            actualVelocitySet = false;
        }
        else if (!gameManager.IsPaused() && !actualVelocitySet)
        {
            rb.velocity = pausedVelocity;
            actualVelocitySet = true;
        }


        if (!onFloor && !gameManager.IsPaused())
        {

            float fixedDelta = Time.fixedDeltaTime;

            if (jumpPressed)
            {

                rb.velocity = new Vector2(0, jumpForce);

                jumpPressed = false;


            }

            rb.velocity -= new Vector2(0, gravity) * fixedDelta;

          

            if (rb.velocity.y > 0f)
            {

                rb.transform.eulerAngles += new Vector3(0f, 0f, rotationUpSpeed) * fixedDelta;

                if (rb.transform.eulerAngles.z > rotationUpLimit && rb.transform.eulerAngles.z < flatAngle)
                {
                    rb.transform.eulerAngles = new Vector3(0f, 0f, rotationUpLimit);
                }
            }

            else if (rb.velocity.y <= rotationDownStartingVelocity || obstacleHit)
            {


                rb.transform.eulerAngles -= new Vector3(0f, 0f, rotationDownSpeed) * Time.fixedDeltaTime;


                if (rb.transform.eulerAngles.z < rotationDownLimit && rb.transform.eulerAngles.z > flatAngle)
                {

                    rb.transform.eulerAngles = new Vector3(0f, 0f, rotationDownLimit);
                }


            }

            if (obstacleHit && rb.position.y <= groungHeight)
            {
                onFloor = true;
                rb.velocity = new Vector2(0f, 0f);
            }

        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (!gameManager.IsPaused() && !obstacleHit)
        {
            obstacleHit = true;
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(0f, 0f);
            }

            gameManager.HandleDeath();
        }

    }
}
