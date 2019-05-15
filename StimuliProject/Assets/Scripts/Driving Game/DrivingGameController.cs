using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DrivingGameController : MonoBehaviour {

    //Variables for spawning new road segment types
    int[] spawnLengths = { 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3 };
    string[] directionTypes = { "Split", "Split", "Split", "Split", "Split", "Left", "Left", "Left", "Left", "Left", "Right", "Right", "Right", "Right", "Right" };
    public int currentSegment = 0;
    int changesSinceLastSeg = 0;
    public GameObject Track;
    public GameObject Car;

    public enum GameState { start, play, end };
    GameState currentState;

    //Game timer variables defined
    float currentTime;
    int currentCount;
    public float timer;

    public Text countdownText, timerText, gameOverText, directionText;

    //The data controller reference
    public GameObject dataController;

    //The game ID
    DataController.GameID gameID = DataController.GameID.drive;

    //Current game variables
    float averageTimeDriving;
    float bestTimeDriving;
    float worstTimeDriving;
    float[] reactionTimesDriving;
    Vector2[] tapPositionsDriving;
    float timeSpentPlayingDriving;

    //This current reaction timer
    float currentReactionTime;
    List<float> listReactionTimes = new List<float>();
    List<Vector2> listTapPositions = new List<Vector2>();

    // Use this for initialization
    void Start ()
    {
        dataController = GameObject.FindGameObjectWithTag("DataController");

        ResetGame();
	}
	
	// Update is called once per frame
	void Update ()
    {
        currentTime += Time.deltaTime;

        if (currentState == GameState.start)
        {
            if (currentTime > 1 && currentTime < 2)
            {
                currentCount = 2;
            }
            else if (currentTime > 2 && currentTime < 3)
            {
                currentCount = 1;
            }
            else if (currentTime > 3 && currentTime < 4)
            {
                currentCount = 0;
            }
            else if (currentTime > 4)
            {
                countdownText.enabled = false;
                timerText.enabled = true;
                currentState = GameState.play;
                timeSpentPlayingDriving = 0;
            }
        }

        if (currentState == GameState.play)
        {
            //Decrement game timer
            timer -= Time.deltaTime;

            //Increment reaction time timer
            currentReactionTime += Time.deltaTime;

            //Increment time played timer
            timeSpentPlayingDriving += Time.deltaTime;

            if (timer <= 0)
            {
                timer = 0;
                currentState = GameState.end;
            }
            else
            {
                CheckForDirectionChange();
            }
        }

        if (currentState == GameState.end)
        {
            //Calculate data
            CalculateIdData();

            //Store data
            dataController.GetComponent<DataController>().UpdateIdInfo(gameID, averageTimeDriving, bestTimeDriving, worstTimeDriving, reactionTimesDriving, tapPositionsDriving, timeSpentPlayingDriving);

            if (Input.GetMouseButtonDown(0))
            {
                //ResetGame();
                ReturnToMenu();
            }
        }
    }

    void OnGUI()
    {
        if (currentState == GameState.start)
        {
            if (currentCount > 0)
            {
                countdownText.text = currentCount.ToString();
            }
            else if (currentCount == 0)
            {
                countdownText.text = "Go!";
            }
        }

        if (currentState == GameState.play)
        {
            if (timerText == null)
            {
                Debug.Log("Timer Text is null.");
            }
            else
            {
                timerText.text = "Time: " + GetTimer((int)timer);
            }
        }

        if (currentState == GameState.end)
        {
            gameOverText.enabled = true;
            gameOverText.text = "Game Over";
            timerText.enabled = false;
            directionText.enabled = false;
        }
    }

    void CheckForDirectionChange()
    {
        changesSinceLastSeg = Track.GetComponent<TrackController>().GetChangesSinceLastSeg();
        if (currentSegment < 15 && changesSinceLastSeg >= spawnLengths[currentSegment])
        {
            //Change the road segment using ChangeDirection() and giving road type variables
            //using randomly generated direction (array similar to time segments)

            Track.GetComponent<TrackController>().ChangeDirection(directionTypes[currentSegment]);
 
            //Notify player about direction change
            //Allow input for the car as soon as stimuli is shown
            //Once input is detected, block any other inputs

            Car.GetComponent<CarController>().UpdateDiection(directionTypes[currentSegment]);

            currentSegment++;
        }
    }

    void CalculateIdData()
    {
        for (int i = 0; i < listReactionTimes.Count; i++)
        {
            if (listReactionTimes[i] > worstTimeDriving)
            {
                worstTimeDriving = listReactionTimes[i];
            }
            else if (listReactionTimes[i] < bestTimeDriving || i == 0)
            {
                bestTimeDriving = listReactionTimes[i];
            }

            averageTimeDriving += listReactionTimes[i];
        }

        //Calculate the new average time
        averageTimeDriving /= listReactionTimes.Count;

        //Convert lists to arrays
        reactionTimesDriving = listReactionTimes.ToArray();
        tapPositionsDriving = listTapPositions.ToArray();
    }

    public void SetCurrentReactionTimeValue(bool reactionHasHappened)
    {
        if (!reactionHasHappened)
        {
            //Reset reaction time
            currentReactionTime = 0;
        }
        else
        {
            //Store reaction time
            listReactionTimes.Add(currentReactionTime);
        }
    }

    public void AddTapPositionToList(Vector2 newPos)
    {
        listTapPositions.Add(newPos);
    }

    void RandomiseSpawnTimes()
    {
        for (int i = 0; i < spawnLengths.Length; i++)
        {
            int tmp = spawnLengths[i];
            int r = Random.Range(i, spawnLengths.Length);
            spawnLengths[i] = spawnLengths[r];
            spawnLengths[r] = tmp;
        }
    }

    void RandomiseDirections()
    {
        for (int i = 0; i < directionTypes.Length; i++)
        {
            string tmp = directionTypes[i];
            int r = Random.Range(i, directionTypes.Length);
            directionTypes[i] = directionTypes[r];
            directionTypes[r] = tmp;
        }
    }

    string GetTimer(int s)
    {
        int mins;
        int secs;
        string minsStr;
        string secsStr;

        mins = s / 60;
        secs = s % 60;

        if (mins > 0)
        {
            minsStr = mins.ToString();
        }
        else
        {
            minsStr = "0";
        }
        if (secs < 10)
        {
            secsStr = "0" + secs.ToString();
        }
        else
        {
            secsStr = secs.ToString();
        }

        return minsStr + ":" + secsStr;
    }

    public GameState GetGameState()
    {
        return currentState;
    }

    public void SetGameState(GameState newState)
    {
        currentState = newState; 
    }

    void ResetGame()
    {
        RandomiseSpawnTimes();
        RandomiseDirections();

        currentCount = 3;
        currentState = GameState.start;
        currentTime = 0.0f;        

        countdownText.enabled = true;
        timerText.enabled = false;
        gameOverText.enabled = false;
        directionText.enabled = false;
    }

    public void DisplayDirection(string dir)
    {
        directionText.text = dir;
    }

    public void SetDirectionActive(bool val)
    {
        directionText.enabled = val;
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene("Title Scene");
    }
}
