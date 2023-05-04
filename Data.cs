
[System.Serializable]
public class Data
{
    public string userID;
    // The number of correct answers the user has given
    public int correctAnswers;

    // The number of incorrect answers the user has given
    public int incorrectAnswers;

    // The current level the user is on
    public int currentLevel;

    // The highest level the user has reached
    public int highestLevel;

    // The current streak of correct answers
    public int currentStreak;

    // The highest streak of correct answers the user has achieved
    public int highestStreak;
}
