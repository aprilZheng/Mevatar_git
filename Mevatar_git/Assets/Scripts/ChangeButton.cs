using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeButton : MonoBehaviour
{
    public void OnValueChanged(bool isOn)
    {
        if (isOn)
        {
            // split button's name, for example, head-1, eyebrow-2, eyeInL-2...
            string[] names = this.name.Split('-');

            // change the avatar on the screen
            AvatarManager._instance.ChangeAvatar(names[0], names[1]);

            // change the avatar data in firebase database
            FirebaseManager._instance.ChangeMevatar(names[0], names[1]);
            Debug.Log(gameObject);

        }
    }
}
