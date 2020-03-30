using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task
{
    public abstract bool run();
}

//Return false if all fail
public class Selector : Task
{
    public Task[] children;

    public Selector(Task[] newArray)
    {
        children = newArray;
    }

    public override bool run()
    {
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].run() == true)
                return true;
        }
        return false;
    }
}

//Return false if one fails
public class Sequence : Task
{
    public Task[] children;

    public Sequence(Task[] newArray)
    {
        children = newArray;
    }
    public override bool run()
    {
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].run() == false)
            {
                return false;
            }
        }
        return true;
    }
}

public class ArriveToTarget : Task
{
    Arrive arriveAI;
    Kinematic target;

    public ArriveToTarget(Arrive newArrive, Kinematic newTarget)
    {
        arriveAI = newArrive;
        target = newTarget;
    }

    public override bool run()
    {
        arriveAI.target = target;
        return true;
    }
}

public class ConditionalIsTrue : Task
{
    bool whatToTest;

    public ConditionalIsTrue(bool test)
    {
        whatToTest = test;
    }

    public override bool run()
    {
        return whatToTest;
    }
}

public class ConditionalIsFalse : Task
{
    bool whatToTest;

    public ConditionalIsFalse(bool test)
    {
        whatToTest = test;
    }

    public override bool run()
    {
        return !whatToTest;
    }
}

public class BustDoor : Task
{
    Door doorToBurst;

    public BustDoor(Door newDoor)
    {
        doorToBurst = newDoor;
    }

    public override bool run()
    {
        return doorToBurst.Burst();
    }
}

public class OpenDoor : Task
{
    Door doorToOpen;

    public OpenDoor(Door newDoor)
    {
        doorToOpen = newDoor;
    }

    public override bool run()
    {
        return doorToOpen.OpenDoor();
    }
}

public class DebugTask : Task
{
    string whatToOutput;

    public DebugTask(string newOutput)
    {
        whatToOutput = newOutput;
    }

    public override bool run()
    {
        Debug.Log(whatToOutput);
        return true;
    }
}
