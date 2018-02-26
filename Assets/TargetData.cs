using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Daniel (Brad) Wallace
// Version: 2/26/18

/// <summary>
/// Class used to store target data information.
/// </summary>
public class TargetData
{

    private Vector3 position;
    private Vector3 velocity;
    private Vector3 acceleration;
    private float time;

    /// <summary>
    /// Default constructor.
    /// Target data consists of position, velocity, acceleration, and the time those values were recorded
    /// // all are initially set to 0, 0, 0.
    /// </summary>
    public TargetData()
    {
        position = new Vector3(0, 0, 0);
        velocity = new Vector3(0, 0, 0);
        acceleration = new Vector3(0, 0, 0);
        time = 0.0f;
    }

    /// <summary>
    /// Constructor to use if position, velocity, and acceleration are known in x, y, and z.
    /// </summary>
    /// <param name="posX">Position in x.</param>
    /// <param name="posY">Position in y.</param>
    /// <param name="posZ">Position in z.</param>
    /// <param name="velX">Velocity in x.</param>
    /// <param name="velY">Velocity in y.</param>
    /// <param name="velZ">Velocity in z.</param>
    /// <param name="accX">Acceleration in x.</param>
    /// <param name="accY">Acceleration in y.</param>
    /// <param name="accZ">Acceleration in z.</param>
    /// <param name="time">time increment this data was received.</param>
    public TargetData(float posX, float posY, float posZ, float velX, float velY, float velZ, float accX, float accY, float accZ, float time)
    {
        position = new Vector3(posX, posY, posZ);
        velocity = new Vector3(velX, velY, velZ);
        acceleration = new Vector3(accX, accY, accZ);
        this.time = time;
    }

    /// <summary>
    /// Returns a Vector3 object storing the target's acceleration at a specific increment in time.
    /// </summary>
    /// <returns>
    /// A Vector3 object that stores the target's x, y, and z acceleration at a specific increment in time.
    /// </returns>
    public Vector3 GetAcceleration()
    {
        return acceleration;
    }

    /// <summary>
    /// Returns a Vector3 object storing the target's velocity at a specific increment in time.
    /// </summary>
    /// <returns>
    /// A Vector3 object that stores the target's x, y, and z velocity at a specific increment in time.
    /// </returns>
    public Vector3 GetVelocity()
    {
        return velocity;
    }

    /// <summary>
    /// Returns a Vector3 object storing the target's coordinates  at a specific increment in time.
    /// </summary>
    /// <returns>
    /// A Vector3 object that stores the target's x, y, and z coordinates at a specific increment in time.
    /// </returns>
    public Vector3 GetPosition()
    {
        return position;
    }

    /// <summary>
    /// Returns a float storing the target's time increment associated with the velocity, acceleration, and position of this object.
    /// </summary>
    /// <returns>
    /// A float that stores the target's time increment.
    /// </returns>
    public float GetTime()
    {
        return time;
    }

    /// <summary>
    /// Sets the target's initial acceleration.
    /// </summary>
    /// <param name="newAccX">
    /// The initial acceleration in x.</param>
    /// <param name="newAccY">
    /// The initial acceleration in y.</param>
    /// <param name="newAccZ">
    /// The initial acceleration in z.</param>
    public void SetAcceleration(float newAccX, float newAccY, float newAccZ)
    {
        acceleration.Set(newAccX, newAccY, newAccZ);
    }

    /// <summary>
    /// Sets the target's initial velocity.
    /// </summary>
    /// <param name="newVelX">
    /// The initial velocity in x.</param>
    /// <param name="newVelY">
    /// The initial velocity in y.</param>
    /// <param name="newVelZ">
    /// The initial velocity in z.</param>
    public void SetVelocity(float newVelX, float newVelY, float newVelZ)
    {
        velocity.Set(newVelX, newVelY, newVelZ);
    }

    /// <summary>
    /// Sets the target's initial position.
    /// </summary>
    /// <param name="newPosX">
    /// Initial x coordinate of target.</param>
    /// <param name="newPosY">
    /// Initial y coordinate of target.</param>
    /// <param name="newPosZ">
    /// Initial z coordinate of target.</param>
    public void SetPosition(float newPosX, float newPosY, float newPosZ)
    {
        position.Set(newPosX, newPosY, newPosZ);
    }

    /// <summary>
    /// Sets the target's time that this target's position, velocity, and acceleration data were received.
    /// </summary>
    /// <param name="time">
    public void SetTime(float time)
    {
        this.time = time;
    }

}