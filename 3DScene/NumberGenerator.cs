using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NumberGenerator : MonoBehaviour
{
    public TextMeshProUGUI A;
    public TextMeshProUGUI B;
    public Image checker_image;
    public Button nextButton;
    public Button tryAgain;

    private int minNum = 1;
    private int maxNum = 5;
    private int numA;
    private int numB;
    private int answer;
    private int currentLevel; 

    private int prevA = 0; 
    private int prevB = 0; 

    public PlayerInventory p;
    public UserProgressTracker uPT;
    private RepeatNumbers repeatNumbers = new RepeatNumbers();

    int inputAnswer; 

    int correctAnswer;
    int incorrectAnswer;


    void GenerateNumbers()
    {
        currentLevel = uPT.currentLevel;
        if  (repeatNumbers.repeat == true)
        {
           numA =   repeatNumbers.resused1;
           numB =   repeatNumbers.resused2;
        }
        else if (currentLevel == 0)
        {
            numA = Random.Range(minNum, maxNum);
            if (numA == prevA || numA == prevB )
            {
                numA = Random.Range(minNum, maxNum);
            }
            numB = Random.Range(minNum, maxNum);
            if (numB == prevB || numB == prevA)
            {
                numB = Random.Range(minNum, maxNum);
            }
        }
        else if (currentLevel == 1)
        {
            numA = Random.Range(minNum + 5, maxNum + 5);
            numB = Random.Range(minNum, maxNum);
        }
        else if (currentLevel == 2)
        {
            numA = Random.Range(minNum + 5, maxNum + 5);
            numB = Random.Range(minNum + 5, maxNum + 5);
        }
        else
        {
            numA = Random.Range(minNum + 5, maxNum + 5);
            numB = Random.Range(minNum + 2, maxNum + 2);
        }

        answer = numA + numB;
        A.text = numA.ToString();
        B.text = numB.ToString();
    }

    void Start()
    {
        GenerateNumbers();
        checker_image.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        tryAgain.gameObject.SetActive(false);
    }

    public void onClickDone()
    {   
        inputAnswer= p.NumberOfDiamonds;
        if (inputAnswer == answer)
        {
            checker_image.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
            uPT.UpdateCorrectAnswer();
            correctAnswer = uPT.correctAnswers;
        }
        else
        {
            tryAgain.gameObject.SetActive(true);
            repeatNumbers.RecordAnswer(numA, numB);
            uPT.UpdateIncorrectAnswer();
            incorrectAnswer = uPT.incorrectAnswers;
        }

    }

    public void onClickNext()
    {
        prevA = numA;
        prevB = numB;
        checker_image.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        repeatNumbers.RepeatValues();
        GenerateNumbers();
        repeatNumbers.increaseNewQuestionCount();
    }

    public void onClickTryAgain()
    {
        tryAgain.gameObject.SetActive(false);
    }

    public void onClickBack()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
