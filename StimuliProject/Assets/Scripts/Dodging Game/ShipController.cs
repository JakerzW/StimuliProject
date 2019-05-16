using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    //Ship variables defined
    public float movementVelocity;
    Vector3 nextShipPosition;
    bool shipIsMoving;
    bool shipHasMoved;

    //Warning objects
    public GameObject upWarning;
    public GameObject rightWarning;
    public GameObject downWarning;
    public GameObject leftWarning;

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
            if (Input.GetMouseButtonDown(0) && !shipHasMoved)
            {
                //Record reaction time - go to game controller
                HUD.GetComponent<DodgingGameController>().AddReactionTime();
                HUD.GetComponent<DodgingGameController>().AddReactionPosition(Input.mousePosition);

                ShowWarnings(false, false, false, false);

                nextShipPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 50));
                shipIsMoving = true;
                shipHasMoved = true;

                Handheld.Vibrate();
            }

            if (shipIsMoving)
            {
                MoveShip(nextShipPosition);
            }
        }

        if (currentState == DodgingGameController.GameState.spawn)
        {
            shipHasMoved = false;
        }

        if (currentState == DodgingGameController.GameState.end)
        {
            nextShipPosition = Vector3.zero;
            shipIsMoving = false;

            ShowWarnings(false, false, false, false);

            if (Input.GetMouseButtonDown(0))
            {
                ResetShip();
            }
        }
    }

    //Move the ship to the mouse click position
    void MoveShip(Vector3 mousePosOnClick)
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, mousePosOnClick, movementVelocity * Time.deltaTime);
    }

    //CHeck for collision with meteor
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Meteor"))
        {
            HUD.GetComponent<DodgingGameController>().SetGameState(DodgingGameController.GameState.end);
        }
    }

    //Show the warning signs
    public void ShowWarnings(bool up, bool right, bool down, bool left)
    {
        upWarning.SetActive(up);
        rightWarning.SetActive(right);
        downWarning.SetActive(down);
        leftWarning.SetActive(left);
    }

    //Reset the ship values
    void ResetShip()
    {
        gameObject.transform.position = Vector3.zero;

        upWarning.SetActive(false);
        rightWarning.SetActive(false);
        downWarning.SetActive(false);
        leftWarning.SetActive(false);
    }
}
