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
        LoadDropdownValues();
    }

    private void Update()
    {
        UpdateIdButtons();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }	

    public void AddNewId()
    {
        dataController.AddNewId();
        idButtonsShowing = false;
    }

    public void ShowIDButtons()
    {
        LoadDropdownValues();

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
        idButton.GetComponentInChildren<Text>().text = "ID: " + dataController.GetCurrentId();

        fadeImage.gameObject.SetActive(idButtonsShowing);
        chooseId.gameObject.SetActive(idButtonsShowing);
        addId.gameObject.SetActive(idButtonsShowing);
    }
    
    void LoadDropdownValues()
    {
        chooseId.ClearOptions();
        chooseId.AddOptions(dataController.GetAllIdStrings());
        chooseId.captionText.text = "ID: " + dataController.GetCurrentId();
    }

    public void ChooseCurrentID()
    {
        dataController.SetCurrentId(chooseId.value);
        idButton.GetComponentInChildren<Text>().text = "ID: " + chooseId.value;
        //chooseId.Hide();
        idButtonsShowing = false;
    }
}
