using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class ReferenceScript : MonoBehaviour
{

    [SerializeField]
    MeshRenderer renderer;
    [SerializeField]
    MeshCollider meshCollider;
    [SerializeField]
    GameObject gm;
    Manager manager;
    [SerializeField]
    Text objectTitleText;
    [SerializeField]
    Text velocityText;
    [SerializeField]
    Text accelerationText;
    [SerializeField]
    Text distanceText;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Text GetTitle()
    {
        return objectTitleText;
    }

    public Text GetVelocity()
    {
        return velocityText;
    }

    public Text GetAcceleration()
    {
        return accelerationText;
    }

    public Text GetDistance()
    {
        return distanceText;
    }

    public GameObject GetGM()
    {
        return gm;
    }


}