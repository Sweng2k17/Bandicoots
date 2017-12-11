using UnityEngine;
using System.Collections;
using UnityEngine.UI; //Need this for calling UI scripts


public class ClickScript : MonoBehaviour {

    [SerializeField]
    MeshRenderer renderer;
    [SerializeField]
    MeshCollider meshCollider;
    [SerializeField]
    GameObject gm;
    Manager manager;
    [SerializeField]
    Text ojectTitleText;
    [SerializeField]
    Text velocityText;
    [SerializeField]
    Text accelerationText;
    [SerializeField]
    Text distanceText;

    private double velocity;
    private double acceleration;
    private double distance;
    private bool active = false;

    // Use this for initialization
    void Start () {
        Manager manager = gm.GetComponent<Manager>();

    }

    // Update is called once per frame
    void Update () {

        if (active)
        {
            double[] velcityHolder = manager.getVelocity(0);
            double[] accelHolder = manager.getAcceleration(0);
            Vector3 posHolder = manager.getPosition(0);

            //TODO set text to values
            velocityText.text = "Velocity: X = " + velcityHolder[0] + "  Y = " + velcityHolder[1] + "  Z = " + velcityHolder[2];
            accelerationText.text = "Acceleration: X = " + accelHolder[0] + "  Y = " + accelHolder[1] + "  Z = " + accelHolder[2];

            Vector3.Distance(posHolder, new Vector3(0, 0, 0));
            distanceText.text = "Distance: " + Vector3.Distance(posHolder, new Vector3(0, 0, 0))/10 + " Miles";

        }

    }

   
    public void setActive(bool set)
    {
        active = set;
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
        }
    }
}
