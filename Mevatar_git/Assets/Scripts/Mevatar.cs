using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mevatar : MonoBehaviour
{
    // parameters for avatar
    public string head;
    public string eyebrow;
    public string eyes;

    // default avatar for a new user
    public Mevatar()
    {
        head = "1";
        eyebrow = "1";
        eyes = "1";
        Debug.Log("Mevatar Class Create Default Mevatar");
    }

    // update avatar with given parameters
    public Mevatar(string _head, string _eyebrow, string _eyes)
    {
        head = _head;
        eyebrow = _eyebrow;
        eyes = _eyes;
        Debug.Log("Mevatar Class Updated");
    }

    // update avatar with given parameter
    public void SetHead(string _head)
    {
        head = _head;
        Debug.Log("Mevatar Class Updated Head");
    }

    // update avatar with given parameter
    public void SetEyebrow(string _eyebrow)
    {
        eyebrow = _eyebrow;
        Debug.Log("Mevatar Class Updated Eyebrow");
    }

    // update avatar with given parameters
    public void SetEyes(string _eyes)
    {
        eyes = _eyes;
        Debug.Log("Mevatar Class Updated Eyes");
    }
}
