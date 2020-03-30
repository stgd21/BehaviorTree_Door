using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public bool isLocked = false;
    public bool isOpen = true;
    public Toggle lockToggle;
    public Toggle openToggle;

    public void Begin()
    {
        if (isOpen)
        {
            transform.Rotate(0f, 90f, 0f, Space.World);
            Debug.Log("its open");
        }
    }
    public bool TryOpening()
    {
        if (isLocked)
            return false;
        else
        {
            return true;
        }    
    }

    public bool OpenDoor()
    {
        Debug.Log("Opening unlocked door");
        transform.Rotate(0f, 60f, 0f, Space.World);
        return true;
    }

    public bool Burst()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(0, 0, 10, ForceMode.VelocityChange);
        Debug.Log("bursting door");
        return true;
    }

    public void ToggleLocked()
    {
        isLocked = !isLocked;
    }

    public void ToggleOpen()
    {
        isOpen = !isOpen;
    }

}
