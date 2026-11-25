using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MainMenuUiManager : MonoBehaviour
{
    public SFXManager sfxManager;

    public AudioSource sfxSource;
    public AudioSource musicSource;
    public Image audioButtonImage;
    public Sprite mutedSprite;
    public Sprite notMutedSprite;
    public GameObject menuObject;
    public GameObject playObject;

    public GameObject[] levelButtons;


    void Start(){
        MenuSetActive(true);

        //CRAZYGAMES
        CrazyGamesManager.Instance.GameplayStop();
    }

    void RefreshAudioButton(){
        if (PlayerPrefs.GetInt("muted", 0) == 0){
            audioButtonImage.sprite = notMutedSprite;
        }
        else {
            audioButtonImage.sprite = mutedSprite;
        }
    }

    void RefreshHighscoreButton(){
        foreach (GameObject levelButton in levelButtons){
            TextMeshProUGUI highscoreText = levelButton.GetComponentsInChildren<TextMeshProUGUI>()[1];
            int highscoreValue = PlayerPrefs.GetInt("bestScore_" + levelButton.name, 0);

            highscoreText.text = "(" + highscoreValue + ")";
            if (highscoreValue == 0){
                highscoreText.text = "-";
            }
        }
    }

    void MenuSetActive(bool boolean){
        menuObject.SetActive(boolean);
        playObject.SetActive(!boolean);

        if (boolean){
            RefreshAudioButton();
        }
        else {
            RefreshHighscoreButton();
        }
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

    public void PlayButton(){
        sfxManager.ApplySFX("button");
        MenuSetActive(false);
    }

    public void QuitButton(){
        sfxManager.ApplySFX("button");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void BackButton(){
        sfxManager.ApplySFX("button");
        MenuSetActive(true);
    }

    public void SelectLevel(string sceneName){
        //CRAZYGAMES
        CrazyGamesManager.Instance.GameplayStart();

        sfxManager.ApplySFX("button");
        SceneManager.LoadScene(sceneName);
    }
}
