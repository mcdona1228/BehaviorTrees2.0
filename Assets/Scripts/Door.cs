using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isClosed = false;
    public bool isLocked = false;
    public bool isSteel = false;
    public bool isTitanium = false;
    public bool isVibranium = false;

    Vector3 closedRotation = new Vector3(0, 0, 0);
    Vector3 openRotation = new Vector3(0, -135, 0);

    // Start is called before the first frame update
    void Start()
    {
        if (isClosed)
        {
            transform.eulerAngles = closedRotation;
        }
        else
        {
            transform.eulerAngles = openRotation;
        }
        if (isSteel)
        {
            //transform.localScale /= 2;
        }
    }

    public bool Open()
    {
        if (isClosed && !isLocked)
        {
            //Debug.Log("door is now open");
            isClosed = false;
            transform.eulerAngles = openRotation;
            return true;
        }

        //Debug.Log("door was either locked or already open");
        return false;
    }

    public bool Close()
    {
        if (!isClosed)
        {
            //Debug.Log("door is now closed");
            transform.eulerAngles = closedRotation;
            isClosed = true;
        }
        return true;
    }

    public bool Steel()
    {
        if (isSteel && isClosed && isLocked)
        {
            Debug.Log("door is now squished");
            transform.localScale /= 2;
            isSteel = true;
        }
        return true;
    }
    public bool Titanium()
    {
        if (isTitanium && isClosed && isLocked)
        {
            Debug.Log("door is now titanium");
            transform.eulerAngles = closedRotation;
            isTitanium = true;
        }
        return true;
    }
    public bool Vibranium()
    {
        if (isVibranium && isClosed && isLocked)
        {
            Debug.Log("door is now vibranium");
            transform.eulerAngles = closedRotation;
            isVibranium = true;
        }
        return true;
    }
}
