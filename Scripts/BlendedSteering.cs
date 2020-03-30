using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorAndWeight
{
    public SteeringBehavior behavior;
    public float weight;
}

public class BlendedSteering
{
    public BehaviorAndWeight[] behaviors;

    float maxAcceleration = 10;
    float maxRotation = 50f;

    public SteeringOutput GetSteering()
    {
        SteeringOutput result = new SteeringOutput();

        foreach (BehaviorAndWeight b in behaviors)
        {
            SteeringOutput s = b.behavior.GetSteering();
            if (s != null)
            {
                result.angular += s.angular * b.weight;
                result.linear += s.linear * b.weight;
            }
        }

        //crop result
        result.linear = result.linear.normalized * Mathf.Min(maxAcceleration, result.linear.magnitude);
        float angularAcc = Mathf.Abs(result.angular);
        if (angularAcc > maxRotation)
        {
            result.angular /= angularAcc;
            result.angular *= maxRotation;
        }

        return result;
    }
}
