using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgingGameController : MonoBehaviour
{
    //Game timer variables defined
    float currentTime;
    int currentCount;
    public float timer;

    public enum GameState { start, play, end };
    GameState currentState;

    public Text countdownText, timerText, gameOverText;

    public GameObject meteor1, meteor2, meteor3, meteor4;

    // Start is called before the first frame update
    void Start()
    {
        ResetGame();
    }

    // Update is called once per frame
    void Update()
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
            }
        }

        if (currentState == GameState.play)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer = 0;
                currentState = GameState.end;
            }
            else
            {
                //Update Game
            }
        }

        if (currentState == GameState.end)
        {
            gameOverText.enabled = true;
            gameOverText.text = "Game Over";
            timerText.enabled = false;

            if (Input.GetMouseButtonDown(0))
            {
                ResetGame();
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
    }
    
    public void SetGameState(GameState newState)
    {
        currentState = newState;
    }

    public GameState GetGameState()
    {
        return currentState;
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

    void ResetGame()
    {
        currentTime = 0f;
        currentCount = 3;

        currentState = GameState.start;

        countdownText.enabled = true;
        timerText.enabled = false;
        gameOverText.enabled = false;
    }
}
