using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : Align
{
    public Kinematic target;

    float angle;

    public override float getAngle()
    {
        return angle;
    }

    public SteeringOutput GetSteering()
    {
        //Calculate target to delegate to align
        Vector3 direction = target.kPosition - character.kPosition;
        if (direction.magnitude == 0)
        {
            Debug.Log("null");
            return null;
        }
            

        base.target = target;
        angle = Mathf.Atan2(direction.x, direction.z);
        angle *= Mathf.Rad2Deg;
        //base.target.transform.eulerAngles = new Vector3(0, angle, 0);
        return base.GetSteering();
    }
}