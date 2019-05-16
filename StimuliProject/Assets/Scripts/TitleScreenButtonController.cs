using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenButtonController : MonoBehaviour
{
    //Array of ID information
    public int[] Ids;

    //Data controller GameObject
    public GameObject dataController;

    //UI objects defined
    public Image fadeImage;
    public Dropdown chooseId;
    public Button addId;
    public Button shootButton;
    public Button driveButton;
    public Button dodgeButton;
    public Button idButton;

    //Title screen variables
    public float buttonPressTimer;
    bool idButtonsShowing;

    //Init the title screen
    private void Start()
    {
        dataController = GameObject.FindGameObjectWithTag("DataController");

        buttonPressTimer = 3f;
        shootButton.interactable = false;
        driveButton.interactable = false;
        dodgeButton.interactable = false;

        idButtonsShowing = false;
        LoadDropdownValues();
    }

    //Update the title screen
    private void Update()
    {
        buttonPressTimer -= Time.deltaTime;

        if (buttonPressTimer <= 0)
        {
            shootButton.interactable = true;
            driveButton.interactable = true;
            dodgeButton.interactable = true;
        }

        UpdateIdButtons();
    }

    //Load a new scene
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }	

    //Add a new ID
    public void AddNewId()
    {
        dataController.GetComponent<DataController>().AddNewId();
        idButtonsShowing = false;
    }

    //Show the ID buttons
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

    //Update the ID buttons
    void UpdateIdButtons()
    {
        idButton.GetComponentInChildren<Text>().text = "ID: " + dataController.GetComponent<DataController>().GetCurrentId();

        fadeImage.gameObject.SetActive(idButtonsShowing);
        chooseId.gameObject.SetActive(idButtonsShowing);
        addId.gameObject.SetActive(idButtonsShowing);
    }
    
    //Load the dropdown values
    void LoadDropdownValues()
    {
        chooseId.ClearOptions();
        chooseId.AddOptions(dataController.GetComponent<DataController>().GetAllIdStrings());
        chooseId.captionText.text = "ID: " + dataController.GetComponent<DataController>().GetCurrentId();
    }

    //Choose the current ID
    public void ChooseCurrentID()
    {
        dataController.GetComponent<DataController>().SetCurrentId(chooseId.value);
        idButton.GetComponentInChildren<Text>().text = "ID: " + chooseId.value;
        chooseId.Hide();
        idButtonsShowing = false;
    }
}
