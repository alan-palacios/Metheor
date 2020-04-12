using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    private int score;

    public PlayerData(int score){
              this.score = score;
   }

   public int getScore(){
             return score;
   }
}
