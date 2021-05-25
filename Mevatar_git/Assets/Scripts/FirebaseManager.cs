using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager _instance;

    // Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;

    // Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    // Register variable
    [Header("Register")]
    public TMP_InputField emailRegisterField;
    public TMP_InputField usernameRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;
    public TMP_Text confirmRegisterText;

    // User data
    [Header("User Data")]
    public TMP_Text displayName;
    public TMP_Text displayEmail;
    public TMP_InputField usernameField;
    public TMP_Text displayHead;
    public TMP_Text displayEyebrow;
    public TMP_Text displayEyes;

    public TMP_InputField testEyebrowField;

    // User object
    [Header("User Object")]
    public GameObject currentUserObject;
    public User currentUser;
    public Mevatar currentMevatar;

    void Awake()
    {
        _instance = this;

        // check all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // if they are avalible, Initialized Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        // set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // function for the login button
    public void LoginButton()
    {
        // call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        // call the Firebase auth log in function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        // wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            // if there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            // user is now logged in, not get the result
            User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            confirmLoginText.text = "Logged in";

            

            displayEmail.text = User.Email;
            usernameField.text = User.DisplayName;

            displayName.text = User.DisplayName;

            

            StartCoroutine(LoadUserData());

            // wait 2 second, go to the show UI
            yield return new WaitForSeconds(2);

            UIManager.instance.MevatarShowScreen();
            confirmLoginText.text = "";// clear the text
            ClearLoginFields();
            ClearRegisterFields();

            AvatarManager._instance.InitAvatar();

            
        }
    }
    public void ClearLoginFields()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
        warningLoginText.text = "";
        confirmLoginText.text = "";
    }
    public void ClearRegisterFields()
    {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
        warningRegisterText.text = "";
        confirmRegisterText.text = "";
    }

    // function for the register button
    public void RegisterButton()
    {
        // call the register coroutine passing the email. password, and username
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            // if the username field is blank, show a warning
            warningRegisterText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            // if the password does not match, show a warning
            warningRegisterText.text = "Password Does Not Match";
        }
        else
        {
            // call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            // wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                // if there are errors, handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                // user has now been created, now get the result
                User = RegisterTask.Result;

                if (User != null)
                {
                    // create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    // call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);

                    // wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        // if there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username Set Failed";
                    }
                    else
                    {
                        // Username is now set, now return to login screen
                        warningRegisterText.text = "";
                        confirmRegisterText.text = "Registered";
                        yield return new WaitForSeconds(1);
                        UIManager.instance.LoginScreen();

                        // for initialize user database, using default value
                        InitData();

                        ClearRegisterFields();
                        ClearLoginFields();
                    }
                }
            }
        }
    }

    public void InitData()
    {
        StartCoroutine(UpdateUsernameAuth(usernameRegisterField.text));
        StartCoroutine(UpdateUsernameDatabase(usernameRegisterField.text));
        StartCoroutine(UpdateEmail(emailRegisterField.text));
        StartCoroutine(UpdateHead("1"));
        StartCoroutine(UpdateEyebrow("1"));
        StartCoroutine(UpdateEyes("1"));
    }

    private IEnumerator UpdateUsernameAuth(string _username)
    {
        // create a user profile and set the username
        UserProfile profile = new UserProfile { DisplayName = _username };

        // call the Firebase auth update user profile function passing the profile with the username
        var ProfileTask = User.UpdateUserProfileAsync(profile);
        //  wait until the task completes
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to create user profile task with {ProfileTask.Exception}");
        }
        else
        {
            // auth username is now updated
        }
    }

    private IEnumerator UpdateUsernameDatabase(string _username)
    {
        //Set the currently logged in user username in the database
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to update username task with {DBTask.Exception}");
        }
        else
        {
            //Database username is now updated
        }
    }

    public IEnumerator UpdateEmail(string _email)
    {
        // set the currently logged in user's avatar head model
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("email").SetValueAsync(_email);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to update email with {DBTask.Exception}");
        }
        else
        {
            //email is now updated
        }
    }

    public IEnumerator UpdateHead(string _head)
    {
        // set the currently logged in user's avatar head model
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("mevatar").Child("head").SetValueAsync(_head);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //head model is now updated
        }
    }
    public IEnumerator UpdateEyebrow(string _eyebrow)
    {
        // set the currently logged in user's avatar head model
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("mevatar").Child("eyebrow").SetValueAsync(_eyebrow);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //eyebrow model is now updated
            Debug.Log("update eyebrow chenggong");
        }
    }

    public IEnumerator UpdateEyes(string _eyes)
    {
        // set the currently logged in user's avatar head model
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("mevatar").Child("eyes").SetValueAsync(_eyes);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //eyes model is now updated
            Debug.Log("update eyes chenggong");
        }
    }

    public IEnumerator LoadUserData()
    {
        //Get the currently logged in user data
        var DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();
        //var DBTask2= DBreference.Child("users").Child(User.UserId).Child("mevatar").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            //No data exists yet
            displayHead.text = "1";
            displayEyebrow.text = "1";
            displayEyes.text = "1";
            //hairField.text = "1";
            UpdateEyebrow("1");
            //UpdateHair("1");
            UpdateHead("1");
            UpdateEyes("1");
            Debug.Log("No data exit");
        }
        else
        {
            Debug.Log("data been retrieved");
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;
            displayName.text = snapshot.Child("username").Value.ToString();
            usernameField.text = snapshot.Child("username").Value.ToString();
            displayEmail.text = snapshot.Child("email").Value.ToString();

            displayHead.text = snapshot.Child("mevatar").Child("head").Value.ToString();
            displayEyebrow.text = snapshot.Child("mevatar").Child("eyebrow").Value.ToString();
            displayEyes.text = snapshot.Child("mevatar").Child("eyes").Value.ToString();

            InitCurrentUser();
        }
    }

    public void InitCurrentUser()
    {
        currentUser = currentUserObject.GetComponent<User>();

        currentUser.SetEmail(displayEmail.text);
        currentUser.SetUsername(usernameField.text);

        currentMevatar = currentUserObject.GetComponent<Mevatar>();

        currentMevatar.SetHead(displayHead.text);
        currentMevatar.SetEyebrow(displayEyebrow.text);
        currentMevatar.SetEyes(displayEyes.text);

        currentUser.mevatar = currentMevatar;
    }

    public void SaveNameButton()
    {
        StartCoroutine(UpdateUsernameAuth(usernameField.text));
        StartCoroutine(UpdateUsernameDatabase(usernameField.text));
        StartCoroutine(LoadUserData());
    }

    public void CloseUserDataButton()
    {
        usernameField.text = displayName.text;
        UIManager.instance.UserDataUI.SetActive(false);
    }

    public void SignOutButton()
    {
        auth.SignOut();
        UIManager.instance.LoginScreen();
        ClearLoginFields();
        ClearRegisterFields();
        ClearDataDisplay();
        AvatarManager._instance.DefaultRotationAvatar();
    }

    public void ClearDataDisplay()
    {
        displayName.text = "";
        displayEmail.text = "";
        usernameField.text = "";
        displayHead.text = "";
        displayEyebrow.text = "";
        displayEyes.text = "";
    }

    public void TestEyebrowButton()
    {
        StartCoroutine(UpdateEyebrow(testEyebrowField.text));
        StartCoroutine(LoadUserData());
        AvatarManager._instance.LoadInitAvatar("eyebrow", testEyebrowField.text);
    }

    public void ChangeHead(string _head)
    {
        StartCoroutine(UpdateHead(_head));
        StartCoroutine(LoadUserData());
        //AvatarManager.instance.LoadInitAvatar("head", _head);
    }
    public void ChangeEyebrow(string _eyebrow)
    {
        StartCoroutine(UpdateHead(_eyebrow));
        StartCoroutine(LoadUserData());
        //AvatarManager.instance.LoadInitAvatar("eyebrow", _eyebrow);
    }
    public void ChangeEyes(string _eyes)
    {
        StartCoroutine(UpdateHead(_eyes));
        StartCoroutine(LoadUserData());
        //AvatarManager.instance.LoadInitAvatar("eyes", _eyes);
    }

    public void ChangeMevatar(string part, string num)
    {
        switch (part)
        {
            case "head":
                Debug.Log("names[1]= " + num);
                ChangeHead(num);
                //AvatarManager.instance.LoadInitAvatar("head", names[1]);
                break;
            case "eyebrow":
                Debug.Log("names[1]= " + num);
                ChangeEyebrow(num);
                //AvatarManager.instance.LoadInitAvatar("eyebrow", names[1]);
                break;
            case "eyeInL":
                Debug.Log("names[1]= " + num);
                ChangeEyes(num);
                //AvatarManager.instance.LoadInitAvatar("eyes", names[1]);
                break;
            default:
                Debug.Log("AvatarChangeButton Save data failed. names[0]= " + part + ", names[1]= " + num);
                break;
        }
    }
}
