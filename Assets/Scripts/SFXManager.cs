using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource sfxSource;

    public AudioClip lose;
    public AudioClip jump;
    public AudioClip chunk;
    public AudioClip button;

    public Vector2 pitchMinMax;

    public void ApplySFX(string sfxName){
        AudioClip audioClip = null;

        if (sfxName == "lose") audioClip = lose;
        else if (sfxName == "jump") audioClip = jump;
        else if (sfxName == "chunk") audioClip = chunk;
        else if (sfxName == "button") audioClip = button;

        if (audioClip != null){
            sfxSource.pitch = Random.Range(pitchMinMax.x, pitchMinMax.y);
            sfxSource.PlayOneShot(audioClip);
        }
    }
}