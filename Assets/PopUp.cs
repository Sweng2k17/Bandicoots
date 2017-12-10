using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour {


	[SerializeField]
	public GameObject popUp;
	[SerializeField]
	public Button button;
	int counter=0;


	void Start()
	{

	}




	public void ShowHidePopUp()
	{
		
		counter++;
		if (counter % 2 == 1) {
			popUp.gameObject.SetActive (false);
		} else {
			popUp.gameObject.SetActive (true);

		}
	
	}


}
