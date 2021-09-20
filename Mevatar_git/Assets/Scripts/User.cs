using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public string email; // user's email
    public string username; // username
    public Mevatar mevatar; // user's avatar

    public User()
    {

    }

    // User method to create a new user
    public User(string _email, string _username)
    {
        // set user's email and name
        email = _email;
        username = _username;
        // create a new avatar for the user
        mevatar = new Mevatar();
        Debug.Log("User Class Created User");
    }

    // update username
    public void SetUsername(string _username)
    {
        username = _username;
        Debug.Log("User Class Updated Username");
    }

    // update user's email
    public void SetEmail(string _email)
    {
        email = _email;
        Debug.Log("User Class Updated email");
    }
}
