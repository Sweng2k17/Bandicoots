using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPositionBehavior : MonoBehaviour {

	public float lifetime = 3.0f;

	private float currentTime;

	// Use this for initialization
	void Start () {
		currentTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		currentTime += Time.deltaTime;

		if (currentTime >= lifetime) {
			Destroy (gameObject);
		}
	}
}
