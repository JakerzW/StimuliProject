using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public GameObject GameController;
    public GameObject Track;

    bool inputAllowed;
    bool inputMade;
    bool leftWasPressed;
    bool chosenDirectionIsCorrect;

    enum Direction { left, right, split };
    Direction nextDirection;

    GameObject currentTrack;

    // Start is called before the first frame update
    void Start()
    {
        inputAllowed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (inputAllowed)
            {
                //Calculate input time

                if (Input.mousePosition.x > (Screen.width / 2))
                {
                    leftWasPressed = false;
                }
                else
                {
                    leftWasPressed = true;
                }

                CheckInput(leftWasPressed);
            }
        }
    }

    public void AllowInput(bool value)
    {
        inputAllowed = value;
    }

    public void UpdateDiection(string newDir)
    {
        switch (newDir)
        {
            case "Left":
            {
                nextDirection = Direction.left;
                break;
            }
            case "Right":
            {
                nextDirection = Direction.right;
                break;
            }
            case "Split":
            {
                nextDirection = Direction.split;
                break;
            }
        }
    }

    void UpdateCar()
    {
        currentTrack = Track.GetComponent<TrackController>().GetNextTrack();
        if (currentTrack.CompareTag("DirectionChange"))
        {
            //Display stimuli and allow input from player
            AllowInput(true);

            //Start timer between now and input from user
        }
    }

    void CheckInput(bool leftWasPressed)
    {
        switch (nextDirection)
        {
            case Direction.left:
            {
                if (leftWasPressed)
                {
                    chosenDirectionIsCorrect = true;
                    //Direction is correct
                }
                else
                {
                    chosenDirectionIsCorrect = false;
                    //Direction is not correct
                }
                break;
            }
            case Direction.right:
            {
                if (!leftWasPressed)
                {
                    chosenDirectionIsCorrect = false;
                    //Direction is correct
                }
                else
                {
                    chosenDirectionIsCorrect = true;
                    //Direction is not correct
                }                      
                break;
            }
            case Direction.split:
            {
                if (leftWasPressed)
                {
                    chosenDirectionIsCorrect = true;
                    //Direction is correct
                }
                else
                {
                    chosenDirectionIsCorrect = true;
                    //Direction is correct
                }
                break;
            }
        }
    }

    void MoveCar()
    {
        //End game if movement is wrong
        if (!chosenDirectionIsCorrect)
        {
            GameController.GetComponent<DrivingGameController>().SetGameState(DrivingGameController.GameState.end);
        }
        else
        {

        }
        
    }
}
