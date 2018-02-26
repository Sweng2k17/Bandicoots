using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetected : MonoBehaviour
{

    float alphaDec = 0.0035f;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("BEAM HAS DETECTED A TARGET:" + collision.collider.name);

        //reset alpha
        resetAlpha(collision.collider.GetComponent<MeshRenderer>());

        //makes target visible if hit by beam
        collision.collider.GetComponent<MeshRenderer>().enabled = true;
        
    }

    private void OnCollisionStay(Collision collision)
    {

    }

    private void OnCollisionExit(Collision collision)
    {

    }

    /**
     *  resetAlpha(MeshRenderer target)
     *  
     *  PRE: takes in a target's mesh renderer data
     *  
     *  POST: sets the alpha value back to 1 and re-enables target's rendering
     *  (Pat M. 02.21.2018)
     **/
    private void resetAlpha(MeshRenderer target)
    {
        // set alpha back to 1
        Color colour = target.material.color;
        colour.a = 1;
        target.material.color = colour;
        // enable rendering
        target.enabled = true;
    }

}
