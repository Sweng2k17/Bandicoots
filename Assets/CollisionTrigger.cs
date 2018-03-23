using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger even occuring - missile found");
        //other.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
}
