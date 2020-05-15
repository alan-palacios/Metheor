using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class CharactersData
{
    // 0 selected
    // 1 owned
    // 2 to buy
    private int [] characters;

    public CharactersData(int [] characters){
                this.characters = new int[characters.Length];
              Array.Copy(characters,this.characters,characters.Length);
   }

   public int GetCharactersLength(){
       return characters.Length;
   }
   public int[] getCharacters(){
             return characters;
   }

   public void setCharacters(int[] characters){
             this.characters = characters;
   }

   public void SetIndexInfo(int index, int info){
       characters[index]=info;
   }
   public int GetIndexInfo(int index){
       return characters[index];
   }

   public int GetSelectedIndex(){
       for (int i=0; i<characters.Length; i++) {
           if (characters[i]==0) {
               return i;
           }
       }
       return 0;
   }

   public void SetSelectedIndex(int index){
       for (int i=0; i<characters.Length; i++) {
           if (characters[i]==0) {
               characters[i]=1;
           }
       }
       characters[index]=0;

   }

   public void UpdateCharactersList(int newLongitude){
       int[] tempArray = new int[newLongitude];
       for (int i=0; i<tempArray.Length; i++) {
           if (i<characters.Length) {
               tempArray[i] = characters[i];
           }else{
               tempArray[i]=2;
           }
       }
       characters = new int[newLongitude];
       Array.Copy(tempArray,characters,newLongitude);
   }


}
