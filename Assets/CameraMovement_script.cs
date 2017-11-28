using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement_script : MonoBehaviour
{

    public float speed = 30.0f;             //speed of camera
    public float mouseSensitivity = 0.25f; // Sensitivity when using mouse to turn
    public bool inverted = false;           // Doesn't invert the mouse
	private bool isPaused = false;			//tells if the game is paused

    private Vector3 lastMouse = new Vector3(225, 225, 225); //Vector used to keep track of mouse movements 
    private Vector3 cam1;                   // Variable for preset camera position	
    private Vector3 angle1;                 // Variable for preset camera angles



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		if (!isPaused) {
			
			//Turning with mouse
			if (!Input.GetMouseButton (0))
				lastMouse = Input.mousePosition;
			if (Input.GetMouseButton (0)) {
				lastMouse = Input.mousePosition - lastMouse;
				if (!inverted)
					lastMouse.y = -lastMouse.y;
				lastMouse *= mouseSensitivity;
				lastMouse = new Vector3 (transform.eulerAngles.x + lastMouse.y, transform.eulerAngles.y + lastMouse.x, 0);
				transform.eulerAngles = lastMouse;

				lastMouse = Input.mousePosition;
			}

			// WSDA Camera Movement
			Vector3 direction = new Vector3 ();

			if (Input.GetKey (KeyCode.W))
				direction.z += 1.0f;
			if (Input.GetKey (KeyCode.S))
				direction.z -= 1.0f;
			if (Input.GetKey (KeyCode.A))
				direction.x -= 1.0f;
			if (Input.GetKey (KeyCode.D))
				direction.x += 1.0f;
			if (Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl))
				direction.y -= 1.0f;
			if (Input.GetKey (KeyCode.Space))
				direction.y += 1.0f;

			//Bow View of ship
			if (Input.GetKey (KeyCode.Keypad1) || Input.GetKey (KeyCode.Alpha1)) {
				cam1 = new Vector3 (1.8f, 28.5f, 64.5f);
				angle1 = new Vector3 (15.5f, -176.25f, 0.0f);
				transform.eulerAngles = angle1;
				transform.position = cam1;

			}

			//Starboard Side view of ship
			if (Input.GetKey (KeyCode.Keypad2) || Input.GetKey (KeyCode.Alpha2)) {
				cam1 = new Vector3 (52.8f, 32.8f, -4.7f);
				angle1 = new Vector3 (19.5f, -86.0f, 0.0f);
				transform.eulerAngles = angle1;
				transform.position = cam1;

			}

			//Stern side view of ship
			if (Input.GetKey (KeyCode.Keypad3) || Input.GetKey (KeyCode.Alpha3)) {
				cam1 = new Vector3 (-0.5f, 29.5f, -53.8f);
				angle1 = new Vector3 (15.75f, -0.5f, 0.0f);
				transform.eulerAngles = angle1;
				transform.position = cam1;

			}

			//Port side view of ship
			if (Input.GetKey (KeyCode.Keypad4) || Input.GetKey (KeyCode.Alpha4)) {
				cam1 = new Vector3 (-48.0f, 23.3f, -1.8f);
				angle1 = new Vector3 (6.75f, -272.25f, 0.0f);
				transform.eulerAngles = angle1;
				transform.position = cam1;

			}

			direction.Normalize ();

			transform.Translate (direction * speed * Time.deltaTime);

			//Sets the max and min positions for the x,y, and z axis
			transform.position = new Vector3 (Mathf.Clamp (transform.position.x, -1000, 1000),
				Mathf.Clamp (transform.position.y, 6, 500), Mathf.Clamp (transform.position.z, -1000, 1000));

		}


		//Pauses/Unpauses the Script if the Escape button is pressed
		if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
		{
			Pause();
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
			{
				UnPause();
			}
		}

	}

	//Sets isPaused to true
	void Pause(){
		isPaused = true;
	}

	//Sets isPaused to fasle
	void UnPause(){
		isPaused = false;
	}

}
