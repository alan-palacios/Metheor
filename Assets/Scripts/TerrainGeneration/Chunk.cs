using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
          //GameObject [][] objetos;
          public GameObject background;
          GameObject displayedObjectsParent;
          public GameObject chunkGameObject;
          ObjectPlacingList objectPlacingList;
          public Coroutine destroyCoroutineReference;

          public event System.Action<Chunk, bool, Coroutine> onVisibilityChanged;

          const float colliderGenerationDistanceTreshold= 5f;

          Vector2 sampleCentre;
          Bounds bounds;
          public Vector2 coord;
          public int chunkSize;
          float maxViewDst;

          bool objectsGenerated=false;
          Transform viewer;


          public Chunk(Vector2 coord, Transform parent, Transform viewer, ObjectPlacingList objectPlacingList, float maxViewDst, int chunkSize) {

                    this.chunkSize=chunkSize;

                    this.coord = coord;
                    this.objectPlacingList = objectPlacingList;

                    this.viewer=viewer;
                    sampleCentre = coord * chunkSize;//mapGenerator.biomeData.meshSettings.meshScale;
                    Vector2 position = coord*chunkSize;//new Vector3(sampleCentre.x,0,sampleCentre.y);
                    bounds = new Bounds(new Vector3(position.x, 0, position.y),Vector3.one * chunkSize);

                    chunkGameObject = new GameObject("Chunk");
                    chunkGameObject.transform.position = new Vector3(position.x, 0, position.y);
                    chunkGameObject.transform.SetParent(parent, false);
                    SetVisible(false);


                    /*background = GameObject.Instantiate(background) as GameObject;
                    background.transform.SetParent( chunkGameObject.transform, false);
                    background.transform.localScale*= (float)chunkSize/10;
                    background.transform.position = new Vector3(position.x, -0.1f, position.y);*/


                    this.maxViewDst = maxViewDst;
                    if ( !(coord.x==0 && coord.y==0)) {
                              displayedObjectsParent = new GameObject("displayedObjectsParent");
                              displayedObjectsParent.transform.SetParent( chunkGameObject.transform, false);
                    }
		//ObjectGenerator.GenerateObjectsInGame(objetos, objectPlacingList, chunkSize,  displayedObjectsParent, coord );

		//displayedObjectsParent.gameObject.SetActive(true);


          }

          Vector3 viewerPosition{
                    get{
                              return new Vector3(viewer.position.x, viewer.position.y, viewer.position.z);
                    }
          }

          void SetVisible(bool visible){
                    chunkGameObject.SetActive(visible);
          }

          public bool IsVisible() {
                    return chunkGameObject.activeSelf;
          }

          public void UpdateChunk() {
                              float viewerDstFromNearestEdge = bounds.SqrDistance (viewerPosition);
                              bool wasVisible= IsVisible();

                              //Debug.Log(viewerDstFromNearestEdge+"   "+maxViewDst);
                              bool visible = viewerDstFromNearestEdge <= maxViewDst*maxViewDst;

                              if (visible) {

                                        if (displayedObjectsParent!=null) {
                                                  displayedObjectsParent.SetActive(true);
                                                  //el valor de los objetos y waterObj no se altera, tiene que pasarse con ref
                                                  if (!objectsGenerated) {
                                                            ObjectGenerator.GenerateObjectsInGame(objectPlacingList, chunkSize,  displayedObjectsParent, coord,
                                                                      viewer.localScale.x, viewer.gameObject.GetComponent<PlayerMove>().score );
                                                            objectsGenerated=true;
                                                  }
                                        }

                              }

                              if (wasVisible!=visible) {
                                        SetVisible (visible);
                                        if (onVisibilityChanged!=null) {
                                                  onVisibilityChanged (this, visible,  destroyCoroutineReference);
                                        }

                              }


          }

}
