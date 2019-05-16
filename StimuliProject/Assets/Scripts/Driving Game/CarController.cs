using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //GameObjects defined
    public GameObject GameController;
    public GameObject Track;
    GameObject currentTrack;
    GameObject thisTrack;

    //Car variables defined
    bool inputAllowed;
    bool inputMade;
    bool leftWasPressed;
    bool chosenDirectionIsCorrect;
    bool directionHasBeenDisplayed;
    bool carIsMoving;
    float directionSpacing = 18.5f;
    Vector3 turningSpeed = Vector3.zero;
    public float turningTime;
    public float moveSpeed = 8;

    //Game state variables defined
    public enum Direction { left, right, split };
    public Direction nextDirection;
    Direction currentDirection;
    public enum Position { left, middle, right};
    public Position currentPosition;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = Position.middle;
        inputAllowed = false;
        directionHasBeenDisplayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (inputAllowed)
            {
                //Calculate input time
                GameController.GetComponent<DrivingGameController>().SetCurrentReactionTimeValue(true);

                //Add tap position to list
                GameController.GetComponent<DrivingGameController>().AddTapPositionToList(Input.mousePosition);

                //Hide the direction
                GameController.GetComponent<DrivingGameController>().SetDirectionActive(false);
                directionHasBeenDisplayed = false;

                //Check side of click
                if (Input.mousePosition.x > (Screen.width / 2))
                {
                    leftWasPressed = false;
                }
                else
                {
                    leftWasPressed = true;
                }

                CheckInput(leftWasPressed);
                
                Handheld.Vibrate();
            }
        }

        UpdateCar();
        //If trigger activated and input is made, move the car
    }

    //Move the car if triggered
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (inputMade)
        {
            inputMade = false;
            //MoveCar();
            carIsMoving = true;
        }
        else
        {
            GameController.GetComponent<DrivingGameController>().SetGameState(DrivingGameController.GameState.end);
        }
    }

    //Set the allow input value
    public void AllowInput(bool value)
    {
        inputAllowed = value;
    }

    //Update the next direction value
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

    //Update the car values
    void UpdateCar()
    {
        currentTrack = Track.GetComponent<TrackController>().GetNextTrack();
        
        if (currentTrack.CompareTag("DirectionChange") && !directionHasBeenDisplayed && thisTrack != currentTrack)
        {
            //Display stimuli and allow input from player

            thisTrack = currentTrack;
            AllowInput(true);

            switch (currentTrack.GetComponent<TileID>().GetID())
            {
                case "Right Turn":
                {
                    GameController.GetComponent<DrivingGameController>().DisplayDirection("Right");
                    currentDirection = Direction.right;
                    break;
                }
                case "Right Return":
                {
                    GameController.GetComponent<DrivingGameController>().DisplayDirection("Left");
                    currentDirection = Direction.left;
                    break;
                }
                case "Left Turn":
                {
                    GameController.GetComponent<DrivingGameController>().DisplayDirection("Left");
                    currentDirection = Direction.left;
                    break;
                }
                case "Left Return":
                {
                    GameController.GetComponent<DrivingGameController>().DisplayDirection("Right");
                    currentDirection = Direction.right;
                    break;
                }
                case "Split Fork":
                {
                    GameController.GetComponent<DrivingGameController>().DisplayDirection("Split");
                    currentDirection = Direction.split;
                    break;
                }
                case "Split Join":
                {
                    GameController.GetComponent<DrivingGameController>().DisplayDirection("Join");
                    currentDirection = Direction.split;
                    break;
                }
            }

            directionHasBeenDisplayed = true;
            GameController.GetComponent<DrivingGameController>().SetDirectionActive(true);

            //Start timer between now and input from user
            GameController.GetComponent<DrivingGameController>().SetCurrentReactionTimeValue(false);
        }

        if (carIsMoving)
        {
            MoveCar();
        }
    }

    //Check the input value
    void CheckInput(bool leftWasPressed)
    {
        switch (currentDirection)
        {
            case Direction.left:
            {
                if (leftWasPressed)
                {
                    //Direction is correct
                    chosenDirectionIsCorrect = true;
                    if (currentPosition == Position.right)
                    {
                        currentPosition = Position.middle;
                    }
                    else
                    {
                        currentPosition = Position.left;
                    }
                }
                else
                {
                    //Direction is not correct
                    chosenDirectionIsCorrect = false;
                }
                break;
            }
            case Direction.right:
            {
                if (leftWasPressed)
                {
                    //Direction is correct
                    chosenDirectionIsCorrect = false;
                }
                else
                {
                    //Direction is not correct
                    chosenDirectionIsCorrect = true;
                    if (currentPosition == Position.left)
                    {
                        currentPosition = Position.middle;
                    }
                    else
                    {
                        currentPosition = Position.right;
                    }
                }
                break;
            }
            case Direction.split:
            {
                if (thisTrack.GetComponent<TileID>().GetID() == "Split Fork")
                {
                    if (leftWasPressed)
                    {
                        //Direction is correct
                        chosenDirectionIsCorrect = true;
                        currentPosition = Position.left;
                    }
                    else
                    {
                        //Direction is correct
                        chosenDirectionIsCorrect = true;
                        currentPosition = Position.right;
                    }
                    break;
                }
                else 
                {
                    if (currentPosition == Position.left)
                    {
                        if (leftWasPressed)
                        {
                            //Direction is correct
                            chosenDirectionIsCorrect = false;
                        }
                        else
                        {
                            //Direction is correct
                            chosenDirectionIsCorrect = true;
                            currentPosition = Position.middle;
                        }
                        break;
                    }
                    else
                    {
                        if (leftWasPressed)
                        {
                            //Direction is correct
                            chosenDirectionIsCorrect = true;
                            currentPosition = Position.middle;
                        }
                        else
                        {
                            //Direction is correct
                            chosenDirectionIsCorrect = false;
                        }
                        break;
                    }
                }                
            }
        }
        inputMade = true;
    }

    //Move the car if click was made
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
                if (currentPosition == Position.middle)
                {
                    if (gameObject.transform.position.x <= 0)
                    {
                        carIsMoving = false;
                    }
                }
                else
                {
                    if (gameObject.transform.position.x <= -directionSpacing)
                    {
                        carIsMoving = false;
                    }
                }  
            }
            else
            {
                gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position,
                                                                   new Vector3(gameObject.transform.position.x + directionSpacing,
                                                                   gameObject.transform.position.y, gameObject.transform.position.z),
                                                                   ref turningSpeed, turningTime);
                if (currentPosition == Position.middle)
                {
                    if (gameObject.transform.position.x >= 0)
                    {
                        carIsMoving = false;
                    }
                }
                else
                {
                    if (gameObject.transform.position.x >= directionSpacing)
                    {
                        carIsMoving = false;
                    }
                }  
            }
        }        
    }
}
