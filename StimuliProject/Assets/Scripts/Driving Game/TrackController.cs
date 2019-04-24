using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour
{    
    public GameObject Background;
    public GameObject TrackStartBlock;
    public GameObject TrackStraight;
    public GameObject TrackLeftTurn;
    public GameObject TrackLeftStraight;
    public GameObject TrackLeftReturn;
    public GameObject TrackRightTurn;
    public GameObject TrackRightStraight;
    public GameObject TrackRightReturn;
    public GameObject TrackSplitFork;
    public GameObject TrackSplitStraight;
    public GameObject TrackSplitJoin;

    //Variables for track position state
    enum TrackPosition {left, right, split, center};
    TrackPosition currentPosition = TrackPosition.center;

    //Create variables for current and next background, and all the current tracks
    GameObject cBG, nBG;
    GameObject cTrack1, cTrack2, cTrack3, cTrack4, nTrack, dTrack;

    //Variables for tile spacings
    float roadSpacing = 38.1f;
    float bgSpacing = 300f;
    float returnSpacing = 19.05f;

    //Variables for changing tracks and background
    float cumuTrackPos, deltaTrackPos, lastTrackPos, currentTrackPos;
    float cumuBGPos, deltaBGPos, lastBGPos, currentBGPos;
    bool directionChange = false;
    bool trackNeedsChanging = false;
    bool bgNeedsChanging = false;
    int changesSinceLastSeg = 0;
    int changesInCurDirection = 0;
    int currentSegment = 0;
    int[] segmentLengths = {3, 3, 3, 3, 3, 6, 6, 6, 6, 6, 9, 9, 9, 9, 9};

    //Variables for speed
    public float speed = 30f;    

    //Variables for game states to organise spawning positions

    // Use this for initialization
    void Start ()
    {
        InitTrack();
        RandomiseSegmentLengths();
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateTracks();
	}

    public void InitTrack()
    {
        cBG = Instantiate(Background, gameObject.transform);
        cBG.transform.position = new Vector3(0f, 0f, 1.5f);

        nBG = Instantiate(Background, gameObject.transform);
        nBG.transform.position = new Vector3(0f, 300f, 1.5f);

        cTrack1 = Instantiate(TrackStartBlock, gameObject.transform);
        cTrack1.transform.position = new Vector3(0f, -30f, 1f);

        cTrack2 = Instantiate(TrackStraight, gameObject.transform);
        cTrack2.transform.position = new Vector3(0f, 8.1f, 1f);

        cTrack3 = Instantiate(TrackStraight, gameObject.transform);
        cTrack3.transform.position = new Vector3(0f, 46.2f, 1f);

        cTrack4 = Instantiate(TrackStraight, gameObject.transform);
        cTrack4.transform.position = new Vector3(0f, 84.3f, 1f);

        nTrack = Instantiate(TrackStraight, gameObject.transform);
        nTrack.transform.position = new Vector3(0f, 122.4f, 1f);
    }

    void SpaceTracks(GameObject curTrack, GameObject prevTrack)
    {
        curTrack.transform.localPosition = new Vector3(curTrack.transform.localPosition.x, prevTrack.transform.localPosition.y + roadSpacing, curTrack.transform.localPosition.z);
    }

    void SpaceBG(GameObject curBG, GameObject prevBG)
    {
        curBG.transform.localPosition = new Vector3(prevBG.transform.localPosition.x, prevBG.transform.localPosition.y + bgSpacing, prevBG.transform.localPosition.z);
    }

    void UpdateTracks()
    {
        //Move the tracks
        gameObject.transform.Translate(Vector3.down * speed * Time.deltaTime);

        //Calculate the current track spacing
        currentTrackPos = gameObject.transform.position.y;
        deltaTrackPos = lastTrackPos - currentTrackPos;
        cumuTrackPos += deltaTrackPos;
        lastTrackPos = currentTrackPos;

        //Check for track spacing and amend variables
        if (cumuTrackPos >= roadSpacing)
        {
            cumuTrackPos = 0f;
            trackNeedsChanging = true;
        }

        //Change the tracks if needed
        if (trackNeedsChanging)
        {
            //Shift up the tracks
            Destroy(cTrack1);
            cTrack1 = cTrack2;
            cTrack2 = cTrack3;
            cTrack3 = cTrack4;
            cTrack4 = nTrack;

            //Create a new track
            //Check if dTrack is empty, if yes create a new object, if not use the nTrack and then delete it to clear it
            if (!directionChange)
            {
                switch (currentPosition)
                {
                    case TrackPosition.left:
                    {
                        if (changesInCurDirection >= segmentLengths[currentSegment])
                        {
                            nTrack = Instantiate(TrackLeftReturn, gameObject.transform, true);
                            changesInCurDirection = 0;
                            currentPosition = TrackPosition.center;
                        }
                        else
                        {
                            nTrack = Instantiate(TrackLeftStraight, gameObject.transform, true);
                            changesInCurDirection++;
                        }
                        break;
                    }
                    case TrackPosition.right:
                    {
                        if (changesInCurDirection >= segmentLengths[currentSegment])
                        {
                            nTrack = Instantiate(TrackRightReturn, gameObject.transform, true);
                            changesInCurDirection = 0;
                            currentPosition = TrackPosition.center;
                        }
                        else
                        {
                            nTrack = Instantiate(TrackRightStraight, gameObject.transform, true);
                            changesInCurDirection++;
                        }
                        break;
                    }
                    case TrackPosition.split:
                    {
                        if (changesInCurDirection >= segmentLengths[currentSegment])
                        {
                            nTrack = Instantiate(TrackSplitJoin, gameObject.transform, true);
                            changesInCurDirection = 0;
                            currentPosition = TrackPosition.center;
                        }
                        else
                        {
                            nTrack = Instantiate(TrackSplitStraight, gameObject.transform, true);
                            changesInCurDirection++;
                        }
                        break;
                    }
                    case TrackPosition.center:
                    {
                        nTrack = Instantiate(TrackStraight, gameObject.transform, true);
                        changesSinceLastSeg++;
                        break;
                    }
                }
            }
            else if (directionChange)
            {
                switch (currentPosition)
                {
                    case TrackPosition.left:
                    {
                        nTrack = Instantiate(TrackLeftTurn, gameObject.transform, true);
                        break;
                    }
                    case TrackPosition.right:
                    {
                        nTrack = Instantiate(TrackRightTurn, gameObject.transform, true);
                        break;
                    }
                    case TrackPosition.split:
                    {
                        nTrack = Instantiate(TrackSplitFork, gameObject.transform, true);
                        break;
                    }
                }
                directionChange = false;
            }

            SpaceTracks(nTrack, cTrack4);

            //Amend the class variables 
            trackNeedsChanging = false;
        }

        //Calculate current background spacing
        currentBGPos = gameObject.transform.position.y;
        deltaBGPos = lastBGPos - currentBGPos;
        cumuBGPos += deltaBGPos;
        lastBGPos = currentBGPos;

        //Check for background spacing and amend variables
        if (cumuBGPos >= bgSpacing)
        {
            cumuBGPos = 0f;
            bgNeedsChanging = true;
        }

        //Change the background if needed
        if (bgNeedsChanging)
        {
            //Shift the backgrounds
            Destroy(cBG);
            cBG = nBG;

            //Create a new background
            nBG = Instantiate(Background, gameObject.transform);
            SpaceBG(nBG, cBG);
            bgNeedsChanging = false;
        }
    }    

    public void SetChangesSinceLastSeg(int newCount)
    {
        changesSinceLastSeg = newCount;
    }

    public int GetChangesSinceLastSeg()
    {
        return changesSinceLastSeg;
    }

    public void ChangeDirection(string type)
    {
        //Spawn the road given in the position given and queue in nTrack
        switch (type)
        {
            case "Left":
            {
                //dTrack = Instantiate(TrackLeftTurn, gameObject.transform, true);
                //SpaceTracks(dTrack, cTrack4);
                directionChange = true;
                currentPosition = TrackPosition.left;
                changesSinceLastSeg = 0;
                break;
            }
            case "Right":
            {
                //dTrack = Instantiate(TrackRightTurn, gameObject.transform, true);
                //SpaceTracks(dTrack, cTrack4);
                directionChange = true;
                currentPosition = TrackPosition.right;
                changesSinceLastSeg = 0;
                break;
            }
            case "Split":
            {
                //dTrack = Instantiate(TrackSplitFork, gameObject.transform, true);
                //SpaceTracks(dTrack, cTrack4);
                directionChange = true;
                currentPosition = TrackPosition.split;
                changesSinceLastSeg = 0;
                break;
            }
            default:
                break;
        }
        currentSegment++;
    }
    void RandomiseSegmentLengths()
    {
        for (int i = 0; i < segmentLengths.Length; i++)
        {
            int tmp = segmentLengths[i];
            int r = Random.Range(i, segmentLengths.Length);
            segmentLengths[i] = segmentLengths[r];
            segmentLengths[r] = tmp;
        }
    }
}
