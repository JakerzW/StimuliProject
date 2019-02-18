using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //Screen size defined
    int screenWidth, screenHeight;

    //Game states defined
    enum GameState {start, wait, spawn, end};
    GameState currentState;
    
    //Game timer variables defined
    float currentTime;
    int currentCount;
	public float timer;

    //Score variables defined
    int score;
    public Text scoreText, countdownText, timerText;

    //Referencing the target objects
	public GameObject Target1, Target2, Target3;

    //Target spawning variables defined
    bool targetsActive;
    public int maxTargets;
	bool spawnStarted;
	float maxSpawnTime;
	float spawnTimer;
	public int maxValX, maxValY;
    int singleSpawns, doubleSpawns, tripleSpawns;

    //Target spawn location struct
    struct Locations
    {
        public bool used;
        public float xPos, yPos;
    };

    Locations[] allLocations;

    void Awake()
    {
        Screen.orientation = ScreenOrientation.Landscape;

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        score = 0;
        currentState = GameState.start;
        currentTime = 0.0f;
        currentCount = 3;
		maxSpawnTime = 3.0f;

        singleSpawns = 8;
        doubleSpawns = 6;
        tripleSpawns = 4;

        allLocations = new Locations[18];

        for (int i = 0; i < allLocations.Length; i++)
        {
            allLocations[i].used = false;

            //Assign x positions
            if (i == 0 || i == 6 || i == 12)
            {
                allLocations[i].xPos = -50;
            }
            else if (i == 1 || i == 7 || i == 13)
            {
                allLocations[i].xPos = -30;
            }
            else if (i == 2 || i == 8 || i == 14)
            {
                allLocations[i].xPos = -10;
            }
            else if (i == 3 || i == 9 || i == 15)
            {
                allLocations[i].xPos = 10;
            }
            else if (i == 4 || i == 10 || i == 16)
            {
                allLocations[i].xPos = 30;
            }
            else if (i == 5 || i == 11 || i == 17)
            {
                allLocations[i].xPos = 50;
            }

            //Assign y positions
            if (i < 6)
            {
                allLocations[i].yPos = 14;
            }
            else if (i < 12 && i >= 6)
            {
                allLocations[i].yPos = 0;
            }
            else if (i >= 12)
            {
                allLocations[i].yPos = -14;
            }
        }

		if (timer == 0)
		{
			timer = 60;
		}
        
		countdownText.enabled = true;
        countdownText.fontSize = 200;
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

            if (score == 18)
            {
                currentState = GameState.end;
            }
        }

        if (currentState == GameState.end)
        {
            countdownText.enabled = true;
            countdownText.fontSize = 100;
            countdownText.text = "Game Over";
            scoreText.enabled = false;
            timerText.enabled = false;
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
	public void TargetHit(GameObject hitTarget)
	{
		currentState = GameState.spawn;
		targetsActive = false;

		score++;

        Debug.Log("Score: " + score);

        hitTarget.GetComponent<TargetController>().Hit(true);

		//Fade out other targets
		GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
		for(int i = 0; i < targets.Length; i++)
		{
            targets[i].GetComponent<CircleCollider2D>().enabled = false;
			targets[i].GetComponent<TargetController>().Hit(false);
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
			spawnTimer = Random.Range(1.0f, maxSpawnTime);
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

        bool locationFound = false;
        

        while (!locationFound && score < 18)
        {
            int newTarget = Random.Range(0, 18);
            if (!allLocations[newTarget].used)
            {
                allLocations[newTarget].used = true;
                locationFound = true;
                Instantiate(Target1, new Vector3(allLocations[newTarget].xPos, allLocations[newTarget].yPos, 0), Quaternion.identity);
            }
        }

        

		/*int numOfTargets = Random.Range(1, maxTargets);

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
		}*/
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
