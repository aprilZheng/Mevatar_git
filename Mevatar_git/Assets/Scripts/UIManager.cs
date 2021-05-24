using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject FirstScreen;
    public GameObject LoginUI;
    public GameObject RegisterUI;
    public GameObject UserDataUI;
    public GameObject MevatarShowUI;
    public GameObject MevatarChangeUI;

    public Camera mainCamera;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void ClearScreen()
    {
        FirstScreen.SetActive(false);
        LoginUI.SetActive(false);
        RegisterUI.SetActive(false);
        UserDataUI.SetActive(false);
        MevatarShowUI.SetActive(false);
        MevatarChangeUI.SetActive(false);
    }

    public void LoginScreen()
    {
        ClearScreen();
        LoginUI.SetActive(true);
    }

    public void RegisterScreen()
    {
        ClearScreen();
        RegisterUI.SetActive(true);
    }

    public void MevatarShowScreen()
    {
        ClearScreen();
        MevatarShowUI.SetActive(true);
    }

    public void MevatarChangeScreen()
    {
        ClearScreen();
        MevatarChangeUI.SetActive(true);
    }

    public void UserDataScreen()
    {
        UserDataUI.SetActive(true);
    }
}
