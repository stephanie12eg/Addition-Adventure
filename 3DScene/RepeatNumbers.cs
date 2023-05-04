using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatNumbers
{
    private int questionCount = 0;
    private int incorrectCount = 0;
    private List<int> incorrectNumAs = new List<int>();
    private List<int> incorrectNumBs = new List<int>();
    public int resused1;
    public int resused2;
    public bool repeat = false;

    public void increaseNewQuestionCount(){
        questionCount++;
        Debug.Log("qc "+questionCount);
    }


    public void RecordAnswer(int numA, int numB)
    {
        incorrectCount++;
        incorrectNumAs.Add(numA);
        incorrectNumBs.Add(numB);
        Debug.Log("ic "+incorrectCount);
        Debug.Log("Recording variables " +numA+ " and " +numB+ " of incorrectly answered question");
    }

    public void RepeatValues(){
        if (questionCount == 3 && incorrectCount > 0)
        {
            repeat = true;
            int questionIndex = 0;

            resused1 = incorrectNumBs[questionIndex];
            resused2 = incorrectNumAs[questionIndex];

            incorrectNumAs.RemoveAt(questionIndex);
            incorrectNumBs.RemoveAt(questionIndex);

            incorrectCount = Mathf.Max(0, incorrectCount - 1);
            questionCount = 0;

            Debug.Log("Repeating variables " +resused1+ " and " + resused2);

        }
        else{
            repeat = false;
        }
    }

}