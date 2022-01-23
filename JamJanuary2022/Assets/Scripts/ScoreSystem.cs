using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public int currentScore = 0;
    Text scoreText;

    private void Start() {
        scoreText = GetComponent<Text>();
    }

    public void Add(int points){
        currentScore += points;
        UpdateText();
    }

    void UpdateText(){
        scoreText.text = currentScore.ToString();
    }
}
