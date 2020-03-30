using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocker : MonoBehaviour
{
    public BlendedSteering steering;
    public Kinematic arriveTarget;
    public Vector3 linearVelocity;
    public float angularVelocity;

    private void Awake()
    {
        steering = new BlendedSteering();

        //Create behaviors to go into the bird
        Arrive arriveAI = new Arrive();
        arriveAI.character = GetComponent<Kinematic>();
        arriveAI.target = arriveTarget;

        Seperation seperateAI = new Seperation();
        seperateAI.character = GetComponent<Kinematic>();
        GameObject[] allBirds = GameObject.FindGameObjectsWithTag("bird");
        Kinematic[] allOtherBirdKinematics = new Kinematic[allBirds.Length];
        int j = 0;
        foreach (GameObject bird in allBirds)
        {
            if (bird != this)
            {
                allOtherBirdKinematics[j] = bird.GetComponent<Kinematic>();
                j++;
            }
            else
            {
                Debug.Log("caught myself");
            }
        }
        seperateAI.targets = allOtherBirdKinematics;

        LookWhereGoing lookAI = new LookWhereGoing();
        lookAI.character = GetComponent<Kinematic>();

        //Set up the blended steering array
        //We need three for Seperation, Arrive, and LookWhereGoing
        steering.behaviors = new BehaviorAndWeight[3];
        steering.behaviors[0] = new BehaviorAndWeight();
        steering.behaviors[1] = new BehaviorAndWeight();
        steering.behaviors[2] = new BehaviorAndWeight();

        //Assign the specific behaviors
        steering.behaviors[0].behavior = arriveAI;
        steering.behaviors[1].behavior = seperateAI;
        steering.behaviors[2].behavior = lookAI;

        //Now set the weight of each behavior
        steering.behaviors[0].weight = 1f;
        steering.behaviors[1].weight = 20f;
        steering.behaviors[2].weight = 4f;
    }

    private void Update()
    {
        SteeringOutput movementSteering;
        transform.position += linearVelocity * Time.deltaTime;
        Vector3 angularIncrement = new Vector3(0, angularVelocity * Time.deltaTime, 0);
        transform.eulerAngles += angularIncrement;

        movementSteering = steering.GetSteering();
        if (movementSteering != null)
        {
            linearVelocity += movementSteering.linear * Time.deltaTime;
            angularVelocity += movementSteering.angular * Time.deltaTime;
        }
        GetComponent<Kinematic>().GetData(movementSteering);
    }
}
