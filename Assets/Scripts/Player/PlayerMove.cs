using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    public ObjectPlacingList objPlacingList;
    public ExplosionConfiguration expConf;
    public GeneralGameControl gameControl;

    public GameObject  cameraChild;
    public GameObject  meteoriteModel;
    private Rigidbody rb;
    float originalSpeed;
    float maxWidthScreenController;
    public float noActionRange;
    public float moveSpeed;
    public float rotationSpeed;
    public float speedOfChange;
    public int modelRotationSpeed;
    //public float incremmentOfScale;
    public float impulseVelocityAdded;
    public float impulseTime;

    public float velocityBoost, transicionTime, boostTime;

    public GameObject explosionParticles;
    public float explosionOffset;


    private Vector3 newCameraRot;

    public int score =0;
    private bool receivingImpulse=false, withSpeedBoost=false, collidingSatellite=false;
    void Start()
    {
          originalSpeed = moveSpeed;
          QualitySettings.vSyncCount = 0;
          //Time.captureFramerate = 30;
          Application.targetFrameRate = 60;
          rb = GetComponent<Rigidbody>();
          gameControl.UpdateScore ();

          maxWidthScreenController = Screen.width/4;
    }

    void FixedUpdate(){
              //Input.gyro.enabled = true;
              //float initialOrientationX = Input.gyro.gravity.x;
              //float initialOrientationY = Input.gyro.gravity.y;
              //Vector2 cameraRot = new Vector2( initialOrientationX, initialOrientationY );
              Vector2 cameraRot = new Vector2( Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") );

              if (Input.touchCount >0) {
                        Touch touch = Input.GetTouch(0);
                        float x = (touch.position.x - Screen.width/2) / (Screen.width/2) ;

                        if ( x>-noActionRange && x<noActionRange) {
                                  x=0;
                        }
                        x*=speedOfChange;
                        /*if (x<-1) {
                                  x=-1;
                        }else if (x > 1) {
                                  x=1;
                        }*/

                        cameraRot = new Vector3(x,0,0);

                        /*if (touch.position.x < Screen.width/2) {
                               cameraRot = new Vector3(-1,0,0);
                        }else if (touch.position.x > Screen.width/2) {
                               cameraRot = new Vector3(1,0,0);
                        }*/
              }



              newCameraRot = new Vector3( 0, cameraRot.x*rotationSpeed, 0);
              transform.eulerAngles = transform.eulerAngles + newCameraRot;

              Vector3 direction =  transform.forward;
              transform.Translate(direction * moveSpeed* Time.fixedDeltaTime, Space.World );
              //rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);


   }

    void Update(){

              meteoriteModel.transform.Rotate(  Time.deltaTime * modelRotationSpeed * new Vector3( 0.5f, 0, 1) );
              //meteoriteModel.transform.rotation = Quaternion.RotateTowards(meteoriteModel.transform.rotation,
                    //Quaternion.Euler(1, 0,1), modelRotationSpeed * Time.deltaTime);
              //meteoriteModel.transform.rotation= Quaternion.Lerp (meteoriteModel.transform.rotation, Vector3.up , modelRotationSpeed * 1f * Time.deltaTime);
   }

    void OnCollisionEnter(Collision collision)
    {
              GameObject objColl = collision.gameObject;
                      if (objColl.tag == "CompletePlanet"){
                                 //father of the complete and fract planet
                                 GameObject GroupOfPlanets = objColl.transform.parent.gameObject;
                                 //father solar system with all the types of planet
                                 GameObject GroupSolarSystem = GroupOfPlanets.transform.parent.gameObject;
                                 //Ouput the Collision to the console
                                if (transform.localScale.x<=GroupOfPlanets.transform.localScale.x) {
                                          StartCoroutine(MostrarExplosion());
                                          gameControl.UpdateScore("You lost");
                                          gameControl.OnLost();
                                          //UnityEditor.EditorApplication.isPlaying = false;
                                }else{

                                          score+=GroupSolarSystem.GetComponent<SolarSystem>().solarSystemConfiguration.solarSystemData.scoreGived;
                                          //float newScale = GroupOfPlanets.transform.localScale.x/incremmentOfScale;
                                          //transform.localScale+= new Vector3(newScale, newScale, newScale);
                                          gameControl.UpdateScore();
                                          StartCoroutine(MostrarExplosion());
                                          if (!receivingImpulse && !withSpeedBoost) {
                                                    StartCoroutine( DarImpulso(impulseVelocityAdded) );
                                          }
                                          objColl.SetActive(false);
                                          GroupOfPlanets.transform.GetChild(1).gameObject.SetActive(true);
                                          StartCoroutine( GroupSolarSystem.GetComponent<SolarSystem>().DestruirPlaneta(GroupOfPlanets) );

                                }
                      }else if (objColl.tag == "Asteroid"){
                                 //StartCoroutine( EstablecerVelocidadOriginal(impulseVelocityAdded) );
                                 GameObject GroupOfAsteroids = objColl.transform.parent.gameObject;
                                //StartCoroutine( EstablecerVelocidadOriginal(impulseVelocityAdded) );
                                score+=GroupOfAsteroids.GetComponent<Asteroids>().asteroidsConfiguration.scoreGived;
                                float newScale = 1/GroupOfAsteroids.GetComponent<Asteroids>().asteroidsConfiguration.incremmentOfScale;
                                //transform.localScale+= new Vector3(newScale, newScale, newScale);
                                gameControl.UpdateScore();
                                StartCoroutine(MostrarExplosion());
                                if (!receivingImpulse && !withSpeedBoost) {
                                          StartCoroutine( DarImpulso(impulseVelocityAdded) );
                                }
                                StartCoroutine( GroupOfAsteroids.GetComponent<Asteroids>().DestruirAsteroide(objColl) );
                      }else if (objColl.tag == "Satellite"){

                                 GameObject GroupOfSatellite = objColl.transform.parent.gameObject;
                                 GameObject MasterParent = GroupOfSatellite.transform.parent.gameObject;

                                 for(int i = 0; i < GroupOfSatellite.transform.childCount; i++){
                                            GameObject g = GroupOfSatellite.transform.GetChild(i).gameObject;
                                            g.GetComponent<Rigidbody>().isKinematic = false;
                                  }

                                score+=MasterParent.GetComponent<SingleAstroObject>().singleAstroObjectConfiguration.scoreGived;
                                //float newScale = 1/MasterParent.GetComponent<SingleAstroObject>().singleAstroObjectConfiguration.decremmentOfScale;
                                gameControl.UpdateScore();
                                if (!collidingSatellite) {
                                          StartCoroutine(MostrarExplosion());
                                }
                                if (!receivingImpulse && !withSpeedBoost) {
                                          StartCoroutine( DarImpulso(impulseVelocityAdded) );
                                }
                                StartCoroutine( MasterParent.GetComponent<SingleAstroObject>().DestruirObjeto(objColl, 0.1f) );
                      }else if (objColl.tag == "Astronaut"){

                                 GameObject GroupOfSatellite = objColl;
                                 GameObject MasterParent = GroupOfSatellite.transform.parent.gameObject;

                                //objColl.gameObject.GetComponent<Collider>().enabled = false;
                                //objColl.tag="Untagged";
                                objColl.GetComponent<Rigidbody>().AddForce(
                                                    transform.forward*6f,
                                                     ForceMode.Impulse);

                                score+=MasterParent.GetComponent<SingleAstroObject>().singleAstroObjectConfiguration.scoreGived;
                                objPlacingList.objectsSettings[5].radius+= objPlacingList.objectsSettings[5].radiusDecremment;
                                gameControl.UpdateScore();
                                StartCoroutine(MostrarExplosion());
                                if (!receivingImpulse && !withSpeedBoost) {
                                          StartCoroutine( DarImpulso(impulseVelocityAdded) );
                                }
                                //Destroy(objColl);
                                StartCoroutine( MasterParent.GetComponent<SingleAstroObject>().DestruirObjeto(objColl, 0.05f) );
                      }else if (objColl.tag == "MeteorEnemy"){
                                 gameControl.UpdateScore("You lost");
                                 StartCoroutine(MostrarExplosion());
                                 gameControl.OnLost();
                      }else if(objColl.tag == "SpeedBoost"){
                                GameObject MasterParent = objColl.transform.parent.gameObject;
                                StartCoroutine(MostrarExplosion());
                                StartCoroutine( DarVelocidad() );
                                StartCoroutine(MasterParent.GetComponent<CollectibleObject>().DestruirObjeto(MasterParent));
                      }

    }

    public IEnumerator DarImpulso(float addedVelocity){
              receivingImpulse=true;
              float fragmentOfTime = impulseTime/20;
              float fragmentOfVel = addedVelocity/10;
              for (int i=0 ; i<10; i+=1) {
                        moveSpeed+=fragmentOfVel;
                        yield return new WaitForSeconds(fragmentOfTime);
              }
              for (int i=0 ; i<10; i+=1) {
                        moveSpeed-=fragmentOfVel;
                        yield return new WaitForSeconds(fragmentOfTime);
              }
              if (!withSpeedBoost) {
                        moveSpeed=originalSpeed;
              }
              receivingImpulse=false;
    }

    public IEnumerator DarVelocidad(){
              withSpeedBoost=true;
              float fragmentOfTime = transicionTime/20;
              float fragmentOfVel = velocityBoost/10;
              for (int i=0 ; i<10; i+=1) {
                        moveSpeed+=fragmentOfVel;
                        yield return new WaitForSeconds(fragmentOfTime);
              }
              yield return new WaitForSeconds(boostTime);
              for (int i=0 ; i<10; i+=1) {
                        moveSpeed-=fragmentOfVel;
                        yield return new WaitForSeconds(fragmentOfTime);
              }
              moveSpeed=originalSpeed;
              withSpeedBoost=false;
    }

    public IEnumerator MostrarExplosion(){
              collidingSatellite=true;
              GameObject objectInstanced = GameObject.Instantiate(expConf.explosionParticles,  Vector3.zero  , expConf.explosionParticles.transform.rotation ) as GameObject;
              objectInstanced.transform.SetParent( transform, false);
              objectInstanced.transform.position+=transform.forward*expConf.explosionOffset;
              Vector3 instPos = objectInstanced.transform.position;
              instPos.y=1;
              objectInstanced.transform.position= instPos;
              objectInstanced.GetComponent<Renderer>().material.color = expConf.colors[Random.Range(0,expConf.colors.Length) ];
              yield return new WaitForSeconds(0.5f);
              Destroy(objectInstanced);
              collidingSatellite=false;
    }

}
