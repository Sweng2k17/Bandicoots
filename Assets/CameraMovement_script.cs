using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement_script : MonoBehaviour {

	public float speed = 30.0f; 		//speed of camera
	public float mouse_sensitivity = 0.25f;
	public bool inverted = false;

	private Vector3 lastMouse = new Vector3(225,225,225);
	private Vector3 cam1;
	private Vector3 angle1;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//Turning with mouse
		if (!Input.GetMouseButton (0))
			lastMouse = Input.mousePosition;
		if (Input.GetMouseButton(0)) {
			lastMouse = Input.mousePosition - lastMouse;
			if (!inverted)
				lastMouse.y = -lastMouse.y;
			lastMouse *= mouse_sensitivity;
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
		

		if (Input.GetKey (KeyCode.Keypad1)){
			cam1 = new Vector3(30.0f,33.0f,100.0f);
			angle1 = new Vector3(12.5f,-181.0f,0.0f);
			transform.eulerAngles = angle1;
			transform.position = cam1;

		}

		if (Input.GetKey (KeyCode.Keypad2)){
			cam1 = new Vector3(89.0f,28.5f,31.0f);
			angle1 = new Vector3(11.0f,-86.75f,0.0f);
			transform.eulerAngles = angle1;
			transform.position = cam1;

		}

		if (Input.GetKey (KeyCode.Keypad3)){
			cam1 = new Vector3(30.4f,28.0f,-20.0f);
			angle1 = new Vector3(11.0f,-0.25f,0.0f);
			transform.eulerAngles = angle1;
			transform.position = cam1;

		}

		if (Input.GetKey (KeyCode.Keypad4)){
			cam1 = new Vector3(-23.0f,27.0f,25.6f);
			angle1 = new Vector3(9.0f,-271.0f,0.0f);
			transform.eulerAngles = angle1;
			transform.position = cam1;

		}
		
		direction.Normalize ();

		transform.Translate (direction * speed * Time.deltaTime);
	}
}
