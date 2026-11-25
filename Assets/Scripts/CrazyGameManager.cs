using UnityEngine;
using CrazyGames;

public class CrazyGamesManager : MonoBehaviour
{
    public static CrazyGamesManager Instance;
    private bool isGameplayActive;

    public bool IsReady { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (CrazySDK.IsAvailable)
        {
            CrazySDK.Init(() =>
            {
                IsReady = true;
                Debug.Log("CrazyGames SDK initialized");
            });
        }
        else
        {
            Debug.Log("CrazyGames SDK unavailable");
        }
    }

    public void GameplayStart(){
        if (!IsReady || isGameplayActive)
            return;

        isGameplayActive = true;
        CrazySDK.Game.GameplayStart();
    }

    public void GameplayStop(){
        if (!IsReady || !isGameplayActive)
            return;

        isGameplayActive = false;
        CrazySDK.Game.GameplayStop();
    }
}