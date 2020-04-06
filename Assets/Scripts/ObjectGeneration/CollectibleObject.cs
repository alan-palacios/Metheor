using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CollectibleObject : MonoBehaviour
{
          GameObject objectInstanced, particles;
          public float rotVelocityX, rotVelocityY;

          void Start(){
                    objectInstanced= transform.GetChild(0).gameObject;
                    particles= transform.GetChild(1).gameObject;
          }

          void Update(){
                    if (objectInstanced!=null) {
                              objectInstanced.transform.Rotate (rotVelocityX,rotVelocityY,0, Space.World);
                    }
          }

          public IEnumerator DestruirObjeto(GameObject objeto){

             Vector3 disminucionEscala = Vector3.one*0.2f;
             float timeBetwenChange = 0.05f;

             while(objeto!=null && objeto.transform.localScale.x>0){
                    objeto.transform.localScale -= disminucionEscala;
                    yield return new WaitForSeconds(timeBetwenChange);

             }
             Destroy(objeto);
          }

          public IEnumerator DestruirObjetos(){
                    StartCoroutine(DestruirObjeto(objectInstanced));
                    StartCoroutine(DestruirObjeto(particles));
                    yield return new WaitForSeconds(0.5f);
                    Destroy(transform.gameObject);
          }
}
