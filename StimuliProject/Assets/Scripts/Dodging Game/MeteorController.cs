using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour
{
    //Define the meteor variables
    public GameObject HUD;
    DodgingGameController.GameState currentState;
    int velocity = 20;
    Vector2 movementDirection;
    Rigidbody2D rb;

    //A struct defining the meteor initial values
    public struct MeteorInitVals
    {
        public MeteorInitVals(Vector2 sp, Vector2 md)
        {
            startPos = sp;
            movementDir = md;
        }

        public Vector2 startPos;
        public Vector2 movementDir;
    }

    //The initial values defined
    MeteorInitVals left = new MeteorInitVals(new Vector2(-75f, 0f), new Vector2(1f, 0f));
    MeteorInitVals right = new MeteorInitVals(new Vector2(75f, 0f), new Vector2(-1f, 0f));
    MeteorInitVals top = new MeteorInitVals(new Vector2(0f, 50f), new Vector2(0f, -1f));
    MeteorInitVals bottom = new MeteorInitVals(new Vector2(0f, -50f), new Vector2(0f, 1f));


    // Start is called before the first frame update
    void Start()
    {
        HUD = GameObject.FindGameObjectWithTag("HUD");

        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        currentState = HUD.GetComponent<DodgingGameController>().GetGameState();

        if (currentState == DodgingGameController.GameState.wait)
        {
            MoveMeteor();
        }
    }

    //Check for meteor collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Meteor"))
        {
            Debug.Log("Meteors have collided.");
            Destroy(gameObject);
        }
    }

    //Check if the meteor has left the game area
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Meteor has left BG box.");
        Destroy(gameObject);
    }

    //Assign the meteor values
    void AssignMeteorValues(Vector2 startPos, Vector2 movementDir)
    {
        gameObject.transform.position = startPos;
        movementDirection = movementDir;
    }

    //Move the meteor
    void MoveMeteor()
    {
        rb.velocity = movementDirection * velocity;
    }

    //Init the meteor values
    public void InitMeteorVals(Vector2 shipPos, string startPos)
    {
        switch (startPos)
        {
            case "Left":
            {
                Debug.Log("Left Meteor Created.");
                gameObject.transform.position = new Vector2(left.startPos.x, shipPos.y);
                movementDirection = left.movementDir;
                break;
            }
            case "Right":
            {
                Debug.Log("Right Meteor Created.");
                gameObject.transform.position = new Vector2(right.startPos.x, shipPos.y);
                movementDirection = right.movementDir;
                break;
            }
            case "Top":
            {
                Debug.Log("Top Meteor Created.");
                gameObject.transform.position = new Vector2(shipPos.x, top.startPos.y);
                movementDirection = top.movementDir;
                break;
            }
            case "Bottom":
            {
                Debug.Log("Bottom Meteor Created.");
                gameObject.transform.position = new Vector2(shipPos.x, bottom.startPos.y);
                movementDirection = bottom.movementDir;
                break;
            }
        }
    }
}
