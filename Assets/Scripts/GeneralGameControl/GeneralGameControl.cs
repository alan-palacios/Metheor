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

          public GameObject  restartButton;
          public GameObject  menuButton;

          public GameObject  pauseButton;

          private bool paused=true;

          [SerializeField]
          private static bool haveLost=false;
          private static bool passLevel=false;
          private static int scoreViewed=0;

          public GameObject GameElements;
          public GameObject mapGenerator;

          public GameObject MenuElements;
          public GameObject solarSystemParent;
           GameObject solarSystemInstanced;

          public GameObject blackHoleParent;
           GameObject blackHoleInstanced;

          public Text highScore;

          public Camera gameCamera;

          public float timeAfterLose;

    void Start()
    {

              if (haveLost) {
                       PlayGame();
             }else{
                       StartCoroutine(StopGame());
             }

    }

    public IEnumerator StopGame(){
              yield return null;
              mapGenerator.SetActive(false);
              if (SaveSystem.LoadData("player.dta")!=null) {
                        highScore.text = SaveSystem.LoadData("player.dta").getScore().ToString();
              }else{
                        highScore.text = "0";
                        SaveSystem.SaveData(0,"player.dta");
              }

              solarSystemInstanced = GameObject.Instantiate(solarSystemParent, Vector3.zero  , Quaternion.identity, transform ) as GameObject;

    }

    public void PlayGame()
    {
              Time.timeScale = paused?1:0;
              paused = false;
              if (passLevel) {
                        blackHoleInstanced = GameObject.Instantiate(blackHoleParent, Vector3.zero  , Quaternion.identity, transform ) as GameObject;
                        Destroy(blackHoleInstanced.GetComponent<Collider>() );
              }
              passLevel=false;
              Destroy(solarSystemInstanced);
              playerElements.SetActive(true);
              mapGenerator.SetActive(true);
              MenuElements.SetActive(false);
    }

    public IEnumerator PassLevel()
    {
              yield return new WaitForSeconds(timeAfterLose);
              scoreViewed = RealScore();
              PlayerMove.score=0;

              passLevel=true;
              RestartGame();
    }

    public void GoToMenu()
    {
              haveLost=false;
              DoSelection();
    }

    public void RestartGame()
    {
              haveLost=true;
              DoSelection();
    }

    public void DoSelection(){
              if (RealScore() > SaveSystem.LoadData("player.dta").getScore()) {
                        SaveSystem.SaveData(RealScore(), "player.dta");
              }
              PlayerMove.score=0;
              if (!passLevel) {
                    scoreViewed=0;
              }

              SceneManager.LoadScene("MainScene");
   }

    public IEnumerator OnLost(){
              Time.timeScale =0.8f;
              if (RealScore() > SaveSystem.LoadData("player.dta").getScore()) {
                        SaveSystem.SaveData( RealScore(), "player.dta");
              }
              yield return new WaitForSeconds(timeAfterLose);
              Time.timeScale = 0;
              pauseButton.SetActive(false);

              restartButton.SetActive(true);
              menuButton.SetActive(true);
   }

   public void BorrarDatos()
   {
             SaveSystem.SaveData(0, "player.dta");
   }

    public void PauseGame()
    {
              Time.timeScale = paused?1:0;
              paused = !paused;
              restartButton.SetActive(!restartButton.activeSelf);
              menuButton.SetActive(!menuButton.activeSelf);
    }

    public void UpdateScore ()
    {
        scoreText.text = "" + RealScore().ToString();
        gameCamera.fieldOfView+=0.1f;

    }

    public void UpdateScore (string txt)
    {
        scoreText.text =  txt +"\n"+RealScore().ToString();
    }

    public int RealScore(){
              return scoreViewed + PlayerMove.score;
   }
}
