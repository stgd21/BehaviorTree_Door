using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This purely holds data that needs to be referenced by other scripts
public class Kinematic : MonoBehaviour
{
    [Header("Setting values here will only affect how other scripts view this gameObject")]
    public Vector3 kPosition;
    public float kOrientation;
    public Vector3 kVelocity = Vector3.zero;
    public float kRotation = 0f;

    public Vector3 linearVelocity;
    public float angularVelocity;

    public void GetData(SteeringOutput currentSteering)
    {
        if (currentSteering != null)
        {
            //Debug.Log("updating with linear of " + currentSteering.linear);
            kVelocity += currentSteering.linear * Time.deltaTime;
            kRotation += currentSteering.angular * Time.deltaTime;

            linearVelocity += currentSteering.linear * Time.deltaTime;
            angularVelocity += currentSteering.angular * Time.deltaTime;
        }
    }

    protected virtual void Update()
    {
        transform.position += linearVelocity * Time.deltaTime;
        Vector3 angularIncrement = new Vector3(0, angularVelocity * Time.deltaTime, 0);
        transform.eulerAngles += angularIncrement;

        kPosition = transform.position;
        kOrientation = transform.eulerAngles.y;
    }
}
