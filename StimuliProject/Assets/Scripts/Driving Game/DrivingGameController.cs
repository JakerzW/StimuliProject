using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingGameController : MonoBehaviour {

    //Variables for spawning new road segment types
    int[] spawnLengths = { 2, 2, 2, 2, 2, 5, 5, 5, 5, 5, 8, 8, 8, 8, 8 };
    string[] directionTypes = { "Left", "Left", "Left", "Left", "Left", "Right", "Right", "Right", "Right", "Right", "Split", "Split", "Split", "Split", "Split" };
    int currentSegment = 0;
    int changesSinceLastSeg = 0;
    public GameObject Track;

    // Use this for initialization
    void Start ()
    {
        RandomiseSpawnTimes();
        RandomiseDirections();
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckForDirectionChange();
    }

    void CheckForDirectionChange()
    {
        changesSinceLastSeg = Track.GetComponent<TrackController>().GetChangesSinceLastSeg();
        if (changesSinceLastSeg >= spawnLengths[currentSegment])
        {
            //Change the road segment using ChangeDirection() and giving road type variables
            //using randomly generated direction (array similar to time segments)

            Track.GetComponent<TrackController>().ChangeDirection(directionTypes[currentSegment]);
            currentSegment++;
        }
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
}
