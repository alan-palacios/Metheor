using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class SolarSystem : MonoBehaviour
{
           GameObject sun;
           Planet [] planets;
          public SolarSystemConfiguration solarSystemConfiguration;
          public float sunVelocityRotation;
          void Start(){

                    //Instantiating sun
                    int sunModelsCount = solarSystemConfiguration.solarSystemData.suns.Length;
                    int planetsModelsCount = solarSystemConfiguration.solarSystemData.planets.Length;
                    GameObject sunPlaced = solarSystemConfiguration.solarSystemData.suns[Random.Range(0,sunModelsCount)];
                    Vector3 angles = new Vector3( 0, Random.Range(0, 36)*10, 0);
                    sun = GameObject.Instantiate(sunPlaced, new Vector3(  0, 0  ,0)  , Quaternion.Euler(angles) ) as GameObject;
                    sun.transform.SetParent( transform, false);
                    float newScale = Random.Range( solarSystemConfiguration.solarSystemData.SunMinScale, solarSystemConfiguration.solarSystemData.SunMaxScale );
                    sun.transform.localScale = Vector3.one*Mathf.Floor (newScale / 0.1f)*0.1f;

                    //Instantiating planets
                    int numOfPlanets = Random.Range(solarSystemConfiguration.solarSystemData.minNumOfPlanets,
                                                                                solarSystemConfiguration.solarSystemData.maxNumOfPlanets);
                    planets = new Planet[numOfPlanets];
                    for (int i = 0; i<numOfPlanets; i++) {
                              GameObject planetPlaced = solarSystemConfiguration.solarSystemData.planets[Random.Range(0,planetsModelsCount)];
                              float dstFromSun = Random.Range( solarSystemConfiguration.solarSystemData.minDst, solarSystemConfiguration.solarSystemData.maxDst);
                              float rndAngle = Random.Range (0,2*Mathf.PI);

                              Vector3 position = new Vector3( dstFromSun * Mathf.Cos(rndAngle), 0, dstFromSun * Mathf.Sin(rndAngle) );
                              angles = new Vector3( Random.Range(0, 36)*10, Random.Range(0, 36)*10, Random.Range(0, 36)*10);

                              planets[i].planet = GameObject.Instantiate( planetPlaced, position, Quaternion.Euler(angles) ) as GameObject;
                              planets[i].planet.transform.SetParent( transform, false);
                              newScale = Random.Range( solarSystemConfiguration.solarSystemData.PlanetsMinScale, solarSystemConfiguration.solarSystemData.PlanetsMaxScale );
                              planets[i].planet.transform.localScale = Vector3.one*Mathf.Floor (newScale / 0.1f)*0.1f;

                              planets[i].dstFromSun = dstFromSun;

                              float trasVelocity = Random.Range(solarSystemConfiguration.solarSystemData.PlanetsMinTVelocity, solarSystemConfiguration.solarSystemData.PlanetsMaxTVelocity);
                              if (Random.value > 0.5f) {
                                        trasVelocity*=-1;
                              }
                              planets[i].trasVelocity = trasVelocity;

                              float rotVelocity = Random.Range(solarSystemConfiguration.solarSystemData.PlanetsMinRVelocity, solarSystemConfiguration.solarSystemData.PlanetsMaxRVelocity);
                              if (Random.value > 0.5f) {
                                        rotVelocity*=-1;
                              }
                              planets[i].rotVelocity = rotVelocity;

                              planets[i].angle = rndAngle;

                              GameObject orbit = GameObject.Instantiate( solarSystemConfiguration.solarSystemData.orbit) as GameObject;
                              orbit.transform.SetParent( transform, false);
                              orbit.DrawCircle(dstFromSun, 0.2f, solarSystemConfiguration.solarSystemData.orbitMaterials[i%3]);

                    }
                    //GameObject light = GameObject.Instantiate( solarSystemConfiguration.solarSystemData.light) as GameObject;
                    //light.transform.SetParent( transform, false);
          }

          void Update(){
              if(Time.timeScale == 0)return;
                    //Vector3 sunAngle = sun.transform.rota;
                    sun.transform.Rotate(0, sunVelocityRotation, 0, Space.Self);
                    for (int i=0; i<planets.Length; i++) {

                              //if (planets[i].planet != null) {
                                        planets[i].planet.transform.RotateAround (sun.transform.position, Vector3.up, planets[i].trasVelocity * Time.deltaTime);
                                        planets[i].planet.transform.Rotate (0,planets[i].rotVelocity,0, Space.World);
                              //}

                    }
          }

          public IEnumerator DestruirPlaneta(GameObject GroupOfPlanets){
              GameObject planetFract = GroupOfPlanets.transform.GetChild(1).gameObject;
              Vector3 disminucionEscala = new Vector3( 0.1f, 0.1f, 0.1f);
              float timeBetwenChange = 0.15f;

              while(planetFract.transform.GetChild(0).gameObject.transform.localScale.x>0){

                        for(int i = 0; i < planetFract.transform.childCount; i++){
                                   GameObject g = planetFract.transform.GetChild(i).gameObject;
                                   g.transform.localScale -= disminucionEscala;
                         }
                    yield return new WaitForSeconds(timeBetwenChange);

              }
              BorrarPlaneta(GroupOfPlanets);
              Destroy(GroupOfPlanets);
          }

          public void BorrarPlaneta(GameObject planet){
                    Planet [] newPlanets = new Planet[planets.Length-1];
                    int j= 0;
                    for (int i=0; i<planets.Length; i++) {

                              if (planets[i].planet != planet) {
                                        newPlanets[j] = planets[i];
                                        j++;
                              }
                    }

                    planets =newPlanets;
          }
}

struct Planet{
          public GameObject planet;
          public float dstFromSun;
          public float trasVelocity;
          public float rotVelocity;
          public float angle;
}
