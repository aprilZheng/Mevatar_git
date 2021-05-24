using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public string email;
    public string username;
    public Mevatar mevatar;

    public User()
    {

    }

    public User(string _email, string _username)
    {
        email = _email;
        username = _username;
        mevatar = new Mevatar();
        Debug.Log("User Class Created User");
    }

    public void UpdateUsername(string _username)
    {
        username = _username;
        Debug.Log("User Class Updated Username");
    }

    public void UpdateEmail(string _email)
    {
        email = _email;
        Debug.Log("User Class Updated email");
    }
}
