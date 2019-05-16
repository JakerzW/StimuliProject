using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDInformation
{
    //Construct the ID with its number
    public IDInformation(int newId)
    {
        id = newId;
    }

    public IDInformation() { }

    //ID number
    public int id;

    //Shooting game variables
    public float averageTimeShooting;
    public float bestTimeShooting;
    public float worstTimeShooting;
    public float[] reactionTimesShooting;
    public Vector2[] tapPositionsShooting;
    public float timeSpentPlayingShooting;

    //Driving game variables
    public float averageTimeDriving;
    public float bestTimeDriving;
    public float worstTimeDriving;
    public float[] reactionTimesDriving;
    public Vector2[] tapPositionsDriving;
    public float timeSpentPlayingDriving;

    //Dodging game variables
    public float averageTimeDodging;
    public float bestTimeDodging;
    public float worstTimeDodging;
    public float[] reactionTimesDodging;
    public Vector2[] tapPositionsDodging;
    public float timeSpentPlayingDodging;
}
