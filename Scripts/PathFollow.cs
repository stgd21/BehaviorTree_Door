using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : Arrive
{
    public Kinematic[] path;

    private float targetRadius = 4f;
    private int currentPathIndex;

    public override SteeringOutput GetSteering()
    {
        if (target == null)
        {
            currentPathIndex = 0;
            target = path[currentPathIndex];
        }

        Vector3 vectorToTarget = target.kPosition - character.kPosition;
        float distanceToTarget = vectorToTarget.magnitude;
        if (distanceToTarget < targetRadius)
        {
            currentPathIndex++;
            if (currentPathIndex > path.Length - 1)
            {
                currentPathIndex = 0;
            }

        }
        //target.gameObject.GetComponent<Renderer>().material.color = Color.white;
        target = path[currentPathIndex];
        //target.gameObject.GetComponent<Renderer>().material.color = Color.red;
        return base.GetSteering();
    }
}
