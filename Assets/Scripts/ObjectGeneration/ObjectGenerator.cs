using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectGenerator
{
          public static void GenerateObjectsInPreview(ref GameObject[][] objetos, ObjectPlacingList objectPlacingList,
                     float chunkSize,  Transform parent, int score){
                    int longLista = objectPlacingList.objectsSettings.Length;
                    if (longLista>0) {
                              objetos = new GameObject[longLista][];

                              for (int i=0; i<longLista; i++) {

                                        if(objectPlacingList.objectsSettings[i].generationMode == ObjectData.GenerationMode.PDS &&
                                                  score >= objectPlacingList.objectsSettings[i].minScoreToAppear &&
                                                  score <= objectPlacingList.objectsSettings[i].maxScoreToAppear ){
                                                  List <Vector2> originalPoints = PoissonDiscSampling.GeneratePoints(objectPlacingList.objectsSettings[i].radius,
                                                            new Vector2( chunkSize, chunkSize), objectPlacingList.objectsSettings[i].rejectionSamples );

                                                  List<Vector2> points = new List<Vector2>();

                                                  for (int x=0; x<originalPoints.Count; x++) {
                                                            points.Add(originalPoints[x]);
                                                            /*float heightValue = heightValues[ (int)originalPoints[x].x, (int)originalPoints[x].y ];
                                                            if ( heightValue >= objectPlacingList.objectsSettings[i].startHeight && heightValue <= objectPlacingList.objectsSettings[i].endHeight ) {
                                                            }*/
                                                  }

                                                  objetos[i]= new GameObject[points.Count];

                                                  for (int j=0; j<objetos[i].Length; j++) {
                                                            /*if(objectPlacingList.objectsSettings[i].typeOfStelarObject == ObjectData.TypeOfStelarObject.Planet){
                                                                      int modelsCount = objectPlacingList.objectsSettings[i].modelos.Length;

                                                                      GameObject objectPlaced = objectPlacingList.objectsSettings[i].modelos[Random.Range(0,modelsCount)];
                                                                      MeshFilter viewedModelFilter = objectPlaced.GetComponent<MeshFilter>();

                                                                      float xCoord = -chunkSize/2+points[j].x;
                                                                      float yCoord = -chunkSize/2+points[j].y;
                                                                      float heightCoord = 0;
                                                                      //heightValues[ (int)points[j].x, (int)points[j].y ] -objectPlacingList.objectsSettings[i].offsetHeight;
                                                                      //float heightCoord = heightValues[ (int)points[j].x, (int)points[j].y ] +
                                                                      //                                        viewedModelFilter.sharedMesh.bounds.size.y*objectPlaced.transform.localScale.y/2 -
                                                                      //                                        objectPlacingList.objectsSettings[i].offsetHeight;


                                                                      Vector3 angles = new Vector3( 0, Random.Range(0, 36)*10, 0);
                                                                      objetos[i][j] = GameObject.Instantiate(objectPlaced, new Vector3(  xCoord, heightCoord  ,yCoord)  , Quaternion.Euler(angles), parent ) as GameObject;

                                                                      if (objectPlacingList.objectsSettings[i].randomMaterial) {
                                                                                int materialCount = objectPlacingList.objectsSettings[i].materiales.Length;
                                                                                Renderer objRenderer = objetos[i][j].GetComponent<Renderer>();
                                                                                objRenderer.sharedMaterial= objectPlacingList.objectsSettings[i].materiales[Random.Range(0,materialCount)];
                                                                      }
                                                                      if (objectPlacingList.objectsSettings[i].randomScale) {
                                                                                float newScale = Random.Range( objectPlacingList.objectsSettings[i].minScale, objectPlacingList.objectsSettings[i].maxScale );

                                                                                objetos[i][j].transform.localScale = Vector3.one*Mathf.Floor (newScale / 0.1f)*0.1f;
                                                                      }
                                                            }*/

                                                            if(objectPlacingList.objectsSettings[i].typeOfStelarObject == ObjectData.TypeOfStelarObject.Asteroids){

                                                                      GameObject objectPlaced = objectPlacingList.objectsSettings[i].objectParent;

                                                                      float xCoord = -chunkSize/2+points[j].x;
                                                                      float yCoord = -chunkSize/2+points[j].y;
                                                                      float heightCoord = 0;
                                                                      objetos[i][j] = GameObject.Instantiate(objectPlaced, new Vector3(  xCoord, heightCoord  ,yCoord)  , Quaternion.identity, parent ) as GameObject;
                                                            }
                                                            if(objectPlacingList.objectsSettings[i].typeOfStelarObject == ObjectData.TypeOfStelarObject.SingleAstroObject){
                                                                      GameObject objectPlaced = objectPlacingList.objectsSettings[i].objectParent;

                                                                      float xCoord = -chunkSize/2+points[j].x;
                                                                      float yCoord = -chunkSize/2+points[j].y;
                                                                      float heightCoord = 0;
                                                                      objetos[i][j] = GameObject.Instantiate(objectPlaced, new Vector3(  xCoord, heightCoord  ,yCoord)  , Quaternion.identity, parent ) as GameObject;
                                                            }
                                                            if(objectPlacingList.objectsSettings[i].typeOfStelarObject == ObjectData.TypeOfStelarObject.SolarSystem){

                                                                      GameObject objectPlaced = objectPlacingList.objectsSettings[i].objectParent;

                                                                      float xCoord = -chunkSize/2+points[j].x;
                                                                      float yCoord = -chunkSize/2+points[j].y;
                                                                      float heightCoord = 0;
                                                                      //heightValues[ (int)points[j].x, (int)points[j].y ] -objectPlacingList.objectsSettings[i].offsetHeight;
                                                                      //float heightCoord = heightValues[ (int)points[j].x, (int)points[j].y ] +
                                                                      //                                        viewedModelFilter.sharedMesh.bounds.size.y*objectPlaced.transform.localScale.y/2 -
                                                                      //                                        objectPlacingList.objectsSettings[i].offsetHeight;
                                                                      objetos[i][j] = GameObject.Instantiate(objectPlaced, new Vector3(  xCoord, heightCoord  ,yCoord)  , Quaternion.identity, parent ) as GameObject;
                                                            }
                                                  }
                                        }

                              }
                    }


          }

          public static void DeleteObjectsInPreview( ref GameObject displayedObjectsParent){
                    if ( displayedObjectsParent != null) {
                              Object.DestroyImmediate(displayedObjectsParent);
                              displayedObjectsParent = null;
                    }

          }

          public static void GenerateObjectsInGame(   ObjectPlacingList objectPlacingList,
                     float chunkSize,  GameObject parentObj, Vector2 coord, float localScale, int score){
                               GameObject [][] objetos;
                              int longLista = objectPlacingList.objectsSettings.Length;
                              if (longLista>0) {
                                        objetos = new GameObject[longLista][];

                                        for (int i=0; i<longLista; i++) {

                                                  if(objectPlacingList.objectsSettings[i].generationMode == ObjectData.GenerationMode.PDS &&
                                                            score >= objectPlacingList.objectsSettings[i].minScoreToAppear &&
                                                            score <= objectPlacingList.objectsSettings[i].maxScoreToAppear ){


                                                            List <Vector2> originalPoints = PoissonDiscSampling.GeneratePoints(objectPlacingList.objectsSettings[i].radius,
                                                            new Vector2( chunkSize, chunkSize), objectPlacingList.objectsSettings[i].rejectionSamples );

                                                            List<Vector2> points = new List<Vector2>();

                                                            for (int x=0; x<originalPoints.Count; x++) {
                                                                      points.Add(originalPoints[x]);
                                                                      /*float heightValue = heightValues[ (int)originalPoints[x].x, (int)originalPoints[x].y ];
                                                                      if ( heightValue >= objectPlacingList.objectsSettings[i].startHeight && heightValue <= objectPlacingList.objectsSettings[i].endHeight ) {
                                                                      }*/
                                                            }

                                                            objetos[i]= new GameObject[points.Count];

                                                            for (int j=0; j<objetos[i].Length; j++) {
                                                                      /*if(objectPlacingList.objectsSettings[i].typeOfStelarObject == ObjectData.TypeOfStelarObject.Planet){
                                                                                int modelsCount = objectPlacingList.objectsSettings[i].modelos.Length;

                                                                                GameObject objectPlaced = objectPlacingList.objectsSettings[i].modelos[Random.Range(0,modelsCount)];
                                                                                MeshFilter viewedModelFilter = objectPlaced.GetComponent<MeshFilter>();

                                                                                float xCoord =  coord.x*chunkSize-chunkSize/2+points[j].x;
                                                                                float yCoord = coord.y*chunkSize-chunkSize/2+points[j].y;
                                                                                float heightCoord = 0;
                                                                                //heightValues[ (int)points[j].x, (int)points[j].y ] -objectPlacingList.objectsSettings[i].offsetHeight;
                                                                                Vector3 angles = new Vector3(0,0,0);
                                                                                angles.y = Random.Range(0, 6)*60;

                                                                                objetos[i][j] = GameObject.Instantiate(objectPlaced, new Vector3(  xCoord, heightCoord  ,yCoord)  , Quaternion.Euler(angles), parentObj.transform) as GameObject;


                                                                                if (objectPlacingList.objectsSettings[i].randomMaterial) {
                                                                                          int materialCount = objectPlacingList.objectsSettings[i].materiales.Length;
                                                                                          Renderer objRenderer = objetos[i][j].GetComponent<Renderer>();
                                                                                          objRenderer.sharedMaterial= objectPlacingList.objectsSettings[i].materiales[Random.Range(0,materialCount)];
                                                                                }
                                                                                if (objectPlacingList.objectsSettings[i].randomScale) {
                                                                                          float newScale = Random.Range( localScale-objectPlacingList.objectsSettings[i].minScale, localScale+objectPlacingList.objectsSettings[i].maxScale );

                                                                                          objetos[i][j].transform.localScale = Vector3.one*Mathf.Floor (newScale / 0.1f)*0.1f;
                                                                                }
                                                                                if (objectPlacingList.objectsSettings[i].generateCollider) {
                                                                                          MeshCollider meshCollider = objetos[i][j].AddComponent<MeshCollider>();
                                                                                          meshCollider.enabled = false;
                                                                                }
                                                                      }*/
                                                                      if(objectPlacingList.objectsSettings[i].typeOfStelarObject == ObjectData.TypeOfStelarObject.Asteroids){
                                                                                GameObject objectPlaced = objectPlacingList.objectsSettings[i].objectParent;

                                                                                float xCoord =  coord.x*chunkSize-chunkSize/2+points[j].x;
                                                                                float yCoord = coord.y*chunkSize-chunkSize/2+points[j].y;
                                                                                float heightCoord = 0;
                                                                                objetos[i][j] = GameObject.Instantiate(objectPlaced, new Vector3(  xCoord, heightCoord  ,yCoord)  , Quaternion.identity, parentObj.transform ) as GameObject;
                                                                      }
                                                                      if(objectPlacingList.objectsSettings[i].typeOfStelarObject == ObjectData.TypeOfStelarObject.SingleAstroObject){
                                                                                GameObject objectPlaced = objectPlacingList.objectsSettings[i].objectParent;

                                                                                float xCoord =  coord.x*chunkSize-chunkSize/2+points[j].x;
                                                                                float yCoord = coord.y*chunkSize-chunkSize/2+points[j].y;
                                                                                float heightCoord = 0;
                                                                                objetos[i][j] = GameObject.Instantiate(objectPlaced, new Vector3(  xCoord, heightCoord  ,yCoord)  , Quaternion.identity, parentObj.transform ) as GameObject;
                                                                      }
                                                                      if(objectPlacingList.objectsSettings[i].typeOfStelarObject == ObjectData.TypeOfStelarObject.SolarSystem){

                                                                                GameObject objectPlaced = objectPlacingList.objectsSettings[i].objectParent;

                                                                                float xCoord =  coord.x*chunkSize-chunkSize/2+points[j].x;
                                                                                float yCoord = coord.y*chunkSize-chunkSize/2+points[j].y;
                                                                                float heightCoord = 0;
                                                                                //heightValues[ (int)points[j].x, (int)points[j].y ] -objectPlacingList.objectsSettings[i].offsetHeight;
                                                                                //float heightCoord = heightValues[ (int)points[j].x, (int)points[j].y ] +
                                                                                //                                        viewedModelFilter.sharedMesh.bounds.size.y*objectPlaced.transform.localScale.y/2 -
                                                                                //                                        objectPlacingList.objectsSettings[i].offsetHeight;
                                                                                Vector3 angles = new Vector3(0,0,0);
                                                                                angles.y = Random.Range(0, 6)*60;
                                                                                objetos[i][j] = GameObject.Instantiate(objectPlaced, new Vector3(  xCoord, heightCoord  ,yCoord)  , Quaternion.identity, parentObj.transform ) as GameObject;
                                                                      }
                                                            }
                                                  }


                                        }
                              }


          }

}
