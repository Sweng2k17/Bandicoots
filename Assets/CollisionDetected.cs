using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetected : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("BEAM HAS DETECTED A TARGET:" + collision.collider.name);

        // enable rendering
        collision.gameObject.GetComponent<MeshRenderer>().enabled = true;

        //reset alpha
        resetAlpha(collision.gameObject.GetComponent<MeshRenderer>());

        ContactPoint contact = collision.contacts[0];
        Debug.Log("x: " + contact.point.x + "  " + "y: " + contact.point.y + "  " + "z: " + contact.point.z);
        
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
    }

}
