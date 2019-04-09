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
    public GameObject TrackRightTurn;
    public GameObject TrackRightStraight;
    public GameObject TrackForkSplit;
    public GameObject TrackForkStraight;
    public GameObject TrackForkJoin;

    //Create variables for current and next background, and all the current tracks
    GameObject cBG, nBG;
    GameObject cTrack1, cTrack2, cTrack3, cTrack4, cTrack5, nTrack;

    //Variables for tile spacings
    float roadSpacing = 38.1f;
    float bgSpacing = 300f;

    //Variables for changing tracks ande background
    float cumuTrackPos, deltaTrackPos, lastTrackPos, currentTrackPos;
    float cumuBGPos, deltaBGPos, lastBGPos, currentBGPos;
    bool trackNeedsChanging = false;
    bool bgNeedsChanging = false;

    //Variables for speed
    public float speed = 10f;

    // Use this for initialization
    void Start ()
    {
        InitTrack();
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
        SpaceTracks(cTrack3, cTrack2);

        cTrack4 = Instantiate(TrackStraight, gameObject.transform);
        SpaceTracks(cTrack4, cTrack3);
        
    }

    void SpaceTracks(GameObject curTrack, GameObject prevTrack)
    {
        curTrack.transform.localPosition = new Vector3(prevTrack.transform.localPosition.x, prevTrack.transform.localPosition.y + roadSpacing, prevTrack.transform.localPosition.z);
    }

    void SpaceBG(GameObject curBG, GameObject prevBG)
    {
        curBG.transform.localPosition = new Vector3(prevBG.transform.localPosition.x, prevBG.transform.localPosition.y + bgSpacing, prevBG.transform.localPosition.z);
    }

    void UpdateTracks()
    {
        gameObject.transform.Translate(Vector3.down * speed * Time.deltaTime);

        currentTrackPos = gameObject.transform.position.y;
        deltaTrackPos = lastTrackPos - currentTrackPos;
        cumuTrackPos += deltaTrackPos;
        lastTrackPos = currentTrackPos;

        if (cumuTrackPos >= roadSpacing)
        {
            cumuTrackPos = 0f;
            trackNeedsChanging = true;
        }

        if (trackNeedsChanging)
        {
            Destroy(cTrack1);
            cTrack1 = cTrack2;
            cTrack2 = cTrack3;
            cTrack3 = cTrack4;

            cTrack4 = Instantiate(TrackStraight, gameObject.transform);
            SpaceTracks(cTrack4, cTrack3);
            trackNeedsChanging = false;
        }

        currentBGPos = gameObject.transform.position.y;
        deltaBGPos = lastBGPos - currentBGPos;
        cumuBGPos += deltaBGPos;
        lastBGPos = currentBGPos;

        if (cumuBGPos >= bgSpacing)
        {
            cumuBGPos = 0f;
            bgNeedsChanging = true;
        }

        if (bgNeedsChanging)
        {
            Destroy(cBG);
            cBG = nBG;

            nBG = Instantiate(Background, gameObject.transform);
            SpaceBG(nBG, cBG);
            bgNeedsChanging = false;
        }

    }
}
