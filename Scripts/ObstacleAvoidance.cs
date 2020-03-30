using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : Seek
{
    //How far to avoid collision
    float avoidDistance = 5f;
    //Distance to look ahead for collision
    float lookAhead = 15f;
    //Workaround for unity components
    GameObject newTarget;

    public override SteeringOutput GetSteering()
    {
        //1. Calculate target to delegate to seek
        //Calculate collision ray vector
        Vector3 ray = character.kVelocity;
        ray = ray.normalized;
        ray *= lookAhead;

        //Find collision
        RaycastHit hit;
        //Will enter this statement if the ray hits something
        Debug.DrawRay(character.kPosition, ray, Color.red);
        if (Physics.Raycast(character.kPosition, ray, out hit, ray.magnitude))
        {
            //This is a weird workaround for my solution
            if (newTarget != null)
                GameObject.Destroy(newTarget);
            newTarget = new GameObject();
            newTarget.transform.position = hit.point + hit.normal * avoidDistance;
            newTarget.AddComponent<Kinematic>();
            
            //newTarget.kPosition = hit.transform.position + hit.normal * avoidDistance;
            base.target = newTarget.GetComponent<Kinematic>();
            //base.target = hit.transform.position + hit.transform.forward * avoidDistance;
            return base.GetSteering();
        }
        else
        {
            GameObject.Destroy(newTarget);
            return null;
        }
    }
}
