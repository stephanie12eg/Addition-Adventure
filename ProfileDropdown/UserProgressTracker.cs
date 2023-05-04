using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using Firebase;
using Firebase.Auth;
using System.Linq;
using System.Web;

public class UserProgressTracker : MonoBehaviour
{
    // Creating an instance of the database object 
    public DB db = new DB();

    // Unique identifier for the user
    public string userID {get; set;}

    // The number of correct answers the user has given
    public int correctAnswers {get; set;}

    // The number of incorrect answers the user has given
    public int incorrectAnswers {get; set;}

    // The current level the user is on
    public int currentLevel {get; set;}

    // The highest level the user has reached
    public int highestLevel {get; set;}

    // The current streak of correct answers
    public int currentStreak {get; set;}

    // The highest streak of correct answers the user has achieved
    public int highestStreak{get; set;}

    public TextMeshProUGUI profileName, profileName2, profileEmail, userCA, userIA, userL, userS;

    public void GetUserDetails(string uI)
    {
        userID = uI;
        Debug.Log("this is the user id"+ userID);
    }


    private void Awake()
    {
        userID = FirebaseController.fC.userId;

        if (File.Exists(Application.dataPath + "/SaveData.json"))
        {
            LoadFromJson();
        }

        profileEmail.text = FirebaseController.fC.emailf;
        profileName.text = FirebaseController.fC.usersName.ToString();
        profileName2.text = FirebaseController.fC.usersName.ToString();
        userCA.text = correctAnswers.ToString();
        userIA.text = incorrectAnswers.ToString();
        userL.text =  currentLevel.ToString();
        userS.text = highestStreak.ToString();
            
    } 
   
    // Update the user's progress when a correct answer is given
    public void UpdateCorrectAnswer()
    {
        correctAnswers++;
        currentStreak++;

        if (currentStreak > highestStreak)
        {
            highestStreak = currentStreak;
        }

        LevelUp();
        SaveToJson();
    }

    // Update the user's progress when an incorrect answer is given
    public void UpdateIncorrectAnswer()
    {
        incorrectAnswers++;
        currentStreak = 0;
        SaveToJson();
    }

    // Update the user's level based on their progress
    private void LevelUp()
    {
        if (correctAnswers >= 8 && incorrectAnswers == 0)
        {
            currentLevel++;
        }
        else if (Mathf.Floor(correctAnswers / 8) > 1 && incorrectAnswers == Mathf.Floor(correctAnswers / 8))
        {
            currentLevel++;
        }

        if (currentLevel > highestLevel)
        {
            highestLevel = currentLevel;
        }
        SaveToJson();
    }
    public void SaveToJson()
    {
        userID = FirebaseController.fC.userId;
        Data data = new Data();
        data.userID = userID;
        data.correctAnswers = correctAnswers;
        data.incorrectAnswers = incorrectAnswers;
        data.currentLevel = currentLevel;
        data.highestLevel = highestLevel;
        data.currentStreak = currentStreak;
        data.highestStreak = highestStreak;
        
        if (db.users.Count == 0){
            db.users.Add(data);
        }
        else
        {
            bool found = false;
            for (int iCount = 0; iCount < db.users.Count; iCount++)
            {
                if (db.users[iCount].userID.Equals(userID))
                {
                    found = true;
                    db.users[iCount] = data;
                    Debug.Log("updating users data");
                }
            }
            if (found == false)
            {
                db.users.Add(data);
                Debug.Log("making new users data");
            }
        }



        string json = JsonUtility.ToJson(db, true);
        File.WriteAllText(Application.dataPath + "/SaveData.json", json);
        
    }
 
    public void LoadFromJson()
    {
        userID = FirebaseController.fC.userId;
        string json = File.ReadAllText(Application.dataPath + "/SaveData.json");
        db = JsonUtility.FromJson<DB>(json);

        Data oFound  = db.users.Find(r => r.userID == userID);

        if(oFound == null)
        {
            Debug.Log("Not Found");
        }
        else
        {
            correctAnswers = oFound.correctAnswers;
            incorrectAnswers = oFound.incorrectAnswers;
            currentLevel = oFound.currentLevel;
            highestLevel = oFound.highestLevel;
            currentStreak = oFound.currentStreak;
            highestStreak = oFound.highestStreak;
        }
    }

    public void LogOut()
    {
        profileName.text = "";
        profileEmail.text = "";
    }

}