using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DodgingGameController : MonoBehaviour
{
    //Game timer variables defined
    float currentTime;
    int currentCount;
    public float timer;

    public enum GameState { start, spawn, wait, end };
    GameState currentState;

    public Text countdownText, timerText, gameOverText;

    public GameObject Ship;
    public GameObject meteor1, meteor2, meteor3, meteor4;
    
    int[] spawnType = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3 };
    int currentSpawnType;    

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
                currentState = GameState.spawn;
            }
        }

        if (currentState == GameState.wait || currentState == GameState.spawn)
        {
            timer -= Time.deltaTime;

            if (timer <= 0 || currentSpawnType >= 32)
            {
                timer = 0;
                currentState = GameState.end;
            }

            if (currentState == GameState.wait)
            {
                if (!AreMeteorsExisting())
                {
                    currentState = GameState.spawn;
                }
            }

            else if (currentState == GameState.spawn)            
            {
                CreateMeteors();
                currentState = GameState.wait;
                currentSpawnType++;
            }
        }

        if (currentState == GameState.end)
        {
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

        if (currentState == GameState.wait || currentState == GameState.spawn)
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
        }
    }

    void CreateMeteors()
    {
        Debug.Log("No. Meteors Spawned: " + spawnType[currentSpawnType]);
        switch (spawnType[currentSpawnType])
        {
            case 1:
            {
                int typeOfMeteor = Random.Range(1, 5);
                int posOfMeteor = Random.Range(1, 5);

                GameObject newMeteor1 = SetTypeOfMeteor(typeOfMeteor);
                SetPosOfMeteor(newMeteor1, posOfMeteor);

                break;
            }
            case 2:
            {
                int typeOfMeteor = Random.Range(1, 5);
                int posOfMeteor = Random.Range(1, 5);

                GameObject newMeteor1 = SetTypeOfMeteor(typeOfMeteor);
                SetPosOfMeteor(newMeteor1, posOfMeteor);

                typeOfMeteor++;
                if (typeOfMeteor > 4)
                {
                    typeOfMeteor = 1;
                }

                posOfMeteor++;
                if (posOfMeteor > 4)
                {
                    posOfMeteor = 1;
                }

                GameObject newMeteor2 = SetTypeOfMeteor(typeOfMeteor);
                SetPosOfMeteor(newMeteor2, posOfMeteor);

                break;
            }
            case 3:
            {
                int typeOfMeteor = Random.Range(1, 5);
                int posOfMeteor = Random.Range(1, 5);

                GameObject newMeteor1 = SetTypeOfMeteor(typeOfMeteor);
                SetPosOfMeteor(newMeteor1, posOfMeteor);

                typeOfMeteor++;
                if (typeOfMeteor > 4)
                {
                    typeOfMeteor = 1;
                }

                posOfMeteor++;
                if (posOfMeteor > 4)
                {
                    posOfMeteor = 1;
                }

                GameObject newMeteor2 = SetTypeOfMeteor(typeOfMeteor);
                SetPosOfMeteor(newMeteor2, posOfMeteor);

                typeOfMeteor++;
                if (typeOfMeteor > 4)
                {
                    typeOfMeteor = 1;
                }

                posOfMeteor++;
                if (posOfMeteor > 4)
                {
                    posOfMeteor = 1;
                }

                GameObject newMeteor3 = SetTypeOfMeteor(typeOfMeteor);
                SetPosOfMeteor(newMeteor3, posOfMeteor);

                break;
            }
        }        
    }

    GameObject SetTypeOfMeteor(int type)
    {
        switch (type)
        {
            case 1:
            {
                GameObject meteor = Instantiate(meteor1);
                return meteor;
            }
            case 2:
            {
                GameObject meteor = Instantiate(meteor2);
                return meteor;
            }
            case 3:
            {
                GameObject meteor = Instantiate(meteor3);
                return meteor;
            }
            case 4:
            {
                GameObject meteor = Instantiate(meteor4);
                return meteor;
            }
            default:
            {
                GameObject meteor = Instantiate(meteor1);
                return meteor;
            }
        }
    }

    void SetPosOfMeteor(GameObject meteor, int pos)
    {
        switch (pos)
        {
            case 1:
            {
                meteor.GetComponent<MeteorController>().InitMeteorVals(new Vector2(Ship.transform.position.x, Ship.transform.position.y), "Left");
                break;
            }
            case 2:
            {
                meteor.GetComponent<MeteorController>().InitMeteorVals(new Vector2(Ship.transform.position.x, Ship.transform.position.y), "Right");
                break;
            }
            case 3:
            {
                meteor.GetComponent<MeteorController>().InitMeteorVals(new Vector2(Ship.transform.position.x, Ship.transform.position.y), "Top");
                break;
            }
            case 4:
            {
                meteor.GetComponent<MeteorController>().InitMeteorVals(new Vector2(Ship.transform.position.x, Ship.transform.position.y), "Bottom");
                break;
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

        RandomiseSpawnTypes();
        currentSpawnType = 0;
    }

    bool AreMeteorsExisting()
    {
        GameObject[] existingMeteors;
        existingMeteors = GameObject.FindGameObjectsWithTag("Meteor");
        if (existingMeteors == null || existingMeteors.Length == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void RandomiseSpawnTypes()
    {
        for (int i = 0; i < spawnType.Length; i++)
        {
            int tmp = spawnType[i];
            int r = Random.Range(i, spawnType.Length);
            spawnType[i] = spawnType[r];
            spawnType[r] = tmp;
        }
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene("Title Scene");
    }
}
