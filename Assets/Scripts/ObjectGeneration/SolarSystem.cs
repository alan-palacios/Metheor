using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class SolarSystem : MonoBehaviour
{
           GameObject solarSystemObject;
           GameObject sun;
           Planet [] planets;
          public SolarSystemConfiguration solarSystemConfiguration;
          public float sunVelocityRotation;
          void Start(){
                    int sunModelsCount = solarSystemConfiguration.solarSystemData.suns.Length;
                    int planetsModelsCount = solarSystemConfiguration.solarSystemData.planets.Length;

                    GameObject sunPlaced = solarSystemConfiguration.solarSystemData.suns[Random.Range(0,sunModelsCount)];

                    //MeshFilter viewedModelFilter = objectPlaced.GetComponent<MeshFilter>();

                    Vector3 angles = new Vector3( 0, Random.Range(0, 36)*10, 0);
                    sun = GameObject.Instantiate(sunPlaced, new Vector3(  0, 0  ,0)  , Quaternion.Euler(angles) ) as GameObject;
                    sun.transform.SetParent( transform, false);
                    float newScale = Random.Range( solarSystemConfiguration.solarSystemData.SunMinScale, solarSystemConfiguration.solarSystemData.SunMaxScale );
                    sun.transform.localScale = Vector3.one*Mathf.Floor (newScale / 0.1f)*0.1f;


                    int numOfPlanets = Random.Range(solarSystemConfiguration.solarSystemData.minNumOfPlanets,
                                                                                solarSystemConfiguration.solarSystemData.maxNumOfPlanets);
                    planets = new Planet[numOfPlanets];

                    for (int i = 0; i<numOfPlanets; i++) {
                              GameObject planetPlaced = solarSystemConfiguration.solarSystemData.planets[Random.Range(0,planetsModelsCount)];
                              float dstFromSun = Random.Range( solarSystemConfiguration.solarSystemData.minDst, solarSystemConfiguration.solarSystemData.maxDst);
                              float rndAngle = Random.Range (0,2*Mathf.PI);

                              Vector3 position = new Vector3( dstFromSun * Mathf.Cos(rndAngle), 0, dstFromSun * Mathf.Sin(rndAngle) );
                              angles = new Vector3( 0, Random.Range(0, 36)*10, 0);

                              planets[i].dstFromSun = dstFromSun;
                              planets[i].planet = GameObject.Instantiate( planetPlaced, position, Quaternion.Euler(angles) ) as GameObject;
                              planets[i].planet.transform.SetParent( transform, false);
                              newScale = Random.Range( solarSystemConfiguration.solarSystemData.PlanetsMinScale, solarSystemConfiguration.solarSystemData.PlanetsMaxScale );
                              planets[i].planet.transform.localScale = Vector3.one*Mathf.Floor (newScale / 0.1f)*0.1f;
                    }

                    //sun = GameObject.Instantiate(objectPlaced, new Vector3(  xCoord, heightCoord  ,yCoord)  , Quaternion.Euler(angles), parent ) as GameObject;

          }

          void Update(){
                    //Vector3 sunAngle = sun.transform.rota;
                    sun.transform.Rotate(0, sunVelocityRotation, 0, Space.Self);
          }
}

struct Planet{
          public GameObject planet;
          public float dstFromSun;
}
