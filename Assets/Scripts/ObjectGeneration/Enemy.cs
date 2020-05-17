using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
          Vector3 moveDirection, center;
          Rigidbody rb;
          GameObject objectInstanced;
          public EnemyConfiguration [] objConfList;
          private EnemyConfiguration objConf;
          public PlayerMove playerScript;
          public float minRange;
          public float maxRange;
          public float timeToInit;
          //public float rotVelocityX, rotVelocityY;


          void Start()
          {

                    StartCoroutine( GenerateEnemy() );
          }

          void FixedUpdate()
          {

                    if (objConf!=null && rb!=null) {
                        if (objConf.multipleObjects) {
                            foreach (Transform child in objectInstanced.transform){
                                Quaternion deltaRotation = Quaternion.Euler(new Vector3(0,objConf.rotationVelocity,0) * Time.deltaTime);
                                Rigidbody rbObj = child.gameObject.GetComponent<Rigidbody>();
                                rbObj.MovePosition(child.position + (moveDirection) * Time.fixedDeltaTime);
                                rbObj.MoveRotation(rbObj.rotation * deltaRotation);
                            }

                        }else{
                                Quaternion deltaRotation = Quaternion.Euler(new Vector3(0,objConf.rotationVelocity,0) * Time.deltaTime);
                                rb.MovePosition(objectInstanced.transform.position + (moveDirection) * Time.fixedDeltaTime);
                                rb.MoveRotation(rb.rotation * deltaRotation);
                                //objectInstanced.transform.Rotate (0,rotVelocityY,0, Space.World);
                        }
                    }

          }

          public IEnumerator GenerateEnemy(){
                    yield return new WaitForSeconds(timeToInit);
                    while(true){
                                //objConf = objConfList[ Random.Range(0,objConfList.Length) ];
                                objConf = objConfList[ 3 ];
                                if (objConf.multipleObjects) {
                                    InstanceMultipleObjects();
                                }else{
                                    InstanceSingleObject();
                                }
                              moveDirection = ((center) - objectInstanced.transform.position).normalized*objConf.movePower;
                              rb = objectInstanced.GetComponent<Rigidbody>();
                              yield return new WaitForSeconds(objConf.timeOfLife);
                              if (objConf.multipleObjects) {
                                  StartCoroutine(DestroyMultipleObjects(objectInstanced, objConf.timeBetwenChange) );
                              }else{
                                  StartCoroutine(DestroySingleObject(objectInstanced, objConf.timeBetwenChange) );
                              }

                    }
          }

          void InstanceSingleObject(){
                    int objModelsCount = objConf.models.Length;
                    GameObject objPlaced = objConf.models[Random.Range(0,objModelsCount)];
                    Vector3 angles = new Vector3( Random.Range(-3, 3)*10, Random.Range(0, 36)*10, objConf.zRotation);

                    center = playerScript.transform.position+playerScript.transform.forward*10f;
                    Vector3 position = RandomCircle(center, Random.Range(minRange, maxRange));

                    objectInstanced = GameObject.Instantiate(objPlaced, position , Quaternion.identity ) as GameObject;
                    objectInstanced.transform.SetParent( transform, false);
                    if (objConf.randomScale) {
                        float newScale = Random.Range( objConf.minScale, objConf.maxScale );
                        objectInstanced.transform.localScale = Vector3.one*Mathf.Floor (newScale / 0.1f)*0.1f;
                    }
                    //int materialsCount = objConf.materials.Length;
                    /*if (materialsCount>0) {
                              objectInstanced.GetComponent<Renderer>().material = objConf.materials[Random.Range(0,materialsCount)];
                    }*/
          }

          void InstanceMultipleObjects(){
                    //Instancing Parent
                    center = playerScript.transform.position+playerScript.transform.forward*10f;
                    Vector3 position = RandomCircle(center, Random.Range(minRange, maxRange));
                    objectInstanced = GameObject.Instantiate(objConf.multipleObjectsParent, position , Quaternion.identity ) as GameObject;
                    objectInstanced.transform.SetParent( transform, false);

                    //Instantiating objects
                    int objectsModelsCount = objConf.objectsModels.Length;
                    int numOfObjects = Random.Range(objConf.minNumOfObjects,objConf.maxNumOfObjects);


                    //Material material = objConf.materials[ Random.Range(0, objConf.materials.Length)];

                    for (int i = 0; i<numOfObjects; i++) {
                              GameObject objModel = objConf.objectsModels[Random.Range(0,objectsModelsCount)];

                              float dstFromOrigin = Random.Range( objConf.minDst, objConf.maxDst);
                              float rndAngle = Random.Range (0,2*Mathf.PI);
                              Vector3 positionObj = new Vector3( dstFromOrigin * Mathf.Cos(rndAngle), 0, dstFromOrigin * Mathf.Sin(rndAngle) );
                              Vector3 angles = new Vector3( Random.Range(0, 36)*10, Random.Range(0, 36)*10, Random.Range(0, 36)*10);

                              GameObject multipleObj = GameObject.Instantiate( objModel, positionObj, Quaternion.Euler(angles) ) as GameObject;
                              multipleObj.transform.SetParent( objectInstanced.transform, false);
                              if (objConf.randomScale) {
                                  float newScale = Random.Range( objConf.minScale, objConf.maxScale );
                                  multipleObj.transform.localScale = Vector3.one*Mathf.Floor (newScale / 0.1f)*0.1f;
                              }
                              //multipleObj.GetComponent<Renderer>().material = material;

                    }

          }

          public IEnumerator DestroyMultipleObjects(GameObject objeto, float timeBetwenChange){
                  yield return null;
                  Vector3 disminucionEscala = Vector3.one *0.15f;
                  foreach (Transform child in objeto.transform){
                      child.gameObject.tag = "Untagged";
                  }
                  //error transform child out of bounds
                  while(objeto.transform.GetChild(0).gameObject.transform.localScale.x>0){
                       for(int i = 0; i < objeto.transform.childCount; i++){
                                  GameObject g = objeto.transform.GetChild(i).gameObject;
                                  g.transform.localScale -= disminucionEscala;
                                  if (g.transform.localScale.x<=0) {
                                          g.transform.localScale=Vector3.zero;
                                          Destroy(objeto);
                                          yield break;
                                }
                        }
                        yield return new WaitForSeconds(timeBetwenChange);
                 }
          }



          public IEnumerator DestroySingleObject(GameObject objeto, float timeBetwenChange){
                    yield return null;
                       Vector3 disminucionEscala = Vector3.one*0.15f;
                       objeto.tag = "Untagged";
                       while(objeto!=null && objeto.transform.localScale.x>0){
                                 if (objeto!=null) {
                                        objeto.transform.localScale -= disminucionEscala;
                                        if (objeto.transform.localScale.x<0) {
                                                  objeto.transform.localScale = Vector3.zero;
                                                  Destroy(objeto);
                                                  yield return 0;
                                        }
                                 }
                              yield return new WaitForSeconds(timeBetwenChange);
                       }
          }

          Vector3  RandomCircle(Vector3 center, float radius){
                    // create random angle between 0 to 360 degrees
                    float ang = playerScript.transform.eulerAngles.y+ Random.Range(-7,7)*10f ;
                    Vector3 pos;
                    pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
                    pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
                    pos.y = center.y;
                    return pos;
          }
}
