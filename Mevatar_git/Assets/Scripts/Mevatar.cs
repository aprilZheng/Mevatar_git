using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mevatar : MonoBehaviour
{
    public string head;
    public string eyebrow;
    public string eyes;

    public Mevatar()
    {
        head = "1";
        eyebrow = "1";
        eyes = "1";
        Debug.Log("Mevatar Class Create Default Mevatar");
    }

    public Mevatar(string _head, string _eyebrow, string _eyes)
    {
        head = _head;
        eyebrow = _eyebrow;
        eyes = _eyes;
        Debug.Log("Mevatar Class Create Default Mevatar");
    }

    public void SetHead(string _head)
    {
        head = _head;
        Debug.Log("Mevatar Class Updated Head");
    }

    public void SetEyebrow(string _eyebrow)
    {
        eyebrow = _eyebrow;
        Debug.Log("Mevatar Class Updated Eyebrow");
    }

    public void SetEyes(string _eyes)
    {
        eyes = _eyes;
        Debug.Log("Mevatar Class Updated Eyes");
    }
}
