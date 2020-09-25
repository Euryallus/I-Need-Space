using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    private AudioSource ambiance; //background noise component
    private AudioSource bgMusic; //background music component
    private void Start()
    {
        ambiance = gameObject.GetComponents<AudioSource>()[0]; //gets 1st audio source component (noise)
        ambiance.time = PlayerPrefs.GetFloat("backgroundPlaytime", 0.0f); //sets the "play from" time to value stored in playerPrefs
        ambiance.Play(); //sets audio to play

        bgMusic = gameObject.GetComponents<AudioSource>()[1]; //same process as "ambiance"
        bgMusic.time = PlayerPrefs.GetFloat("backgroundMusic", 0.0f);
        bgMusic.Play();

        AudioListener.volume = PlayerPrefs.GetFloat("Volume", 0.5f); //sets volume to that set in playerPrefs

        updateMuted();
    }

    public void SetTime()
    {
        PlayerPrefs.SetFloat("backgroundPlaytime", gameObject.GetComponents<AudioSource>()[0].time); //stores play-from time for each background audio (run @ end of level)
        PlayerPrefs.SetFloat("backgroundMusic", gameObject.GetComponents<AudioSource>()[1].time);
    }

    private void updateMuted()
    {
         switch (PlayerPrefs.GetInt("Muted", 0)){ //sets volume to 0 if muted, stored volume if unmuted, or 0.5 if no volume data exists
            case 1:
                AudioListener.volume = 0;
                break;
            case 0:
                AudioListener.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
                break;
            default:
                AudioListener.volume = 0.5f;
                break;
        }
    }
    private void FixedUpdate()
    {
        updateMuted(); //checks each frame if settings have been altered
    }
}
