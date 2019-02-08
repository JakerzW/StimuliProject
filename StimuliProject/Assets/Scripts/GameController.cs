using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    enum GameState {start, wait, spawn, end};
    GameState currentState;
    
    float currentTime;
    int currentCount;

    public int score;
    public Text scoreText;
    public Text countdownText;

    public int maxTargets;

    void Awake()
    {
        score = 0;
        currentState = GameState.start;
        currentTime = 0.0f;
        currentCount = 3;

        countdownText.enabled = true;
        scoreText.enabled = false;
    }

    // Use this for initialization
    void Start ()
    {
        countdownText.text = currentCount.ToString();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (currentState == GameState.start)
        {
            currentTime += Time.deltaTime;

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
                scoreText.enabled = true;
                currentState = GameState.spawn;
            }         
        }

        if (currentState == GameState.wait || currentState == GameState.spawn)
        {
            updateTargets();
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
        
        if (currentState == GameState.wait || currentState == GameState.spawn)
        {
            if (scoreText == null)
            {
                Debug.Log("Score Text is null.");
            }
            else
            {
                scoreText.text = "Score: " + score;
            }
        }        
    }

    public void IncrementScore()
    {
        score++;
    }

    void updateTargets()
    {

    }
}
