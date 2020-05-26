using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AdsPersistance
{
    private bool adsRemoved;
    public AdsPersistance(bool adsRemoved){
              this.adsRemoved = adsRemoved;
   }

   public bool getAdsRemoved(){
             return adsRemoved;
   }

   public void setAdsRemoved(bool adsRemoved){
             this.adsRemoved = adsRemoved;
   }
}
