using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GeneralGameControl : MonoBehaviour
{
          public GameObject playerElements;
          public PlayerMove playerMove;
          public Text scoreText;

          public GameObject  hidenRestartButton;
          public GameObject  pauseButton;

          private bool paused=true;

          [SerializeField]
          private static bool haveLost=false;

          public GameObject GameElements;
          public GameObject MenuElements;
          public Camera gameCamera;

          public float timeAfterLose;

    void Start()
    {
          if (haveLost) {
                    PlayGame();
          }else{
                    StopGame();
          }

    }

    public void StopGame(){
              if (paused) {
                        Time.timeScale = 0;
                        paused=true;
              }

    }

    public void PlayGame()
    {
              Time.timeScale = paused?1:0;
              paused = false;
              playerElements.SetActive(true);
              GameElements.SetActive(true);
              MenuElements.SetActive(false);
    }

    public void RestartGame()
    {
              haveLost=true;
              SceneManager.LoadScene("MainScene");

    }

    public IEnumerator OnLost(){
              Time.timeScale =0.8f;
              yield return new WaitForSeconds(timeAfterLose);
              Time.timeScale = 0;
              pauseButton.SetActive(false);
              hidenRestartButton.SetActive(true);
   }
    public void PauseGame()
    {
              Time.timeScale = paused?1:0;
              paused = !paused;
              hidenRestartButton.SetActive(!hidenRestartButton.activeSelf);
    }

    public void UpdateScore ()
    {
        scoreText.text = "" + playerMove.score.ToString ();
        gameCamera.fieldOfView+=0.1f;

    }

    public void UpdateScore (string txt)
    {
        scoreText.text =  txt +"\n"+playerMove.score.ToString ();
    }
}
