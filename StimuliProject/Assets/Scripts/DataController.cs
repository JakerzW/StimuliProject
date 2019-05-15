using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataController : MonoBehaviour
{
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

    List<IDInformation> allIds = new List<IDInformation>();

    int numOfIds;
    int currentId;

    public enum GameID { shoot, drive, dodge };
    public GameID currentGame;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        numOfIds = PlayerPrefs.GetInt("NumOfIds");
        for (int i = 0; i < numOfIds; i++)
        {
            AddId();
        }
        Load();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<IDInformation> GetAllIds()
    {
        return allIds;
    }

    public List<string> GetAllIdStrings()
    {
        List<string> allIdStr = new List<string>();
        for (int i = 0; i < allIds.Count; i++)
        {
            allIdStr.Add("ID: " + allIds[i].id.ToString());
        }
        return allIdStr;
    }

    public int GetCurrentId()
    {
        return currentId;
    }

    public void SetCurrentId(int newId)
    {
        currentId = newId;
    }

    void AddId()
    {
        allIds.Add(new IDInformation());
    }

    public void AddNewId()
    {
        if (PlayerPrefs.GetInt("NumOfIds") == 0)
        {
            PlayerPrefs.SetInt("NumOfIds", 1);
            IDInformation newId = new IDInformation(0);
            allIds.Add(newId);
            currentId = 0;
            Save();
        }

        else
        {
            PlayerPrefs.SetInt("NumOfIds", PlayerPrefs.GetInt("NumOfIds") + 1);
            IDInformation newId = new IDInformation(allIds[allIds.Count - 1].id + 1);
            allIds.Add(newId);
            currentId = newId.id;
            Save();
        }
    }

    public void UpdateIdInfo(GameID game, float av, float best, float worst, float[] times, Vector2[] positions, float playTime)
    {
        switch (game)
        {
            case GameID.shoot:
            {
                allIds[currentId].averageTimeShooting = av;
                allIds[currentId].bestTimeShooting = best;
                allIds[currentId].worstTimeShooting = worst;
                allIds[currentId].reactionTimesShooting = times;
                allIds[currentId].tapPositionsShooting = positions;
                allIds[currentId].timeSpentPlayingShooting = playTime;
                break;
            }
            case GameID.drive:
            {
                allIds[currentId].averageTimeDriving = av;
                allIds[currentId].bestTimeDriving = best;
                allIds[currentId].worstTimeDriving = worst;
                allIds[currentId].reactionTimesDriving = times;
                allIds[currentId].tapPositionsDriving = positions;
                allIds[currentId].timeSpentPlayingDriving = playTime;
                break;
            }
            case GameID.dodge:
            {
                allIds[currentId].averageTimeDodging = av;
                allIds[currentId].bestTimeDodging = best;
                allIds[currentId].worstTimeDodging = worst;
                allIds[currentId].reactionTimesDodging = times;
                allIds[currentId].tapPositionsDodging = positions;
                allIds[currentId].timeSpentPlayingDodging = playTime;
                break;
            }
            default:
                break;
        }

        Save();
    }

    public void Load()
    {
        for (int i = 0; i < allIds.Count; i++)
        {
            string json = File.ReadAllText(Application.dataPath + "/ID Data/ID_" + i + ".json");
            allIds[i] = JsonUtility.FromJson<IDInformation>(json);
        }
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(allIds[currentId]);
        File.WriteAllText(Application.dataPath + "/ID Data/ID_" + allIds[currentId].id + ".json", json);
        Debug.Log("Writing json file.");
        Debug.Log(allIds[currentId].id);
    }
}
