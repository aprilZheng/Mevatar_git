using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeButton : MonoBehaviour
{
    public void OnValueChanged(bool isOn)
    {
        if (isOn)
        {
            string[] names = this.name.Split('-');
            AvatarManager.instance.ChangeAvatar(names[0], names[1]);
            Debug.Log(gameObject);

            switch (names[0])
            {
                case "head":
                    Debug.Log("names[1]= " + names[1]);
                    StartCoroutine(FirebaseManager.instance.UpdateHead(names[1].ToString()));
                    StartCoroutine(FirebaseManager.instance.LoadUserData());
                    break;
                case "eyebrow":
                    Debug.Log("names[1]= " + names[1]);
                    StartCoroutine(FirebaseManager.instance.UpdateEyebrow(names[1].ToString()));
                    StartCoroutine(FirebaseManager.instance.LoadUserData());
                    break;
                case "eyeInL":
                    Debug.Log("names[1]= " + names[1]);
                    StartCoroutine(FirebaseManager.instance.UpdateEyes(names[1].ToString()));
                    StartCoroutine(FirebaseManager.instance.LoadUserData());
                    break;
                default:
                    Debug.Log("AvatarChangeButton Save data failed. names[0]= " + names[0] + ", names[1]= " + names[1]);
                    break;
            }
        }
    }
}
