using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockerPrioritized : MonoBehaviour
{
    public PrioritySteering steering;
    public Kinematic arriveTarget;
    public Vector3 linearVelocity;
    public float angularVelocity;

    void Start()
    {
        steering = new PrioritySteering();
        //Make 2 groups, one for flocking, and one for avoiding obstacles
        steering.groups = new BlendedSteering[2];
        //Initialize them
        steering.groups[0] = new BlendedSteering();
        steering.groups[1] = new BlendedSteering();

        //Put obstacle avoidance into its own group
        ObstacleAvoidance avoidAI = new ObstacleAvoidance();
        avoidAI.character = GetComponent<Kinematic>();

        //Initialize array, only 1 behavior needed in this group's consideration
        steering.groups[0].behaviors = new BehaviorAndWeight[1];
        steering.groups[0].behaviors[0] = new BehaviorAndWeight();

        //Assign specific behavior
        steering.groups[0].behaviors[0].behavior = avoidAI;

        //Set weight
        steering.groups[0].behaviors[0].weight = 4f;

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

        //Set up flocking group in priority steering
        steering.groups[1].behaviors = new BehaviorAndWeight[3];
        steering.groups[1].behaviors[0] = new BehaviorAndWeight();
        steering.groups[1].behaviors[1] = new BehaviorAndWeight();
        steering.groups[1].behaviors[2] = new BehaviorAndWeight();

        //Assign specific behaviors
        steering.groups[1].behaviors[0].behavior = arriveAI;
        steering.groups[1].behaviors[1].behavior = seperateAI;
        steering.groups[1].behaviors[2].behavior = lookAI;

        //Set weight of each behavior
        steering.groups[1].behaviors[0].weight = 1f;
        steering.groups[1].behaviors[1].weight = 20f;
        steering.groups[1].behaviors[2].weight = 4f;
    }

    // Update is called once per frame
    void Update()
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
