using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SingleAstroObject : MonoBehaviour
{
           GameObject objectInstanced;
          public SingleAstroObjectConfiguration singleAstroObjectConfiguration;

          void Start(){
                    int objModelsCount = singleAstroObjectConfiguration.models.Length;
                    if (objModelsCount>0) {
                              GameObject objPlaced = singleAstroObjectConfiguration.models[Random.Range(0,objModelsCount)];
                              Vector3 angles = new Vector3( Random.Range(-3, 3)*10, Random.Range(0, 36)*10, singleAstroObjectConfiguration.zRotation);
                              objectInstanced = GameObject.Instantiate(objPlaced, new Vector3(  0, 0  ,0)  , Quaternion.Euler(angles) ) as GameObject;
                              objectInstanced.transform.SetParent( transform, false);
                              float newScale = Random.Range( singleAstroObjectConfiguration.minScale, singleAstroObjectConfiguration.maxScale );
                              objectInstanced.transform.localScale = Vector3.one*Mathf.Floor (newScale / 0.1f)*0.1f;
                              int materialsCount = singleAstroObjectConfiguration.materials.Length;
                              if (materialsCount>0) {
                                        objectInstanced.GetComponent<Renderer>().material = singleAstroObjectConfiguration.materials[Random.Range(0,materialsCount)];
                              }
                              if (singleAstroObjectConfiguration.hasImpulse && objectInstanced.GetComponent<Rigidbody>()!=null) {
                                        objectInstanced.GetComponent<Rigidbody>().AddForce(
                                                            new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f))*singleAstroObjectConfiguration.impulsePower,
                                                             ForceMode.Impulse);
                              }
                    }else{
                              Vector3 angles = new Vector3( 0, Random.Range(0, 36)*10, singleAstroObjectConfiguration.zRotation);
                              float newScale = Random.Range( singleAstroObjectConfiguration.minScale ,
                                                                                          singleAstroObjectConfiguration.maxScale);
                              transform.localScale = Vector3.one*Mathf.Floor (newScale / 0.1f)*0.1f;
                              if (GetComponent<Rigidbody>()!=null) {
                                        GetComponent<Rigidbody>().AddForce(
                                                            new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f))*singleAstroObjectConfiguration.impulsePower,
                                                             ForceMode.Impulse);
                              }
                    }
          }

          public IEnumerator DestruirObjeto(GameObject objeto){

             Vector3 disminucionEscala = Vector3.one*0.3f;
             Vector3 aumentoEscala = Vector3.one*0.1f;
             float timeBetwenChange = 0.04f;

             //aumento de escala
             for (int i=0; i<3; i++) {
                       objeto.transform.localScale += aumentoEscala;
                       yield return new WaitForSeconds(0.05f);
             }

             while(objeto!=null && objeto.transform.localScale.x>0){
                    objeto.transform.localScale -= disminucionEscala;
                    yield return new WaitForSeconds(timeBetwenChange);

             }
             Destroy(objeto);
          }
}
