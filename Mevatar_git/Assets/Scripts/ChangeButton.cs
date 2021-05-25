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
            AvatarManager._instance.ChangeAvatar(names[0], names[1]);

            FirebaseManager._instance.ChangeMevatar(names[0], names[1]);
            Debug.Log(gameObject);

        }
    }
}
