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

    public enum Direction { left, right, split };
    public Direction nextDirection;

    float directionSpacing = 18.5f;
    Vector3 turningSpeed = Vector3.zero;
    public float turningTime;

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

        UpdateCar();
        //If trigger activated and input is made, move the car
    }

    private void OnTriggerEnter(Collider other)
    {
        if (inputMade)
        {
            MoveCar();
        }
        else
        {
            GameController.GetComponent<DrivingGameController>().SetGameState(DrivingGameController.GameState.end);
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
            GameController.GetComponent<DrivingGameController>().DisplayDirection(currentTrack.ToString());
            switch (currentTrack.GetComponent<TileID>().GetID())
            {
                case "Right Turn":
                {
                    GameController.GetComponent<DrivingGameController>().DisplayDirection("Right");
                    break;
                }
                case "Right Return":
                {
                    GameController.GetComponent<DrivingGameController>().DisplayDirection("Left");
                    break;
                }
                case "Left Turn":
                {
                    GameController.GetComponent<DrivingGameController>().DisplayDirection("Left");
                    break;
                }
                case "Left Return":
                {
                    GameController.GetComponent<DrivingGameController>().DisplayDirection("Right");
                    break;
                }
                case "Split Fork":
                {
                    GameController.GetComponent<DrivingGameController>().DisplayDirection("Split");
                    break;
                }
                case "Split Join":
                {
                    GameController.GetComponent<DrivingGameController>().DisplayDirection("Join");
                    break;
                }
            }
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
        inputMade = true;
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
            if (leftWasPressed)
            {
                gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position,
                                                                   new Vector3(gameObject.transform.position.x - directionSpacing, 
                                                                   gameObject.transform.position.y, gameObject.transform.position.z),
                                                                   ref turningSpeed, turningTime);

            }
            else
            {
                gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position,
                                                                   new Vector3(gameObject.transform.position.x + directionSpacing,
                                                                   gameObject.transform.position.y, gameObject.transform.position.z),
                                                                   ref turningSpeed, turningTime);
            }
        }        
    }
}
