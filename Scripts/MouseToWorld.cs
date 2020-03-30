using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseToWorld : MonoBehaviour
{
    private void Update()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = 60.5f;
        pos = Camera.main.ScreenToWorldPoint(pos);
        transform.position = pos;
        transform.position = new 
            Vector3(transform.position.x, 0, transform.position.z);
    }
}
