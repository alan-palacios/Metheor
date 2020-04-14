using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    private int score;
    private int coins;

    public PlayerData(int score, int coins){
              this.score = score;
              this.coins = coins;
   }

   public int getScore(){
             return score;
   }
   public int getCoins(){
             return coins;
   }

   public void setScore( int score){
             this.score = score;
   }
   public void setCoins(int coins){
             this.coins = coins;
   }
   public void Reiniciar(){
             score=0;
             coins=0;
   }
}
