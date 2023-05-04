using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using System;
using System.Threading.Tasks;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class FirebaseController : MonoBehaviour
{
    public static FirebaseController fC;
    public GameObject loginPanel, signupPanel, resetPanel, errorPanel;
    public InputField loginEmail, loginPassword, signupName, signupUsername, signupEmail, signupPassword, signupCPassword, resetPassEmail;
    public TextMeshProUGUI errorTitle, errorMessage;
    public Toggle rememberMe, rememberMeSignup;


    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    //for user progress
    public string userId {get; set;}
    public string usersName{get; set;}
    public string emailf {get; set;}

    bool isSignIn = false;
    private void Awake()
    {
        if(fC !=null){
            Destroy(this.gameObject);
            return;
        }
        fC = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                InitializeFirebase();

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }
    
    public void OpenLoginPanel ()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        resetPanel.SetActive(false);
        CloseError();
    }
    public void OpenSignupPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(true);
        resetPanel.SetActive(false);
        CloseError();
    }

    public void OpenResetPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        resetPanel.SetActive(true);
        CloseError();
    }

    public void LoginUser()
    {
        if (string.IsNullOrEmpty (loginEmail.text) &&string.IsNullOrEmpty (loginPassword.text))
        {
            ShowError("Error", "* Field is empty");
            return;
        }
        SignInUser(loginEmail.text,loginPassword.text);
    }

    public void SignUpUser()
    {
        if (string.IsNullOrEmpty(signupEmail.text) && string.IsNullOrEmpty(signupPassword.text) && string.IsNullOrEmpty(signupCPassword.text)&& string.IsNullOrEmpty(signupName.text))
        {
            ShowError("Error", "* Field is empty");
            return;
        }
        
        CreateUser(signupEmail.text, signupPassword.text, signupName.text);
        SignInUser(signupEmail.text, signupPassword.text);
    }

    public void ResetPassword()
    {
        if (string.IsNullOrEmpty(resetPassEmail.text))
        {
            ShowError("Error", "* Email is empty");
            return;
        }
        // Do reset password
    }

    public void ShowError(string title, string message)
    {
        errorTitle.text = title;
        errorMessage.text = message;
        errorPanel.SetActive(true);
    }

    public void CloseError()
    {
        errorTitle.text = "";
        errorMessage.text = "";
        errorPanel.SetActive(false);
    }

    public void CreateUser(string email, string password, string UserName)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

        });
        UpdateUserProfile(UserName);
    }

    public void SignInUser(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            

            emailf = user.Email;
            usersName = user.DisplayName;
            userId = user.UserId;

            Debug.Log(emailf+ usersName);

            //dC.GetUserDetails(user.Email, user.DisplayName);
    
            });    
    }

    void InitializeFirebase() 
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs) 
    {
        if (auth.CurrentUser != user) {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null) {
            Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn) {
                Debug.Log("Signed in " + user.UserId);
                isSignIn = true;
            }
        }
    }

    void OnDestroy() 
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    public void UpdateUserProfile(string UserName)
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null) {
            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile {
                DisplayName = UserName,
                PhotoUrl = new System.Uri("https://via.placeholder.com/150C/0%20https://placeholder.com/"),
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task => {
                if (task.IsCanceled) {
                Debug.LogError("UpdateUserProfileAsync was canceled.");
                return;
                }
                if (task.IsFaulted) {
                Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                return;
                }

                Debug.Log("User profile updated successfully.");
            });
        }
    }

    bool isSigned = false;
    void Update()
    {
        if(isSignIn)
        {
            if(isSigned)
            {
                isSigned = true;
                //Debug.Log(username, userId, email, usersName);
                //profileName.text = "" + user.DisplayName;
                //profileEmail.text = "" + user.Email;
                        
            }

        }
    }
    
    public void LogOut()
    {
        auth.SignOut();
    }

    //void SetMenuDetails{

    //}

    // Load the user's progress from persistent storage
   /*  private void LoadUser()
    {
        // Load progress for the specified user ID from persistent storage
        // and assign values to the appropriate variables
        // Example using PlayerPrefs:
        username = PlayerPrefs.GetString(userID + "_username", 0);
    }

    // Save the user's progress to persistent storage
    private void SaveUser()
    {
        // Save progress for the specified user ID to persistent storage
        // Example using PlayerPrefs:
        PlayerPrefs.SetString(userID + "_username", username);
    } */

}
