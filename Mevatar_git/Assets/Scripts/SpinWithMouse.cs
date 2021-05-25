using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWithMouse : MonoBehaviour
{
    private bool isDown = false;

    public Vector3 curPos;
    public Vector3 lastPos;

    private float length = 0;

    // Update is called once per frame
    void Update()
    {
        curPos = Input.mousePosition;
        if (isDown)
        {
            Vector3 offset = curPos - lastPos;
            if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y) && Mathf.Abs(offset.x) > length)
            {
                transform.Rotate(Vector3.up, -offset.x);
            }
        }
        lastPos = Input.mousePosition;
    }

    void OnMouseUp()
    {
        isDown = false;
    }
    void OnMouseDown()
    {
        isDown = true;
    }
}
