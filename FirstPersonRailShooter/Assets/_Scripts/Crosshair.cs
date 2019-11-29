using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    void Start()
    {
        // Make cursor invisible.
        Cursor.visible = false;
    }

    void Update()
    {
        // Press escape to make cursor visible.
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.visible = true;

        // Update crosshair position to follow cursor.
        transform.position = Input.mousePosition;
    }
}
