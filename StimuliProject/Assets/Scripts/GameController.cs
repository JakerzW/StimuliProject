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
	public float timer;

    int score;
    public Text scoreText;
    public Text countdownText;
	public Text timerText;

	public GameObject Target1;
	public GameObject Target2;
	public GameObject Target3;

    public int maxTargets;
	bool targetsActive;
	bool spawnStarted;
	float maxSpawnTime;
	float spawnTimer;
	public int maxValX;
	public int maxValY;

    void Awake()
    {
        score = 0;
        currentState = GameState.start;
        currentTime = 0.0f;
        currentCount = 3;
		maxSpawnTime = 3.0f;

		if (timer == 0)
		{
			timer = 60;
		}
        
		countdownText.enabled = true;
        scoreText.enabled = false;
		timerText.enabled = false;

		targetsActive = false;
    }

    // Use this for initialization
    void Start ()
    {
        countdownText.text = currentCount.ToString();
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
                scoreText.enabled = true;
				timerText.enabled = true;
				currentState = GameState.spawn;
            }         
        }

        if (currentState == GameState.wait || currentState == GameState.spawn)
        {
			timer -= Time.deltaTime;
            UpdateTargets();

			if (timer <= 0)
			{
				timer = 0;
				currentState = GameState.end;
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
			if (timerText == null)
			{
				Debug.Log ("Timer Text is null.");
			}
			else
			{
				timerText.text = "Time: " + GetTimer((int)timer);
			}
        }        
    }

	//Called when a target has been hit to reset targets
	public void TargetHit()
	{
		currentState = GameState.spawn;
		targetsActive = false;

		score++;

		//Fade out other targets
		GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
		for(int i = 0; i < targets.Length; i++)
		{
			Destroy(targets[i]);
		}

		//Set player reaction timer to 0 and record time

		UpdateTargets();
	}

	//Update the current state of the targets
    void UpdateTargets()
    {
		//Chech if spawning is ready and has started
		if (currentState == GameState.spawn && !spawnStarted)
		{
			//Set the spawn timer
			spawnTimer = Random.Range(0.0f, maxSpawnTime);
			spawnStarted = true;
		}
		//If spawning has started then decrease timer until 0 and then spawn targets
		if (currentState == GameState.spawn && spawnStarted)
		{
			spawnTimer -= Time.deltaTime;

			if (spawnTimer <= 0)
			{
				CreateTargets();
				spawnStarted = false;
			}
		}
    }

	//Create targets and assign relative variables
	void CreateTargets()
	{
		targetsActive = true;
		currentState = GameState.wait;

		int numOfTargets = Random.Range(1, maxTargets);

		for (int i = 0; i < numOfTargets; i++)
		{
			int targetChoice = Random.Range(1, 3);
			switch (targetChoice)
			{
				case 1:
				{
					Instantiate(Target1, new Vector3(Random.Range(-maxValX, maxValX), Random.Range(-maxValY, maxValY), 0), Quaternion.identity);
					break;
				}
				case 2:	
				{
					Instantiate(Target2, new Vector3(Random.Range(-maxValX, maxValX), Random.Range(-maxValY, maxValY), 0), Quaternion.identity);
					break;
				}
				case 3:	
				{
					Instantiate(Target3, new Vector3(Random.Range(-maxValX, maxValX), Random.Range(-maxValY, maxValY), 0), Quaternion.identity);
					break;
				}
			}
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
}
