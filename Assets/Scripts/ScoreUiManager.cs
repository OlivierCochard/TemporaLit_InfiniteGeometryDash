using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ScoreUiManager : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    int score;

    void Awake(){
        scoreText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void RefreshScoreText(){
        scoreText.text = "" + score;
    }

    public void AddAndRefreshScore(){
        score++;
        RefreshScoreText();
    }

    public int End(){
        gameObject.SetActive(false);
        return score;
    }

    public int GetScore(){
        return score;
    }
}
