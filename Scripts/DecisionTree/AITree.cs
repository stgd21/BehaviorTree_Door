using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITree : MonoBehaviour
{
    public Kinematic doorNavTarget;
    public Kinematic roomNavTarget;
    public Door door;
    private Arrive arrive;

    Vector3 linearVelocity;
    float angularVelocity;
    private Kinematic kinematic;
    // Start is called before the first frame update
    void Start()
    {
        kinematic = GetComponent<Kinematic>();
        arrive = new Arrive();
        arrive.character = kinematic;
        arrive.target = kinematic;  
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            door.Begin();
            Behave();
        }

        //Steering stuff
        SteeringOutput movementSteering;
        //Update position and rotation
        transform.position += linearVelocity * Time.deltaTime;
        Vector3 angularIncrement = new Vector3(0, angularVelocity * Time.deltaTime, 0);
        transform.eulerAngles += angularIncrement;
        movementSteering = arrive.GetSteering();
        if (movementSteering != null)
            linearVelocity += movementSteering.linear * Time.deltaTime;
        kinematic.GetData(movementSteering);
    }

    void Behave()
    {
        //Sequence for going in an open door
        DebugTask openDebug = new DebugTask("Going through open door sequence");
        ConditionalIsTrue doorOpenCheck = new ConditionalIsTrue(door.isOpen);
        ArriveToTarget goInRoom = new ArriveToTarget(arrive, roomNavTarget);
        Task[] openDoorSystem = new Task[3];
        openDoorSystem[0] = openDebug;
        openDoorSystem[1] = doorOpenCheck;
        openDoorSystem[2] = goInRoom;
        Sequence doorOpenSequence = new Sequence(openDoorSystem);

        //Sequence for going in closed door
        DebugTask closedDebug = new DebugTask("Going through closed door sequence");
        ArriveToTarget goToDoor = new ArriveToTarget(arrive, doorNavTarget);
        ConditionalIsTrue tryOpeningDoor = new ConditionalIsTrue(door.TryOpening());
        OpenDoor openTheDoorUp = new OpenDoor(door);
        //As is, these arrives don't have timing so it will seem like just going into the room
        ArriveToTarget advanceIntoRoom = new ArriveToTarget(arrive, roomNavTarget);
        Task[] closedDoorSystem = new Task[5];
        closedDoorSystem[0] = closedDebug;
        closedDoorSystem[1] = goToDoor;
        closedDoorSystem[2] = tryOpeningDoor;
        closedDoorSystem[3] = openTheDoorUp;
        closedDoorSystem[4] = advanceIntoRoom;
        Sequence closedDoorSequence = new Sequence(closedDoorSystem);

        //Sequence for going in locked door
        //Can just reuse goToDoor!
        DebugTask lockedDebug = new DebugTask("Going through locked door sequence");
        ConditionalIsFalse tryOpeningDoorFalse = new ConditionalIsFalse(door.TryOpening());
        BustDoor burstIn = new BustDoor(door);
        //Can reuse advanceIntoRoom as well!
        Task[] lockedDoorSystem = new Task[5];
        lockedDoorSystem[0] = lockedDebug;
        lockedDoorSystem[1] = goToDoor;
        lockedDoorSystem[2] = tryOpeningDoorFalse;
        lockedDoorSystem[3] = burstIn;
        lockedDoorSystem[4] = advanceIntoRoom;
        Sequence lockedDoorSequence = new Sequence(lockedDoorSystem);

        //Make a selector to try to go in closed door before locked
        Task[] closedDoorOptions = new Task[2];
        closedDoorOptions[0] = closedDoorSequence;
        closedDoorOptions[1] = lockedDoorSequence;
        Selector closedProtocols = new Selector(closedDoorOptions);

        //Make a selector to try to go in the open door before closed ones
        Task[] overallDoorProtocols = new Task[2];
        overallDoorProtocols[0] = doorOpenSequence;
        overallDoorProtocols[1] = closedProtocols;
        Selector overallDoorOptions = new Selector(overallDoorProtocols);
        overallDoorOptions.run();
    }

}

