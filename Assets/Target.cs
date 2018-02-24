using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to store target information.
/// </summary>
public class Target : MonoBehaviour {

    private Vector3 position;
    private Vector3 velocity;
    private Vector3 acceleration;


    // Use this for initialization
    // Target has position, velocity, and acceleration initially set to 0, 0, 0.
    void Start () {
        position = new Vector3(0, 0, 0);
        velocity = new Vector3(0, 0, 0);
        acceleration = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Returns a Vector3 object storing the target's acceleration.
    /// </summary>
    /// <returns>
    /// A Vector3 object that stores the target's x, y, and z acceleration.
    /// </returns>
    public Vector3 GetAcceleration()
    {
        return acceleration;
    }

    /// <summary>
    /// Returns a Vector3 object storing the target's velocity.
    /// </summary>
    /// <returns>
    /// A Vector3 object that stores the target's x, y, and z velocity.
    /// </returns>
    public Vector3 GetVelocity()
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
    /// Sets the target's acceleration to new values.
    /// </summary>
    /// <param name="newAccX">
    /// The new acceleration in x.</param>
    /// <param name="newAccY">
    /// The new acceleration in y.</param>
    /// <param name="newAccZ">
    /// The new acceleration in z.</param>
    public void ChangeAcceleration(float newAccX, float newAccY, float newAccZ)
    {
        acceleration.Set(newAccX, newAccY, newAccZ);
    }

    /// <summary>
    /// Sets the target's velocity to new values.
    /// </summary>
    /// <param name="newVelX">
    /// The new velocity in x.</param>
    /// <param name="newVelY">
    /// The new velocity in y.</param>
    /// <param name="newVelZ">
    /// The new velocity in z.</param>
    public void ChangeVelocity(float newVelX, float newVelY, float newVelZ)
    {
        velocity.Set(newVelX, newVelY, newVelZ);
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
