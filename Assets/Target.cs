using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Justin Davis
// Version: 2/26/18

/// <summary>
/// Class used to store target information.
/// </summary>
public class Target
{

    private TargetData data;
    private Queue prevData;

    /// <summary>
    /// Default constructor.
    /// Target has position, velocity, and acceleration initially set to 0, 0, 0.
    /// </summary>
    public Target()
    {
        data = new TargetData();
        prevData = new Queue();
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
    public Target(float posX, float posY, float posZ, float velX, float velY, float velZ, float accX, float accY, float accZ, float time)
    {
        data = new TargetData(posX, posY, posZ, velX, velY, velZ, accX, accY, accZ, time);
        prevData = new Queue();
        InsertNewTargetData(data);
    }

    /// <summary>
    ///  Method to insert newly received target data into the queue
    /// </summary>
    /// <param name="targetData">target data object that contains newly received position, velocity, and acceleration and the time increment this data was received</param>
    public void InsertNewTargetData(TargetData targetData)
    {
        //dequeues least recently received data from this target
        if(prevData.Count >= 10)
        {
            prevData.Dequeue();
        }
        //adds newly recorded target data to the queue
        prevData.Enqueue(targetData);
    }

    /// <summary>
    /// updates the target data object to reflect newly received target position, velocity, acceleration, and the time increment this data was received.
    /// </summary>
    /// <param name="posX">New Position in x.</param>
    /// <param name="posY">New Position in y.</param>
    /// <param name="posZ">New Position in z.</param>
    /// <param name="velX">New Velocity in x.</param>
    /// <param name="velY">New Velocity in y.</param>
    /// <param name="velZ">New Velocity in z.</param>
    /// <param name="accX">New Acceleration in x.</param>
    /// <param name="accY">New Acceleration in y.</param>
    /// <param name="accZ">New Acceleration in z.</param>
    /// <param name="time">time increment this data was received.</param>
    public void NewTargetData(float posX, float posY, float posZ, float velX, float velY, float velZ, float accX, float accY, float accZ, float time)
    {
        data.SetPosition(posX, posY, posZ);
        data.SetVelocity(velX, velY, velZ);
        data.SetAcceleration(accX, accY, accZ);
        data.SetTime(time);
        InsertNewTargetData(data);
    }

    /// <summary>
    /// Returns a Vector3 object storing the target's currentacceleration.
    /// </summary>
    /// <returns>
    /// A Vector3 object that stores the target's current x, y, and z acceleration.
    /// </returns>
    public Vector3 GetAcceleration()
    {
        return data.GetAcceleration();
    }

    /// <summary>
    /// Returns a Vector3 object storing the target's current velocity.
    /// </summary>
    /// <returns>
    /// A Vector3 object that stores the target's current x, y, and z velocity.
    /// </returns>
    public Vector3 GetVelocity()
    {
        return data.GetVelocity();
    }

    /// <summary>
    /// Returns a Vector3 object storing the target's current coordinates.
    /// </summary>
    /// <returns>
    /// A Vector3 object that stores the target's current x, y, and z coordinates.
    /// </returns>
    public Vector3 GetPosition()
    {
        return data.GetPosition();
    }

    /// <summary>
    /// Returns a float storing the time the target was detected.
    /// </summary>
    /// <returns>
    /// A float that stores the target's time of detection.
    /// </returns>
    public float GetTime()
    {
        return data.GetTime();
    }

    /// <summary>
    /// Sets the target's current acceleration to new values.
    /// </summary>
    /// <param name="newAccX">
    /// The new acceleration in x.</param>
    /// <param name="newAccY">
    /// The new acceleration in y.</param>
    /// <param name="newAccZ">
    /// The new acceleration in z.</param>
    public void ChangeAcceleration(float newAccX, float newAccY, float newAccZ)
    {
        data.SetAcceleration(newAccX, newAccY, newAccZ);
    }

    /// <summary>
    /// Sets the target's current velocity to new values.
    /// </summary>
    /// <param name="newVelX">
    /// The new velocity in x.</param>
    /// <param name="newVelY">
    /// The new velocity in y.</param>
    /// <param name="newVelZ">
    /// The new velocity in z.</param>
    public void ChangeVelocity(float newVelX, float newVelY, float newVelZ)
    {
        data.SetVelocity(newVelX, newVelY, newVelZ);
    }

    /// <summary>
    /// Sets the target's current position to a new coordinates.
    /// </summary>
    /// <param name="newPosX">
    /// New x coordinate of target.</param>
    /// <param name="newPosY">
    /// New y coordinate of target.</param>
    /// <param name="newPosZ">
    /// New z coordinate of target.</param>
    public void ChangePosition(float newPosX, float newPosY, float newPosZ)
    {
        data.SetPosition(newPosX, newPosY, newPosZ);
    }

    /// <summary>
    /// Changes the target's time of detection to the most recent time that it was detected.
    /// </summary>
    /// <param name="time">
    public void ChangeTime(float time)
    {
        data.SetTime(time);
    }

}