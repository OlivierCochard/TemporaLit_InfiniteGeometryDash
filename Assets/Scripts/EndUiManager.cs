using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EndUiManager : MonoBehaviour
{
    public PauseUiManager pauseUiManager;
    public Button pauseButton;

    TextMeshProUGUI titleText;
    TextMeshProUGUI scoreText;
    bool activated;

    void Awake(){
        titleText = GetComponentsInChildren<TextMeshProUGUI>()[0];
        scoreText = GetComponentsInChildren<TextMeshProUGUI>()[1];

        //PlayerPrefs.DeleteAll();
    }

    void Start(){
        gameObject.SetActive(false);
    }

    public void On(int score){
        if (activated) return;

        activated = true;
        gameObject.SetActive(true);
        int bestScore = PlayerPrefs.GetInt("bestScore_" + SceneManager.GetActiveScene().name, 0);

        //TITLES &/OR SAVES
        if (score > bestScore){
            titleText.text = "NEW HIGHSCORE!";
            scoreText.text = $"You scored {score} points";

            PlayerPrefs.SetInt("bestScore_" + SceneManager.GetActiveScene().name, score);
            PlayerPrefs.Save();
        }
        else{
            titleText.text = "YOU LOST";
            scoreText.text = $"You got {score} points";
        }

        Destroy(pauseButton);
        Destroy(pauseUiManager);
    }

    public void MenuButton(){
        SceneManager.LoadScene("MENU");
    }

    public void ReplayButton(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}
