using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenButtonController : MonoBehaviour
{
    public int[] Ids;

    public DataController dataController;

    public Image fadeImage;
    public Dropdown chooseId;
    public Button addId;
    public Button shootButton;
    public Button driveButton;
    public Button dodgeButton;
    public Button idButton;

    bool idButtonsShowing;

    private void Start()
    {
        idButtonsShowing = false;
    }

    private void Update()
    {
        UpdateIdButtons();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }	

    public void ChooseID(int newId)
    {
        dataController.SetCurrentId(newId);
    }

    public void AddId()
    {
        dataController.AddNewId();
        idButtonsShowing = false;
    }

    public void ShowIDButtons()
    {
        if (idButtonsShowing)
        {
            idButtonsShowing = false;
        }
        else
        {
            idButtonsShowing = true;
        }
    }

    void UpdateIdButtons()
    {
        fadeImage.gameObject.SetActive(idButtonsShowing);
        chooseId.gameObject.SetActive(idButtonsShowing);
        addId.gameObject.SetActive(idButtonsShowing);
    }
    
}
