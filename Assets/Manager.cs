using UnityEngine;
using System.Collections;
using UnityEngine.UI; //Need this for calling UI scripts

/// <summary>
/// Manages the User Interface. Listens for 
/// different keystrokes.
/// </summary>
public class Manager : MonoBehaviour
{

    [SerializeField]
    Transform UIPanel; //Will assign our panel to this variable so we can enable/disable it
    


    [SerializeField]

    bool isPaused; //Used to determine paused state

	//run when script is initialized.
    void Start()
    {
        UIPanel.gameObject.SetActive(false); //make sure our pause menu is disabled when scene starts
        isPaused = false; //make sure isPaused is always false when our scene opens
    }

	//called once per frame
    void Update()
    {


        //If player presses escape and game is not paused. Pause game. If game is paused and player presses escape, unpause.
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
            Pause();
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
            UnPause();
    }

	//called when esc key is pressed.
	//pauses game and opens UI menu
    public void Pause()
    {
        isPaused = true;
        UIPanel.gameObject.SetActive(true); //turn on the pause menu
    }

	//called when esc key is pressed
	//again. Closes UI menu and continues game. 
    public void UnPause()
    {
        isPaused = false;
        UIPanel.gameObject.SetActive(false); //turn off pause menu
    }

	//quits program.
    public void ExitProgram()
    {
        Application.Quit();
    }


}