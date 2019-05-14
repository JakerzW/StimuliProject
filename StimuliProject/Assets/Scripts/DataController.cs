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
        public float averageTimeDriving;
        public float bestTimeDriving;
        public float worstTimeDriving;
        public float averageTimeDodging;
        public float bestTimeDodging;
        public float worstTimeDodging;
    }

    List<IDInformation> allIds = new List<IDInformation>();

    int numOfIds;
    int currentId;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        numOfIds = PlayerPrefs.GetInt("NumOfIds");
        for (int i = 0; i < numOfIds; i++)
        {
            AddId();
        }
        ReadJsonFiles();
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
            AmmendJsonFile();
        }

        else
        {
            PlayerPrefs.SetInt("NumOfIds", PlayerPrefs.GetInt("NumOfIds") + 1);
            IDInformation newId = new IDInformation(allIds[allIds.Count - 1].id + 1);
            allIds.Add(newId);
            currentId = newId.id;
            AmmendJsonFile();
        }
    }

    public void UpdateIdInfo(string game, int av, int best, int worst)
    {
        switch (game)
        {
            case "Shooting":
            {
                allIds[currentId].averageTimeShooting = av;
                allIds[currentId].bestTimeShooting = best;
                allIds[currentId].worstTimeShooting = worst;
                break;
            }
            case "Driving":
            {
                allIds[currentId].averageTimeDriving = av;
                allIds[currentId].bestTimeDriving = best;
                allIds[currentId].worstTimeDriving = worst;
                break;
            }
            case "Dodging":
            {
                allIds[currentId].averageTimeDodging = av;
                allIds[currentId].bestTimeDodging = best;
                allIds[currentId].worstTimeDodging = worst;
                break;
            }
            default:
            {
                break;
            }
        }

        AmmendJsonFile();
    }

    public void ReadJsonFiles()
    {
        for (int i = 0; i < allIds.Count; i++)
        {
            string json = File.ReadAllText(Application.dataPath + "/ID Data/ID_" + i + ".json");
            allIds[i] = JsonUtility.FromJson<IDInformation>(json);
        }
    }

    public void AmmendJsonFile()
    {
        string json = JsonUtility.ToJson(allIds[currentId]);
        File.WriteAllText(Application.dataPath + "/ID Data/ID_" + allIds[currentId].id + ".json", json);
        Debug.Log("Writing json file.");
        Debug.Log(allIds[currentId].id);
    }
}
