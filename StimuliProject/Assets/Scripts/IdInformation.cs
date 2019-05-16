using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDInformation
{
    public IDInformation(int newId)
    {
        id = newId;
    }

    public IDInformation() { }

    public int id;

    public float averageTimeShooting;
    public float bestTimeShooting;
    public float worstTimeShooting;
    public float[] reactionTimesShooting;
    public Vector2[] tapPositionsShooting;
    public float timeSpentPlayingShooting;

    public float averageTimeDriving;
    public float bestTimeDriving;
    public float worstTimeDriving;
    public float[] reactionTimesDriving;
    public Vector2[] tapPositionsDriving;
    public float timeSpentPlayingDriving;

    public float averageTimeDodging;
    public float bestTimeDodging;
    public float worstTimeDodging;
    public float[] reactionTimesDodging;
    public Vector2[] tapPositionsDodging;
    public float timeSpentPlayingDodging;
}
