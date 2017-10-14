using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement_script : MonoBehaviour {

	public float speed = 30.0f; 		//speed of camera
	public float mouse_sensitivity = 0.25f;
	public bool inverted = false;

	private Vector3 lastMouse = new Vector3(225,225,225);


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//Turning with mouse
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
		

		//if (Input.GetKey (KeyCode.Keypad1)){
		//	direction.x = -17.0f;
		//	direction.y = 16.0f;
		//	direction.z = -6.0f;
		//	lastMouse.x = 34.0f;
		//	lastMouse.x = 72.0f;
		//	lastMouse.x = 0.0f;
		//}
		
		
		direction.Normalize ();

		transform.Translate (direction * speed * Time.deltaTime);
	}
}
