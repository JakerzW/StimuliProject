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
    public int singleSpawns, doubleSpawns, tripleSpawns;

    //Target spawn location struct
    struct Locations
    {
        public bool used;
        public float xPos, yPos;
    };

    Locations[] allLocations = new Locations[18];
    Locations[,] doubleLocations = new Locations[2, 9];
    Locations[,] tripleLocations = new Locations[2, 9];

    void Awake()
    {
        Screen.orientation = ScreenOrientation.Landscape;

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        InitLocations(allLocations);
        InitLocations(doubleLocations);
        InitLocations(tripleLocations);

        ResetGame();
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

            if (maxTargets <= 0)
            {
                currentState = GameState.end;
            }
        }

        if (currentState == GameState.end)
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
            for (int i = 0; i < targets.Length; i++)
            {
                Destroy(targets[i]);
            }

            countdownText.enabled = true;
            countdownText.fontSize = 100;
            countdownText.text = "Game Over";
            scoreText.enabled = false;
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

        //Debug.Log("Score: " + score);

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
        bool locationValid = false;
        bool replacementFound = false;
        int newTarget, newTargetSide, newTargetPos;

        //Debug.Log("Entering Spawn Procedure");
        switch (Random.Range(1, 4))
        {
            case 1:
            {
                Debug.Log("Entering Single Spawn Procedure");
                if (singleSpawns > 0)
                {
                    while (!locationValid)
                    {
                        newTarget = Random.Range(0, 18);
                        if (!allLocations[newTarget].used)
                        {
                            Instantiate(Target1, new Vector3(allLocations[newTarget].xPos, allLocations[newTarget].yPos, 0), Quaternion.identity);
                            allLocations[newTarget].used = true;
                            locationValid = true;
                            maxTargets--;
                            continue;
                        }
                        else
                        {
                            replacementFound = false;
                            for (int i = 0; i < allLocations.Length; i++)
                            {
                                if (!allLocations[i].used)
                                {
                                    Instantiate(Target1, new Vector3(allLocations[newTarget].xPos, allLocations[newTarget].yPos, 0), Quaternion.identity);
                                    allLocations[newTarget].used = true;
                                    locationValid = true;
                                    maxTargets--;
                                    replacementFound = true;
                                    Debug.Log("Replacement Found");
                                    break;
                                }
                            }
                            if (!replacementFound)
                            {
                                Debug.Log("Location for Single Spawn could not be found");
                                Debug.Break();
                            }
                        }
                    }
                }
                singleSpawns--;
                break;
            }
            case 2:
            {
                Debug.Log("Entering Double Spawn Procedure");
                if (doubleSpawns > 0)
                {
                    int targetsCreated = 0;
                    while (!locationValid || targetsCreated < 2)
                    {
                        newTargetSide = Random.Range(0, 2);
                        newTargetPos = Random.Range(0, 9);
                        if (!doubleLocations[newTargetSide, newTargetPos].used)
                        {
                            Instantiate(Target1, new Vector3(doubleLocations[newTargetSide, newTargetPos].xPos, doubleLocations[newTargetSide, newTargetPos].yPos, 0), Quaternion.identity);
                            doubleLocations[newTargetSide, newTargetPos].used = true;                            
                            maxTargets--;
                            locationValid = true;
                            targetsCreated++;
                        }
                        else
                        {
                            replacementFound = false;
                        }
                    }
                }
                doubleSpawns--;
                break;
            }
            case 3:
            {
                Debug.Log("Entering Triple Spawn Procedure");
                if (tripleSpawns > 0)
                {
                    int targetsCreated = 0;
                    while (!locationValid || targetsCreated < 3)
                    {
                        newTarget = Random.Range(0, 18);
                       
                    }
                }
                tripleSpawns--;
                break;
            }
        }
        




        //bool locationFound = false;

        //while (!locationFound && maxTargets > 0)
        //{
        //    int spawnType = Random.Range(1, 4);
        //    switch (spawnType)
        //    {
        //        case 1:
        //        {
        //            if (singleSpawns > 0)
        //            {
        //                Debug.Log("Spawn Number: " + spawnType);
        //                while (!locationFound)
        //                {
        //                    int newTarget = Random.Range(0, 18);
        //                    //Debug.Log("Spawn Location (S): " + newTarget);                           
        //                    if (!allLocations[newTarget].used)
        //                    {
        //                        allLocations[newTarget].used = true;
        //                        Instantiate(Target1, new Vector3(allLocations[newTarget].xPos, allLocations[newTarget].yPos, 0), Quaternion.identity);
        //                        //Debug.Log("Spawning...");
        //                        maxTargets--;
        //                        locationFound = true;
        //                    }
        //                }
        //                singleSpawns--;
        //            }
        //            break;
        //        }
        //        case 2:
        //        {
        //            if (doubleSpawns > 0)
        //            {
        //                Debug.Log("Spawn Number: " + spawnType);

        //                int targetsCreated = 2;
        //                ClearLocations(doubleLocations);

        //                while (!locationFound || targetsCreated > 0)
        //                {
        //                    int newTarget = Random.Range(0, 18);

        //                    if (!doubleLocations[newTarget].used)
        //                    {
        //                        doubleLocations[newTarget].used = true;

        //                        //Block the same side of screen to be used twice for double target locations
        //                        if (doubleLocations[newTarget].xPos > 0)
        //                        {
        //                            for (int i = 0; i < doubleLocations.Length; i++)
        //                            {
        //                                if (doubleLocations[i].xPos > 0)
        //                                {
        //                                    doubleLocations[i].used = true;
        //                                }
        //                            }
        //                        }
        //                        else if (doubleLocations[newTarget].xPos < 0)
        //                        {
        //                            for (int i = 0; i < doubleLocations.Length; i++)
        //                            {
        //                                if (doubleLocations[i].xPos < 0)
        //                                {
        //                                    doubleLocations[i].used = true;
        //                                }
        //                            }
        //                        }

        //                        Instantiate(Target1, new Vector3(doubleLocations[newTarget].xPos, doubleLocations[newTarget].yPos, 0), Quaternion.identity);
        //                        //Debug.Log("Spawning...");                                
        //                        maxTargets--;
        //                        targetsCreated--;
        //                        locationFound = true;                                    
        //                    }
        //                }
        //                doubleSpawns--;
        //            }
        //            break;
        //        }
        //        case 3:
        //        {
        //            if (tripleSpawns > 0)
        //            {
        //                Debug.Log("Spawn Number: " + spawnType);

        //                int targetsCreated = 3;
        //                ClearLocations(tripleLocations);

        //                while (targetsCreated > 0)
        //                {
        //                    int newTarget = Random.Range(0, 18);

        //                    if (!tripleLocations[newTarget].used)
        //                    {                                
        //                        tripleLocations[newTarget].used = true;

        //                        //If there is one target left to be spawned, block the last used to side to ensure there is at least one target on each side
        //                         /*if (targetsCreated == 2)
        //                         {
        //                             if (tripleLocations[newTarget].xPos > 0)
        //                             {
        //                                 for (int i = 0; i < tripleLocations.Length; i++)
        //                                 {
        //                                     if (tripleLocations[i].xPos > 0)
        //                                     {
        //                                         tripleLocations[i].used = true;
        //                                     }
        //                                 }
        //                             }
        //                             else if (tripleLocations[newTarget].xPos < 0)
        //                             {
        //                                 for (int i = 0; i < tripleLocations.Length; i++)
        //                                 {
        //                                     if (tripleLocations[i].xPos < 0)
        //                                     {
        //                                         tripleLocations[i].used = true;
        //                                     }
        //                                 }
        //                             }
        //                         }*/                              

        //                        Instantiate(Target1, new Vector3(tripleLocations[newTarget].xPos, tripleLocations[newTarget].yPos, 0), Quaternion.identity);
        //                        //Debug.Log("Spawning...");
        //                        maxTargets--;
        //                        targetsCreated--;
        //                        locationFound = true; 
        //                    }                           
        //                }
        //                tripleSpawns--;
        //            }
                    
        //            break;
        //        }
        //    }
        //}      
    }

    void ClearLocations(Locations[] loc)
    {
        for (int i = 0; i < loc.Length; i++)
        {
            loc[i].used = false;
        }
    }

    void ClearLocations(Locations[,] loc)
    {
        for (int i = 0; i < loc.GetLength(0); i++)
        {
            loc[0, i].used = false;
        }
        for (int i = 0; i < loc.GetLength(1); i++)
        {
            loc[1, i].used = false;
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

    void ResetGame()
    {
        score = 0;
        currentState = GameState.start;
        currentTime = 0.0f;
        currentCount = 3;
        maxSpawnTime = 3.0f;

        maxTargets = 36;
        singleSpawns = 18;
        doubleSpawns = 9;
        tripleSpawns = 6;

        ClearLocations(allLocations);

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

    void InitLocations(Locations[] loc)
    {
        ClearLocations(loc);

        for (int i = 0; i < loc.Length; i++)
        {
            loc[i].used = false;

            //Assign x positions
            if (i == 0 || i == 6 || i == 12)
            {
                loc[i].xPos = -40;
            }
            else if (i == 1 || i == 7 || i == 13)
            {
                loc[i].xPos = -25;
            }
            else if (i == 2 || i == 8 || i == 14)
            {
                loc[i].xPos = -10;
            }
            else if (i == 3 || i == 9 || i == 15)
            {
                loc[i].xPos = 10;
            }
            else if (i == 4 || i == 10 || i == 16)
            {
                loc[i].xPos = 25;
            }
            else if (i == 5 || i == 11 || i == 17)
            {
                loc[i].xPos = 40;
            }

            //Assign y positions
            if (i < 6)
            {
                loc[i].yPos = 14;
            }
            else if (i < 12 && i >= 6)
            {
                loc[i].yPos = 0;
            }
            else if (i >= 12)
            {
                loc[i].yPos = -14;
            }
        }
    }

    void InitLocations(Locations[,] loc)
    {
        ClearLocations(loc);

        //Assign left side positions
        for (int i = 0; i < loc.GetLength(0); i++)
        {
            if (i == 0 || i == 3 || i == 6)
            {
                loc[0, i].xPos = -40;
            }
            else if (i == 1 || i == 4 || i == 7)
            {
                loc[0, i].xPos = -25;
            }
            else if (i == 2 || i == 5 || i == 8)
            {
                loc[0, i].xPos = -10;
            }

            if (i == 0 || i == 1 || i == 2)
            {
                loc[0, i].yPos = 14;
            }
            if (i == 3 || i == 4 || i == 5)
            {
                loc[0, i].yPos = 0;
            }
            if (i == 6 || i == 7 || i == 8)
            {
                loc[0, i].yPos = -14;
            }
        }

        //Assign right side positions
        for (int i = 0; i < loc.GetLength(1); i++)
        {
            if (i == 0 || i == 3 || i == 6)
            {
                loc[1, i].xPos = 10;
            }
            if (i == 1 || i == 4 || i == 7)
            {
                loc[1, i].xPos = 25;
            }
            if (i == 2 || i == 5 || i == 8)
            {
                loc[1, i].xPos = 40;
            }

            if (i == 0 || i == 1 || i == 2)
            {
                loc[1, i].yPos = 14;
            }
            if (i == 3 || i == 4 || i == 5)
            {
                loc[1, i].yPos = 0;
            }
            if (i == 6 || i == 7 || i == 8)
            {
                loc[1, i].yPos = -14;
            }
        }
    }
}
