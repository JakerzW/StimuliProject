using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public int score;
    public Text scoreText;

    void Awake()
    {
        score = 0;
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
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

    public void IncrementScore()
    {
        score++;
    }
}
