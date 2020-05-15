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
          public CharactersData charactersPersistance;
          private int characterSelected=0;
          public int columns;

          public GameObject storeItem;
          public GameObject charactersGroup;

          [Header("Display Settings")]
          public GameObject store;
          public GameObject buyOptions;
          public Text totalCoins;
          public Text totalCoinShop;
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



                        if (SaveSystem.LoadData<CharactersData>("characters.dta")!=null) {
                                charactersPersistance = SaveSystem.LoadData<CharactersData>("characters.dta");
                                if (charactersList.charactersData.Length!=charactersPersistance.GetCharactersLength()) {
                                    charactersPersistance.UpdateCharactersList(charactersList.charactersData.Length);
                                    SaveSystem.SaveData<CharactersData>(charactersPersistance ,"characters.dta");
                                }

                        }else{
                                Debug.Log("generando characters data");
                                InitializeCharacterData();                                
                        }
                        UpdateCoinsText();
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

              public void UpdateCoinsText(){
                  totalCoins.text = SaveSystem.LoadData<PlayerData>("player.dta").getCoins().ToString();
                  totalCoinShop.text = totalCoins.text;
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

                                  if (charactersPersistance.GetIndexInfo(index)==0) {
                                      SetSelectedItem(item);
                                      item.transform.GetChild(3).GetComponent<Button>().onClick.AddListener( () => SelectButton(temp) );
                                      item.transform.GetChild(4).GetComponent<Button>().onClick.AddListener( () => SelectButton(temp) );
                                  }else if(charactersPersistance.GetIndexInfo(index)==1){
                                      SetSelectItem(item);
                                      item.transform.GetChild(3).GetComponent<Button>().onClick.AddListener( () => SelectButton(temp) );
                                      item.transform.GetChild(4).GetComponent<Button>().onClick.AddListener( () => SelectButton(temp) );

                                  }else{
                                      SetBuyItem(item, index);
                                      item.transform.GetChild(3).gameObject.SetActive(false);
                                      item.transform.GetChild(4).GetComponent<Button>().onClick.AddListener( () => BuyButton(temp) );
                                  }



                                  i++;
                                  index++;
                                  if (i==columns) {
                                            i=0;
                                            j++;
                                  }

                        }

                        //SelectButton(PlayerMove.characterIndex);
                        SelectButton(charactersPersistance.GetSelectedIndex());

              }

              public void SetSelectItem(GameObject item){
                  item.transform.GetChild(0).gameObject.SetActive(false);
                  item.transform.GetChild(4).GetComponentInChildren<Text>().text = "select";
                  item.transform.GetChild(4).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                  item.transform.GetChild(4).GetComponentInChildren<Text>().alignment = TextAnchor.MiddleCenter;
              }
              public void SetSelectedItem(GameObject item){
                  item.transform.GetChild(0).gameObject.SetActive(true);
                  item.transform.GetChild(4).GetComponentInChildren<Text>().text = "selected";
                  item.transform.GetChild(4).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                  item.transform.GetChild(4).GetComponentInChildren<Text>().alignment = TextAnchor.MiddleCenter;
              }
              public void SetBuyItem(GameObject item, int index){
                  item.transform.GetChild(4).GetComponentInChildren<Text>().text= charactersList.charactersData[index].price.ToString();
                  item.transform.GetChild(4).GetComponentInChildren<Text>().alignment = TextAnchor.MiddleRight;
              }

              public void SelectButton(int i){
                        GameObject oldItem = charactersGroup.transform.GetChild (characterSelected).gameObject;
                        SetSelectItem(oldItem);
                        //charactersGroup.transform.GetChild (characterSelected).gameObject.transform.GetChild(4).gameObject.transform.GetChild(1).gameObject.SetActive(true);

                        GameObject newItem = charactersGroup.transform.GetChild (i).gameObject;
                        SetSelectedItem(newItem);

                        characterSelected = i;
                        PlayerMove.characterIndex = characterSelected;
                        charactersPersistance.SetSelectedIndex(characterSelected);
                        SaveSystem.SaveData<CharactersData>(charactersPersistance ,"characters.dta");
              }

              public void BuyButton(int i){
                  if (charactersPersistance.GetIndexInfo(i)==2) {
                      GameObject item = charactersGroup.transform.GetChild (i).gameObject;
                      int price = Int32.Parse(item.transform.GetChild(4).GetComponentInChildren<Text>().text);
                      if(SaveSystem.LoadData<PlayerData>("player.dta").getCoins() >= price ){
                          //buy and save
                          PlayerData pd = SaveSystem.LoadData<PlayerData>("player.dta");
                          pd.setCoins(pd.getCoins()-price);
                          SaveSystem.SaveData<PlayerData>(pd,"player.dta");
                          charactersPersistance.SetIndexInfo(i,1);
                          SaveSystem.SaveData<CharactersData>(charactersPersistance ,"characters.dta");

                          //UI response
                          item.transform.GetChild(4).GetComponent<Button>().onClick.RemoveListener( () => BuyButton(i) );
                          item.transform.GetChild(3).gameObject.SetActive(true);
                          item.transform.GetChild(3).GetComponent<Button>().onClick.AddListener( () => SelectButton(i) );
                          item.transform.GetChild(4).GetComponent<Button>().onClick.AddListener( () => SelectButton(i) );
                          SelectButton(i);
                          UpdateCoinsText();
                      }
                  }

              }

              public void DeleteProducts(){

                        int childCount = charactersGroup.transform.childCount;
                        for (int i = 0; i< childCount; i++) {
                                  DestroyImmediate(charactersGroup.transform.GetChild (0).gameObject) ;
                        }
              }

              public void InitializeCharacterData(){
                  int[] charactersInfo = new int[charactersList.charactersData.Length];
                  for (int i=0; i<charactersInfo.Length; i++) {
                      charactersInfo[i]=2;
                  }
                  charactersInfo[0]=0;
                  charactersPersistance = new CharactersData(charactersInfo);
                  characterSelected=0;
                  SaveSystem.SaveData<CharactersData>(charactersPersistance ,"characters.dta");
              }

              [Serializable]
              public struct Character{
                       public int price;
                       public Sprite img;
                       public GameObject model;
             }
}
