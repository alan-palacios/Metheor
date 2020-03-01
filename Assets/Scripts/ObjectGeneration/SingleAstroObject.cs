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
                    GameObject objPlaced = singleAstroObjectConfiguration.models[Random.Range(0,objModelsCount)];
                    Vector3 angles = new Vector3( 0, Random.Range(0, 36)*10, 0);
                    objectInstanced = GameObject.Instantiate(objPlaced, new Vector3(  0, 0  ,0)  , Quaternion.Euler(angles) ) as GameObject;
                    objectInstanced.transform.SetParent( transform, false);
                    float newScale = Random.Range( singleAstroObjectConfiguration.minScale, singleAstroObjectConfiguration.maxScale );
                    objectInstanced.transform.localScale = Vector3.one*Mathf.Floor (newScale / 0.1f)*0.1f;
          }
}
