using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    private int score;
    private int coins;
    private bool tutorialViewed;
    public PlayerData(int score, int coins, bool tutorialViewed){
              this.score = score;
              this.coins = coins;
              this.tutorialViewed = tutorialViewed;
   }

   public int getScore(){
             return score;
   }
   public int getCoins(){
             return coins;
   }
   public bool getTutorialViewed(){
             return tutorialViewed;
   }

   public void setScore( int score){
             this.score = score;
   }
   public void setCoins(int coins){
             this.coins = coins;
   }

   public void setTutorialViewed(bool tutorialViewed){
             this.tutorialViewed = tutorialViewed;
   }
   public void Reiniciar(){
             score=0;
             coins=0;
   }
}
