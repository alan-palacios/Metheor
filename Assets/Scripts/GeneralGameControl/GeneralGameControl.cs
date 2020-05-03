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
          public Text coinText;

          public GameObject  restartButton;
          public GameObject  menuButton;
          public GameObject  backgroundImg;

          public GameObject  pauseButton;

          private bool paused=true;

          [SerializeField]
          private static bool haveLost=false;
          private static bool passLevel=false;

          public GameObject GameElements;
          public GameObject mapGenerator;
          public GameObject meteorEnemy;

          public GameObject MenuElements;
          public GameObject solarSystemParent;
           GameObject solarSystemInstanced;

          public GameObject blackHoleParent;
           GameObject blackHoleInstanced;

          public Text highScore;
          public Text totalCoins;
          public GameObject options;
          public GameObject credits;
          public GameObject shop;
          public float timeAfterLose;

          public Camera gameCamera;
          public Color [] bgColors;
          static PlayerData playerData;

          public static int level = 0;
          public SoundControl soundCtrl;
          public Tutorial tutorial;

    void Start()
    {
              if (haveLost) {
                        PlayGame();
             }else{
                       StartCoroutine(ShowMenu());
             }

    }



    public IEnumerator ShowMenu(){
              soundCtrl.PlaySound("menu");
              Time.timeScale = 1;
              solarSystemInstanced = GameObject.Instantiate(solarSystemParent, Vector3.zero  , Quaternion.identity, transform ) as GameObject;
              meteorEnemy.SetActive(false);
              yield return null;
              mapGenerator.SetActive(false);

              if (SaveSystem.LoadData("player.dta")!=null) {
                        playerData = new PlayerData(PlayerMove.score,0,SaveSystem.LoadData("player.dta").getTutorialViewed());
              }else{
                        playerData = new PlayerData(PlayerMove.score,0,false);
                        SaveSystem.SaveData(playerData ,"player.dta");
              }
              highScore.text = SaveSystem.LoadData("player.dta").getScore().ToString();
              totalCoins.text = SaveSystem.LoadData("player.dta").getCoins().ToString();
    }

    public void PlayGame()
    {
              if (SaveSystem.LoadData("player.dta").getTutorialViewed()) {
                        Time.timeScale = paused?1:0;
                        paused = false;
                        if (passLevel) {
                                  blackHoleInstanced = GameObject.Instantiate(blackHoleParent, Vector3.zero  , Quaternion.identity, transform ) as GameObject;
                                  Destroy(blackHoleInstanced.GetComponent<Collider>() );
                                  gameCamera.backgroundColor = bgColors[level%bgColors.Length];
                        }
                        coinText.text = playerData.getCoins().ToString();
                        scoreText.text = playerData.getScore().ToString();
                        passLevel=false;
                        Destroy(solarSystemInstanced);

                        playerElements.SetActive(true);
                        mapGenerator.SetActive(true);
                        meteorEnemy.SetActive(true);
                        MenuElements.SetActive(false);
                        soundCtrl.PlaySound("bgMusic");
                        soundCtrl.PlaySound("fire");
              }else{
                        Time.timeScale = 0;
                        tutorial.Show(true);
              }

    }

// in game calls

    public IEnumerator PassLevel()
    {
              yield return new WaitForSeconds(timeAfterLose);
              PlayerMove.score=0;
              level++;
              passLevel=true;
              haveLost=true;
              SceneManager.LoadScene("MainScene");
    }

    public IEnumerator OnLost(){
              Time.timeScale =0.8f;
              yield return new WaitForSeconds(timeAfterLose);
              Time.timeScale = 0;
              pauseButton.SetActive(false);
              restartButton.SetActive(true);
              menuButton.SetActive(true);
              backgroundImg.SetActive(true);
   }

   public void UpdateScore ( int scoreGived){
          soundCtrl.PlaySound("score");
            playerData.setScore( playerData.getScore() + scoreGived);
            scoreText.text = playerData.getScore().ToString();
            gameCamera.fieldOfView+=0.1f;
            if (gameCamera.fieldOfView>25) {
                      gameCamera.fieldOfView=25;
            }
   }

   public void GiveCoin(){
            playerData.setCoins( playerData.getCoins() +1 );
            coinText.text = playerData.getCoins().ToString();
  }

// input button calls
    public void PauseGame(){
            soundCtrl.StopSound("fire");
              Time.timeScale = paused?1:0;
              paused = !paused;
              restartButton.SetActive(!restartButton.activeSelf);
              menuButton.SetActive(!menuButton.activeSelf);
              backgroundImg.SetActive(!backgroundImg.activeSelf);
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
            paused=false;
            SaveActualData();
            PlayerMove.score=0;
            level=0;
            SceneManager.LoadScene("MainScene");
 }

 void Update(){
          if (Input.GetKey("up")) {
                    PauseGame();
          }
}

public void ShowOptions(){
          credits.SetActive(false);
          options.SetActive(!options.activeSelf);
}



public void ShowCredits(){
          credits.SetActive(true);
          options.SetActive(false);
}

/// Saving Data
   public void SaveActualData(){
             if (playerData.getScore() < SaveSystem.LoadData("player.dta").getScore()) {
                       playerData.setScore( SaveSystem.LoadData("player.dta").getScore());
             }
             playerData.setCoins(playerData.getCoins() +SaveSystem.LoadData("player.dta").getCoins() );
             SaveSystem.SaveData( playerData , "player.dta");
             playerData.Reiniciar();
   }

   public void BorrarDatos(){
             playerData = new PlayerData(0,0,false);
             SaveSystem.SaveData( playerData , "player.dta");
             highScore.text = SaveSystem.LoadData("player.dta").getScore().ToString();
             totalCoins.text = SaveSystem.LoadData("player.dta").getCoins().ToString();
   }
   public void SaveTutorial(){
             playerData.setTutorialViewed(true);
             SaveActualData();
   }

}
