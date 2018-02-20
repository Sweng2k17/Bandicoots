using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to store target information.
/// </summary>
public class Target : MonoBehaviour {

    private Vector3 position;
    private float velocity;
    private float acceleration;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Default constructor. Target initially has velocity and acceleration of 0.
    /// Position of target is initially 0, 0, 0.
    /// </summary>
    public Target()
    {
        position = new Vector3(0,0,0);
        velocity = 0;
        acceleration = 0;
    }

    /// <summary>
    /// Returns the acceleration of the target.
    /// </summary>
    /// <returns>
    /// Acceleration of the target.
    /// </returns>
    public float GetAcceleration()
    {
        return acceleration;
    }

    /// <summary>
    /// Returns the velocity of the target.
    /// </summary>
    /// <returns>
    /// Velocity of the target.
    /// </returns>
    public float GetVelocity()
    {
        return velocity;
    }

    /// <summary>
    /// Returns a Vector3 object storing the target's coordinates.
    /// </summary>
    /// <returns>
    /// A Vector3 object that stores the target's x, y, and z coordinates.
    /// </returns>
    public Vector3 GetPosition()
    {
        return position;
    }

    /// <summary>
    /// Sets the acceleration of the target to the new specified value.
    /// </summary>
    /// <param name="newAccel">
    /// The new acceleration of the target.</param>
    public void SetAcceleration(float newAccel)
    {
        acceleration = newAccel;
    }

    /// <summary>
    /// Sets the velocity of the target to the new specified value.
    /// </summary>
    /// <param name="newVelocity">
    /// The new velocity of the target.</param>
    public void SetVelocity(float newVelocity)
    {
        velocity = newVelocity;
    }

    /// <summary>
    /// Sets the target's position to a new coordinates.
    /// </summary>
    /// <param name="newPosX">
    /// New x coordinate of target.</param>
    /// <param name="newPosY">
    /// New y coordinate of target.</param>
    /// <param name="newPosZ">
    /// New z coordinate of target.</param>
    public void ChangePosition(float newPosX, float newPosY, float newPosZ)
    {
        position.Set(newPosX, newPosY, newPosZ);
    }

}
