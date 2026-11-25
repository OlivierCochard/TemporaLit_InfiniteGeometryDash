using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class PauseUiManager : MonoBehaviour
{   
    public SFXManager sfxManager;

    public PlayerJump playerJump;
    public AudioSource sfxSource;
    public AudioSource musicSource;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI mutedText;
    public GameObject parentObject;
    public ScoreUiManager scoreUiManager;
    bool isActive;

    bool needReset;

    void Start(){
        Off();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (isActive){
                Off();
                return;
            }

            On();
        }
    }

    public void On(bool reset = false){
        needReset = reset;

        parentObject.SetActive(true);
        isActive = true;

        playerJump.enabled = false;
        Time.timeScale = 0f;

        //AUDIO
        RefreshAudioButton();

        //SCORE
        int score = scoreUiManager.GetScore();
        int bestScore = PlayerPrefs.GetInt("bestScore_" + SceneManager.GetActiveScene().name, 0);
        scoreText.text = $"{score}/{bestScore} points";

        //CRAZYGAMES
        CrazyGamesManager.Instance.GameplayStop();
    }
    public void Off(){
        parentObject.SetActive(false);
        isActive = false;

        if (needReset && playerJump.IsGrounded()){
            playerJump.ResetAnimation();
            needReset = false;
        }

        playerJump.enabled = true;
        Time.timeScale = 1f;

        //CRAZYGAMES
        CrazyGamesManager.Instance.GameplayStart();
    }

    public void AudioButton(){
        sfxManager.ApplySFX("button");

        int currentMuted = PlayerPrefs.GetInt("muted", 0);
        if (currentMuted == 1) currentMuted = 0;
        else currentMuted = 1;

        sfxSource.mute = currentMuted == 1;
        musicSource.mute = currentMuted == 1;

        PlayerPrefs.SetInt("muted", currentMuted);
        PlayerPrefs.Save();

        RefreshAudioButton();
    }

    void RefreshAudioButton(){
        if (PlayerPrefs.GetInt("muted", 0) == 0){
            mutedText.text = "on";
        }
        else {
            mutedText.text = "off";
        }
    }

    public void MenuButton(){
        sfxManager.ApplySFX("button");
        SceneManager.LoadScene("MENU");
    }
}
