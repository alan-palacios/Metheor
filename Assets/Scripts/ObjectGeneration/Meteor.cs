using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
          Vector3 moveDirection, center;
          Rigidbody rb;
          GameObject objectInstanced;
          public SingleAstroObjectConfiguration singleAstroObjectConfiguration;
          public PlayerMove playerScript;
          public float movePower;
          public float minRange;
          public float maxRange;
          public float timeOfLife;

          public float rotVelocityX, rotVelocityY;

          // Start is called before the first frame update
          void Start()
          {

                    StartCoroutine( GenerarMeteoro() );
          }

          void FixedUpdate()
          {
                    if (rb!=null) {
                              rb.MovePosition(objectInstanced.transform.position + (moveDirection) * Time.fixedDeltaTime);
                    }
          }

          public IEnumerator GenerarMeteoro(){
                    yield return new WaitForSeconds(timeOfLife);
                    while(true){
                              InstanciarModelo();
                              moveDirection = ((center) -
                                                            objectInstanced.transform.position).normalized*movePower;
                              rb = objectInstanced.GetComponent<Rigidbody>();
                              yield return new WaitForSeconds(timeOfLife);
                              Destroy(objectInstanced);
                              objectInstanced=null;
                    }
          }

          void InstanciarModelo(){
                    int objModelsCount = singleAstroObjectConfiguration.models.Length;
                    GameObject objPlaced = singleAstroObjectConfiguration.models[Random.Range(0,objModelsCount)];
                    Vector3 angles = new Vector3( Random.Range(-3, 3)*10, Random.Range(0, 36)*10, singleAstroObjectConfiguration.zRotation);

                    center = playerScript.transform.position+playerScript.transform.forward*10f;
                    Vector3 position = RandomCircle(center, Random.Range(minRange, maxRange));

                    objectInstanced = GameObject.Instantiate(objPlaced, position , Quaternion.Euler(angles) ) as GameObject;
                    objectInstanced.transform.SetParent( transform, false);
                    float newScale = Random.Range( singleAstroObjectConfiguration.minScale, singleAstroObjectConfiguration.maxScale );
                    objectInstanced.transform.localScale = Vector3.one*Mathf.Floor (newScale / 0.1f)*0.1f;
                    int materialsCount = singleAstroObjectConfiguration.materials.Length;
                    if (materialsCount>0) {
                              objectInstanced.GetComponent<Renderer>().material = singleAstroObjectConfiguration.materials[Random.Range(0,materialsCount)];
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
