using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour
{
    public GameObject HUD;
    DodgingGameController.GameState currentState;

    public int velocity;
    Vector2 movementDirection;

    Rigidbody2D rb;

    struct MeteorInitVals
    {
        public Vector2 startPos;
        public Vector2 movementDir;
    }

    MeteorInitVals left, right, top, bottom;

    // Start is called before the first frame update
    void Start()
    {
        HUD = GameObject.FindGameObjectWithTag("HUD");

        rb = gameObject.GetComponent<Rigidbody2D>();

        left.startPos = new Vector2(-75f, 0f);
        left.movementDir = new Vector2(1f, 0f);

        right.startPos = new Vector2(75f, 0f);
        right.movementDir = new Vector2(-1f, 0f);

        top.startPos = new Vector2(50f, 0f);
        top.movementDir = new Vector2(0f, -1f);

        bottom.startPos = new Vector2(-50f, 0f);
        bottom.movementDir = new Vector2(0f, 1f);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Meteor"))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Background"))
        {
            Destroy(gameObject);
        }
    }

    void AssignMeteorValues(Vector2 startPos, Vector2 movementDir)
    {
        gameObject.transform.position = startPos;
        movementDirection = movementDir;
    }

    void MoveMeteor()
    {
        rb.velocity = movementDirection * velocity;
    }

    public void InitMeteorVals(Vector2 shipPos, string startPos)
    {
        switch (startPos)
        {
            case "Left":
            {
                gameObject.transform.position = new Vector2(left.startPos.x, shipPos.y);
                movementDirection = left.movementDir;
                break;
            }
            case "Right":
            {
                gameObject.transform.position = new Vector2(right.startPos.x, shipPos.y);
                movementDirection = right.movementDir;
                break;
            }
            case "Top":
            {
                gameObject.transform.position = new Vector2(shipPos.x, top.startPos.y);
                movementDirection = top.movementDir;
                break;
            }
            case "Bottom":
            {
                gameObject.transform.position = new Vector2(shipPos.x, bottom.startPos.y);
                movementDirection = bottom.movementDir;
                break;
            }
        }
    }
}
