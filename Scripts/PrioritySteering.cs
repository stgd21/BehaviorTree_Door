using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrioritySteering
{
    public BlendedSteering[] groups;
    float epsilon = 0.3f;
    SteeringOutput steering;

    public SteeringOutput GetSteering()
    {
        foreach (BlendedSteering group in groups)
        {
            steering = group.GetSteering();

            if (steering.linear.magnitude > epsilon)
            {
                return steering;
            }
        }

        return steering;
    }
}
