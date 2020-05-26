using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("sound options")]
    public Button sfxButton;
    public Sprite SFXMuted;
    public Sprite SFXUnmuted;

    public Button musicButton;
    public Sprite MusicMuted;
    public Sprite MusicUnmuted;



    void Start(){
        audioSource = GetComponent<AudioSource>();
        if ( !PlayerPrefs.HasKey("SoundEffectsOn")) PlayerPrefs.SetInt("SoundEffectsOn", 1);
        if ( !PlayerPrefs.HasKey("MusicOn")) PlayerPrefs.SetInt("MusicOn", 1);


        if (PlayerPrefs.GetInt("SoundEffectsOn")==0) {
            playerAudioSource.mute = true;
            sfxButton.GetComponent<Image>().sprite = SFXMuted;
        }
        if (PlayerPrefs.GetInt("MusicOn")==0) {
            inGameMusicSource.mute = true;
            menuMusicSource.mute = true;
            musicButton.GetComponent<Image>().sprite = MusicMuted;
        }



    }

    public void PlayClickSound(){

        if (PlayerPrefs.GetInt("SoundEffectsOn") == 1) {
            audioSource.PlayOneShot(clickSound);
        }
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
              playerAudioSource.mute = !playerAudioSource.mute;

              if (playerAudioSource.mute) {
                  sfxButton.GetComponent<Image>().sprite = SFXMuted;
                  PlayerPrefs.SetInt("SoundEffectsOn", 0 );
              }else{
                  sfxButton.GetComponent<Image>().sprite = SFXUnmuted;
                  PlayerPrefs.SetInt("SoundEffectsOn", 1 );
              }
   }

   public void ChangeMusicState(){
             inGameMusicSource.mute = !inGameMusicSource.mute;
             menuMusicSource.mute = !menuMusicSource.mute;

             if ( inGameMusicSource.mute ) {
                 musicButton.GetComponent<Image>().sprite = MusicMuted;
                 PlayerPrefs.SetInt("MusicOn", 0 );
             }else{
                 musicButton.GetComponent<Image>().sprite = MusicUnmuted;
                 PlayerPrefs.SetInt("MusicOn", 1 );
             }
   }
}
