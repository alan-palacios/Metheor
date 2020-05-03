using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
          public int index = 0;
          public GameObject [] gameObjectSteps;
          public GameObject nextButton;
          public GameObject PreviousButton;
          public GeneralGameControl gameCtrl;
          bool tutorialOnGame;

             public void Show(bool tutorialOnGame){
                       this.tutorialOnGame = tutorialOnGame;
                       transform.gameObject.SetActive(true);
                       gameCtrl.SaveTutorial();
             }

             public void NextStep(){
                       gameObjectSteps[index].SetActive(false);
                       index++;
                       if (index>=gameObjectSteps.Length-1) {
                                 nextButton.SetActive(false);
                                 index = gameObjectSteps.Length-1;
                       }
                      PreviousButton.SetActive(true);
                       gameObjectSteps[index].SetActive(true);
            }

            public void PreviousStep(){
                      gameObjectSteps[index].SetActive(false);
                      index--;
                      if (index<=0) {
                                PreviousButton.SetActive(false);
                                index = 0;
                      }
                      nextButton.SetActive(true);
                      gameObjectSteps[index].SetActive(true);
            }

            public void CloseTutorial(){
                      transform.gameObject.SetActive(false);
                      if (tutorialOnGame) {
                                        gameCtrl.PlayGame();
                      }
            }
}
