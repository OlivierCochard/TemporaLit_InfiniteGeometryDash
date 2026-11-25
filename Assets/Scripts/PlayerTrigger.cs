using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{   
    public SFXManager sfxManager;

    public EndUiManager endUiManager;
    public ScoreUiManager scoreUiManager;

    public WorldGenerator worldGenerator;
    public PlayerJump playerJump;

    void Awake(){
        Time.timeScale = 1f;
    }

    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.tag == "Chunk"){
            scoreUiManager.AddAndRefreshScore();
            worldGenerator.GenerateChunk();

            sfxManager.ApplySFX("chunk");
        }

        if (col.gameObject.tag == "Death"){
            Time.timeScale = 0f;
            Destroy(playerJump);

            int score = scoreUiManager.End();
            endUiManager.On(score);

            sfxManager.ApplySFX("lose");
        }
    }
}