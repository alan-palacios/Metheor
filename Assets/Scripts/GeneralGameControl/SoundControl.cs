using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{
    AudioSource audioSource;
    public AudioSource playerAudioSource;
    public AudioSource inGameMusicSource;
    public AudioClip [] music;
    public AudioSource menuMusicSource;
    public AudioClip scoreSound;
    public AudioClip clickSound;
    public AudioClip glass;
    public AudioClip ice;
    public AudioClip fire;
    public AudioClip hit;
    public AudioClip coin;

    void Start(){
        audioSource = GetComponent<AudioSource>();
        if ( !PlayerPrefs.HasKey("SoundEffectsOn")) PlayerPrefs.SetInt("SoundEffectsOn", 1);
        if ( !PlayerPrefs.HasKey("MusicOn")) PlayerPrefs.SetInt("MusicOn", 1);
    }

    public void PlayClickSound(){
        audioSource.PlayOneShot(clickSound);
    }

    public void PlaySound(string name){
        switch (name) {
            case "score":
                playerAudioSource.PlayOneShot(scoreSound);
                break;
            case "freeze":
                playerAudioSource.PlayOneShot(ice);
                break;
            case "unfreeze":
                playerAudioSource.PlayOneShot(glass);
                break;
            case "fire":
                playerAudioSource.Play();
                break;
            case "coin":
                playerAudioSource.PlayOneShot(coin);
                break;
            case "hit":
                playerAudioSource.PlayOneShot(hit);
                break;
            case "menu":
                menuMusicSource.Play();
                break;
            case "bgMusic":
                int index = Random.Range(0, music.Length);
                inGameMusicSource.clip = music[index];
                inGameMusicSource.Play();
                break;
        }
    }

    public void StopSound(string name){
        switch (name) {
            case "fire":
                playerAudioSource.Stop();
                break;
        }
    }


    public void ChangeSoundEffectsState(){
              PlayerPrefs.SetInt("SoundEffectsOn", PlayerPrefs.GetInt("SoundEffectsOn")==1?0:1 );

              if ( PlayerPrefs.GetInt("SoundEffectsOn") ==1 ) {
                        playerAudioSource.volume = 0.7f;
              }else{
                        playerAudioSource.volume = 0;
              }
   }

   public void ChangeMusicState(){
             PlayerPrefs.SetInt("MusicOn", PlayerPrefs.GetInt("MusicOn")==1?0:1 );

             if ( PlayerPrefs.GetInt("MusicOn") ==1 ) {
                       playerAudioSource.volume = 0.65f;
             }else{
                       playerAudioSource.volume = 0;
             }
   }
}
