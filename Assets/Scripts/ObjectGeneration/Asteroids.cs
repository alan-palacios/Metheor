using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Asteroids : MonoBehaviour
{
    GameObject [] asteroids;
    public AsteroidsConfiguration asteroidsConfiguration;
    void Start(){

              //Instantiating asteroids
              int asteroidsModelsCount = asteroidsConfiguration.asteroidsModels.Length;
              int numOfAsteroids = Random.Range(asteroidsConfiguration.minNumOfAsteroids,
                                                                          asteroidsConfiguration.maxNumOfAsteroids);
              asteroids = new GameObject[numOfAsteroids];
              for (int i = 0; i<numOfAsteroids; i++) {
                        GameObject asteroidPlaced = asteroidsConfiguration.asteroidsModels[Random.Range(0,asteroidsModelsCount)];
                        float dstFromOrigin = Random.Range( asteroidsConfiguration.minDst, asteroidsConfiguration.maxDst);
                        float rndAngle = Random.Range (0,2*Mathf.PI);

                        Vector3 position = new Vector3( dstFromOrigin * Mathf.Cos(rndAngle), 0, dstFromOrigin * Mathf.Sin(rndAngle) );
                        Vector3 angles = new Vector3( Random.Range(0, 36)*10, Random.Range(0, 36)*10, Random.Range(0, 36)*10);

                        asteroids[i] = GameObject.Instantiate( asteroidPlaced, position, Quaternion.Euler(angles) ) as GameObject;
                        asteroids[i].transform.SetParent( transform, false);
                        float newScale = Random.Range( asteroidsConfiguration.AsteroidsMinScale, asteroidsConfiguration.AsteroidsMaxScale );
                        asteroids[i].transform.localScale = Vector3.one*Mathf.Floor (newScale / 0.1f)*0.1f;                        

              }

    }

    public IEnumerator DestruirAsteroide(GameObject asteroid){

       Vector3 disminucionEscala = new Vector3( 0.1f, 0.1f, 0.1f);
       float timeBetwenChange = 0.1f;

       while(asteroid.transform.localScale.x>0){
              asteroid.transform.localScale -= disminucionEscala;
              yield return new WaitForSeconds(timeBetwenChange);

       }
       Destroy(asteroid);
    }

}
