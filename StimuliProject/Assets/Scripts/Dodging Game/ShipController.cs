﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float movementVelocity;
    Vector3 nextShipPosition;
    bool shipIsMoving;

    //State variable
    public GameObject HUD;
    DodgingGameController.GameState currentState;

    // Start is called before the first frame update
    void Start()
    {
        ResetShip();
    }

    // Update is called once per frame
    void Update()
    {
        currentState = HUD.GetComponent<DodgingGameController>().GetGameState();

        if (currentState == DodgingGameController.GameState.wait)
        {
            if (Input.GetMouseButtonDown(0))
            {
                nextShipPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 50));
                shipIsMoving = true;
            }

            if (shipIsMoving)
            {
                MoveShip(nextShipPosition);
            }
        }

        if (currentState == DodgingGameController.GameState.end)
        {
            nextShipPosition = Vector3.zero;
            shipIsMoving = false;

            if (Input.GetMouseButtonDown(0))
            {
                ResetShip();
            }
        }
    }

    void MoveShip(Vector3 mousePosOnClick)
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, mousePosOnClick, movementVelocity * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HUD.GetComponent<DodgingGameController>().SetGameState(DodgingGameController.GameState.end);
    }

    void ResetShip()
    {
        gameObject.transform.position = Vector3.zero;
    }
}
