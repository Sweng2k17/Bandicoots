using UnityEngine;
using System.Collections;
using UnityEngine.UI; //Need this for calling UI scripts


public class ClickScript : MonoBehaviour
{



    private ReferenceScript reference;
    private Text objectTitleText;
    private Text velocityText;
    private Text accelerationText;
    private Text distanceText;

    private Manager manager;
    private GameObject gm;

    private double velocity;
    private double acceleration;
    private double distance;
    private bool active = false;
    private MeshCollider meshCollider = new MeshCollider();
    private int missileNumber = 0;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Start of click script");
        gm = GameObject.FindGameObjectWithTag("GM");
        reference = gm.GetComponent<ReferenceScript>();
        manager = gm.GetComponent<Manager>();
        objectTitleText = reference.GetTitle();
        velocityText = reference.GetVelocity();
        accelerationText = reference.GetAcceleration();
        distanceText = reference.GetDistance();
        meshCollider = this.GetComponentInParent<MeshCollider>();


    }

    // Update is called once per frame
    void Update()
    {

        if (active)
        {
            Debug.Log("Active");
            try
            {
                manager.enableObjectInfo();
                Vector3 posHolder = manager.getPosition(missileNumber);

                velocityText.text = "Velocity: \n X = " + (int)manager.getVelocity(missileNumber)[0] + "mph  Y = " + (int)manager.getVelocity(missileNumber)[1] + "mph  Z = " + (int)manager.getVelocity(missileNumber)[2] + " mpg";
                accelerationText.text = "Acceleration:\n X = " + manager.getAcceleration(missileNumber)[0] + "  Y = " + manager.getAcceleration(missileNumber)[1] + "  Z = " + manager.getAcceleration(missileNumber)[2] + " mph";

                Vector3.Distance(posHolder, new Vector3(0, 0, 0));
                distanceText.text = "Distance: " + Vector3.Distance(posHolder, new Vector3(0, 0, 0)) / 10 + " Miles";
            }
            catch (System.Exception e)
            {
                Debug.Log("Index out of range " + e);

            }



        }

    }

    public void setNumber(int number)
    {
        missileNumber = number;
    }

    public void setActive(bool set)
    {
        active = set;
        if (set == false)
        {
            this.GetComponentInParent<MeshRenderer>().material.color = Color.white;

        }
    }

    void OnMouseOver()
    {
        Debug.Log("Mouse over missile");
        //print("Mouse is now over missile");
        if (Input.GetMouseButtonDown(0))
        {
            active = true;
            manager.enableObjectInfo();
            Debug.Log("The click has worked");
            this.GetComponentInParent<MeshRenderer>().material.color = Color.red;
        }
    }
}