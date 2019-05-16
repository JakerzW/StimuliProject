using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataController : MonoBehaviour
{ 
    //List of all the IDs
    List<IDInformation> allIds = new List<IDInformation>();

    //Data variables
    int numOfIds;
    public int currentId;

    //Current gameID enum
    public enum GameID { shoot, drive, dodge };

    //Call this function first
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("DataController");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        numOfIds = PlayerPrefs.GetInt("NumOfIds");
        for (int i = 0; i < numOfIds; i++)
        {
            AddId();
        }
        Load();
    }

    //Return all the IDs
    public List<IDInformation> GetAllIds()
    {
        return allIds;
    }

    //Get all the ID strings
    public List<string> GetAllIdStrings()
    {
        List<string> allIdStr = new List<string>();
        for (int i = 0; i < allIds.Count; i++)
        {
            allIdStr.Add("ID: " + allIds[i].id.ToString());
        }
        return allIdStr;
    }

    //Get the current ID
    public int GetCurrentId()
    {
        return currentId;
    }

    //Set the current ID
    public void SetCurrentId(int newId)
    {
        currentId = newId;
    }

    //Add an ID to the list
    void AddId()
    {
        allIds.Add(new IDInformation());
    }

    //Add a new ID
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

    //Update the ID with the info given
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

    //Load the ID info from the associated json file
    public void Load()
    {
        for (int i = 0; i < allIds.Count; i++)
        {
            string filePath = Path.Combine(Application.persistentDataPath, "ID_" + i + ".json");
            string json = File.ReadAllText(filePath);
            allIds[i] = JsonUtility.FromJson<IDInformation>(json);
        }        
    }
    //Save the ID info into the associated json file
    public void Save()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "ID_" + allIds[currentId].id + ".json");
        string json = JsonUtility.ToJson(allIds[currentId]);
        File.WriteAllText(filePath, json);
        Debug.Log("Saving to json file.");
    }

    //Clear all the data
    public void ClearAllData()
    {
        Directory.Delete(Application.dataPath + "/ID Data/");
        PlayerPrefs.DeleteAll();
    }    
}
