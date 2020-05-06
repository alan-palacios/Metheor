using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;
using System;

public class Shop : MonoBehaviour
{
          [SerializeField]
          public CharactersList charactersList;          
          private int characterSelected=0;
          public int columns;

          public GameObject storeItem;
          public GameObject charactersGroup;

          [Header("Display Settings")]
          public GameObject store;
          public GameObject buyOptions;
          public float xPadding;
          public float yPadding;
          public float upMargin;
          public float itemHeight;
          public float minScrollHeight;
          public float minScrollWidth;
          public Vector2 initCoords = new Vector2(-200, 865);


              public void CloseShop(){
                        buyOptions.SetActive(false);
                        store.SetActive(false);
                        transform.gameObject.SetActive(false);
              }

              public void OpenStore(){
                        transform.gameObject.SetActive(true);
                        store.SetActive(true);
                        GenerateProducts();
              }

              public void ShowBuyOptions(){
                        transform.gameObject.SetActive(true);
                        buyOptions.SetActive(true);
              }

              public void QuitAds(){

              }

              public void BuyCoins(){

              }

              public void GenerateProducts(){
                    //characters = charactersList.charactersData;
                        int rows;
                        if (charactersList.charactersData.Length%columns == 0) {
                                  rows = charactersList.charactersData.Length/columns;
                        }else{
                                  rows = Mathf.FloorToInt(charactersList.charactersData.Length/columns) + 1;
                        }
                        float scrollHeight = rows* itemHeight;
                        if (scrollHeight < minScrollHeight) {
                                  scrollHeight = minScrollHeight;
                        }

                        charactersGroup.GetComponent<RectTransform>().sizeDelta = new Vector2( minScrollWidth, scrollHeight);
                        initCoords = new Vector2(-xPadding, scrollHeight/2 - upMargin);
                        charactersGroup.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, - scrollHeight/2 - 500, 0);
                        DeleteProducts();

                        int index = 0;
                        int j = 0;
                        int i = 0;

                        while(index<charactersList.charactersData.Length){
                                  GameObject item = GameObject.Instantiate(storeItem, new Vector3(initCoords.x + i*xPadding, initCoords.y - j*yPadding, 0) , Quaternion.identity);

                                  int temp = index;
                                  item.transform.SetParent( charactersGroup.transform, false);
                                  item.transform.GetChild(2).GetComponentInChildren<Image>().sprite= charactersList.charactersData[index].img;
                                  item.transform.GetChild(4).GetComponentInChildren<Text>().text= charactersList.charactersData[index].price.ToString();
                                  item.transform.GetChild(4).GetComponent<Button>().onClick.AddListener( () => BuyButton() );
                                  item.transform.GetChild(3).GetComponent<Button>().onClick.AddListener( () => SelectButton(temp) );

                                  i++;
                                  index++;
                                  if (i==columns) {
                                            i=0;
                                            j++;
                                  }

                        }

                        SelectButton(PlayerMove.characterIndex);

              }

              public void SelectButton(int i){
                        charactersGroup.transform.GetChild (characterSelected).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        charactersGroup.transform.GetChild (i).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        characterSelected = i;
                        PlayerMove.characterIndex = characterSelected;
              }

              public void BuyButton(){

              }

              public void DeleteProducts(){

                        int childCount = charactersGroup.transform.childCount;
                        for (int i = 0; i< childCount; i++) {
                                  DestroyImmediate(charactersGroup.transform.GetChild (0).gameObject) ;
                        }
              }

              [Serializable]
              public struct Character{
                       public int price;
                       public Sprite img;
                       public GameObject model;
             }
}
