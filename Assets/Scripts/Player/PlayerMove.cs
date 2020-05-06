using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
     [Header("Scripts And Data")]
      public ObjectPlacingList objPlacingList;
      public CharactersList charactersList;
      public ExplosionConfiguration expConf;
      public GeneralGameControl gameControl;
      public SoundControl soundCtrl;

     [Header("Character")]
     public static int characterIndex=0;

     [Header("Hierarchy")]
    public GameObject  cameraChild;
    public GameObject  metheorElements;

    //public GameObject  meteoritePrefab;
    private GameObject  meteoriteModel;

    public GameObject  meteoriteModelFract;

    //public GameObject [] meteorParticlesPrefab;
    private GameObject [] meteorParticles;

     [Header("Effects")]
    public Color congeledColor;
    public GameObject explosionParticles;
    public float explosionOffset;

     [Header("Move Configuration")]

    public float noActionRange;
    public float moveSpeed;
    public float rotationSpeed;
    public float speedOfChange;
    public int modelRotationSpeed;
    //public float incremmentOfScale;
    public float impulseVelocityAdded;
    public float impulseTime;

    public float velocityBoost, transicionTime, boostTime;
    private Vector3 newCameraRot;

    private Rigidbody rb;
    float originalSpeed;
    float maxWidthScreenController;
     [Header("Others")]
    public static int score = 0;
    private bool receivingImpulse=false, withSpeedBoost=false, collidingSatellite=false, alive=true;


    void Start()
    {
          originalSpeed = moveSpeed;
          QualitySettings.vSyncCount = 0;
          Application.targetFrameRate = 60;
          rb = GetComponent<Rigidbody>();
          //gameControl.UpdateScore ();
          maxWidthScreenController = Screen.width/4;

          //instancing metheor
          meteoriteModel = Instantiate(charactersList.charactersData[characterIndex].meteoritePrefab, Vector3.zero, Quaternion.identity);
          meteoriteModel.transform.SetParent(metheorElements.transform,false);

          meteoriteModelFract = charactersList.charactersData[characterIndex].meteoriteModelFract;
          meteorParticles = new GameObject[3];

          meteoriteModel.GetComponent<Renderer>().material = charactersList.charactersData[characterIndex].material;
          foreach (Transform child in meteoriteModelFract.transform) {
              child.gameObject.GetComponent<Renderer>().material = charactersList.charactersData[characterIndex].material;
          }

          GameObject meteorP1 = charactersList.charactersData[characterIndex].meteorParticlesPrefab[0];
          GameObject meteorP2 = charactersList.charactersData[characterIndex].meteorParticlesPrefab[1];
          GameObject meteorP3 = charactersList.charactersData[characterIndex].meteorParticlesPrefab[2];

          meteorParticles[0] = Instantiate(meteorP1, meteorP1.transform.position, meteorP1.transform.rotation);
          meteorParticles[0].transform.SetParent(meteoriteModel.transform,false);

          meteorParticles[1] = Instantiate(meteorP2, meteorP2.transform.position, meteorP2.transform.rotation);
          meteorParticles[1].transform.SetParent(meteoriteModel.transform,false);

          meteorParticles[2] = Instantiate(meteorP3, meteorP3.transform.position, meteorP3.transform.rotation);
          meteorParticles[2].transform.SetParent(metheorElements.transform,false);
    }

    void FixedUpdate(){

              if (alive) {
                        Vector2 cameraRot = new Vector2( Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") );

                        if (Input.touchCount >0) {
                                  Touch touch = Input.GetTouch(0);
                                  float x = (touch.position.x - Screen.width/2) / (Screen.width/2) ;

                                  if ( x>-noActionRange && x<noActionRange) {
                                            x=0;
                                  }
                                  x*=speedOfChange;

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
              }

   }

    void Update(){

              meteoriteModel.transform.Rotate(  Time.deltaTime * modelRotationSpeed * new Vector3( 0.5f, 0, 1) );
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
                                if (transform.localScale.x<=GroupOfPlanets.transform.localScale.x * GroupSolarSystem.transform.localScale.x + 0.15f) {
                                          Die();
                                          //UnityEditor.EditorApplication.isPlaying = false;
                                }else{
                                          ObjectGenerator.modifyAllPlacementValues(objPlacingList, score);
                                          score+=GroupSolarSystem.GetComponent<SolarSystem>().solarSystemConfiguration.solarSystemData.scoreGived;
                                          //float newScale = GroupOfPlanets.transform.localScale.x/incremmentOfScale;
                                          //transform.localScale+= new Vector3(newScale, newScale, newScale);
                                          gameControl.UpdateScore( GroupSolarSystem.GetComponent<SolarSystem>().solarSystemConfiguration.solarSystemData.scoreGived);
                                          StartCoroutine(MostrarExplosion());
                                          if (!receivingImpulse && !withSpeedBoost) {
                                                    StartCoroutine( DarImpulso(impulseVelocityAdded) );
                                          }
                                          objColl.SetActive(false);
                                          GroupOfPlanets.transform.GetChild(1).gameObject.SetActive(true);
                                          StartCoroutine( GroupSolarSystem.GetComponent<SolarSystem>().DestruirPlaneta(GroupOfPlanets) );

                                }
                      }else if (objColl.tag == "Asteroid"){
                                ObjectGenerator.modifyAllPlacementValues(objPlacingList, score);

                                 //StartCoroutine( EstablecerVelocidadOriginal(impulseVelocityAdded) );
                                 GameObject GroupOfAsteroids = objColl.transform.parent.gameObject;
                                //StartCoroutine( EstablecerVelocidadOriginal(impulseVelocityAdded) );

                                score+=GroupOfAsteroids.GetComponent<Asteroids>().asteroidsConfiguration.scoreGived;
                                float newScale = 1/GroupOfAsteroids.GetComponent<Asteroids>().asteroidsConfiguration.incremmentOfScale;
                                //transform.localScale+= new Vector3(newScale, newScale, newScale);
                                gameControl.UpdateScore( GroupOfAsteroids.GetComponent<Asteroids>().asteroidsConfiguration.scoreGived);
                                StartCoroutine(MostrarExplosion());
                                if (!receivingImpulse && !withSpeedBoost) {
                                          StartCoroutine( DarImpulso(impulseVelocityAdded) );
                                }
                                StartCoroutine( GroupOfAsteroids.GetComponent<Asteroids>().DestruirAsteroide(objColl) );
                      }else if (objColl.tag == "Satellite"){

                                 GameObject GroupOfSatellite = objColl.transform.parent.gameObject;
                                 GameObject MasterParent = GroupOfSatellite.transform.parent.gameObject;
                                 if (transform.localScale.x<=GroupOfSatellite.transform.localScale.x * MasterParent.transform.localScale.x) {
                                          Die();
                                          //UnityEditor.EditorApplication.isPlaying = false;
                                }else{
                                          ObjectGenerator.modifyAllPlacementValues(objPlacingList, score);
                                           for(int i = 0; i < GroupOfSatellite.transform.childCount; i++){
                                                      GameObject g = GroupOfSatellite.transform.GetChild(i).gameObject;
                                                      g.GetComponent<Rigidbody>().isKinematic = false;
                                            }

                                          score+=MasterParent.GetComponent<SingleAstroObject>().singleAstroObjectConfiguration.scoreGived;
                                          //float newScale = 1/MasterParent.GetComponent<SingleAstroObject>().singleAstroObjectConfiguration.decremmentOfScale;
                                          gameControl.UpdateScore( MasterParent.GetComponent<SingleAstroObject>().singleAstroObjectConfiguration.scoreGived);
                                          if (!collidingSatellite) {
                                                    StartCoroutine(MostrarExplosion());
                                          }
                                          if (!receivingImpulse && !withSpeedBoost) {
                                                    StartCoroutine( DarImpulso(impulseVelocityAdded) );
                                          }
                                          StartCoroutine( MasterParent.GetComponent<SingleAstroObject>().DestruirObjeto(objColl, 0.05f) );
                                }
                      }else if (objColl.tag == "Astronaut"){

                                 GameObject GroupOfSatellite = objColl;
                                 GameObject MasterParent = GroupOfSatellite.transform.parent.gameObject;
                                 ObjectGenerator.modifyAllPlacementValues(objPlacingList, score);
                                //objColl.gameObject.GetComponent<Collider>().enabled = false;
                                //objColl.tag="Untagged";
                                objColl.GetComponent<Rigidbody>().AddForce(
                                                    transform.forward*6f,
                                                     ForceMode.Impulse);

                                score+=MasterParent.GetComponent<SingleAstroObject>().singleAstroObjectConfiguration.scoreGived;

                                gameControl.UpdateScore( MasterParent.GetComponent<SingleAstroObject>().singleAstroObjectConfiguration.scoreGived);
                                StartCoroutine(MostrarExplosion());
                                if (!receivingImpulse && !withSpeedBoost) {
                                          StartCoroutine( DarImpulso(impulseVelocityAdded) );
                                }
                                //Destroy(objColl);
                                StartCoroutine( MasterParent.GetComponent<SingleAstroObject>().DestruirObjeto(objColl, 0.05f) );
                      }else if (objColl.tag == "MeteorEnemy"){

                                Die();
                      }else if(objColl.tag == "SpeedBoost"){
                                GameObject MasterParent = objColl.transform.parent.gameObject;
                                StartCoroutine(MostrarExplosion());
                                StartCoroutine( DarVelocidad() );
                                StartCoroutine(MasterParent.GetComponent<CollectibleObject>().DestruirObjeto(MasterParent));
                      }else if(objColl.tag == "Coin"){
                                soundCtrl.PlaySound("coin");
                                GameObject MasterParent = objColl.transform.parent.gameObject;
                                gameControl.GiveCoin();
                                StartCoroutine(MostrarExplosion());
                                StartCoroutine(MasterParent.GetComponent<CollectibleObject>().DestruirObjeto(MasterParent));
                      }else if(objColl.tag == "Bomb"){
                                soundCtrl.StopSound("fire");
                                GameObject MasterParent = objColl.transform.parent.gameObject;
                                StartCoroutine(MostrarExplosion());
                                StartCoroutine(MostrarExplosion());
                                StartCoroutine(MasterParent.GetComponent<CollectibleObject>().DestruirObjeto(MasterParent));
                                Die();
                      }else if(objColl.tag == "Ice"){
                                GameObject MasterParent = objColl.transform.parent.gameObject;
                                StartCoroutine(MostrarExplosion());
                                soundCtrl.StopSound("fire");
                                soundCtrl.PlaySound("freeze");
                                StartCoroutine(Congelarse());
                                StartCoroutine(MasterParent.GetComponent<CollectibleObject>().DestruirObjeto(MasterParent));
                      }else if(objColl.tag == "BlackHole"){
                                soundCtrl.StopSound("fire");
                                Time.timeScale=0.5f;
                                objColl.GetComponent<BlackHole>().AtraerObjeto(transform);
                                DieWithoutFracture();
                      }else if(objColl.tag == "Teleporter"){
                                soundCtrl.StopSound("fire");
                                Time.timeScale=0.8f;
                                objColl.GetComponent<BlackHole>().AtraerObjeto(transform);
                                Teleport();
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

    public IEnumerator Congelarse(){
              moveSpeed=0;
              alive=false;
              for (int i=0; i<meteorParticles.Length; i++) {
                        meteorParticles[i].SetActive(false);
              }
              Material mat = meteoriteModel.GetComponent<Renderer>().material;
              Color originalColor = mat.GetColor("_Color");
              mat.SetColor( "_Color", congeledColor );

              yield return new WaitForSeconds(1.7f);

              moveSpeed=originalSpeed;
              alive=true;
              for (int i=0; i<meteorParticles.Length; i++) {
                        meteorParticles[i].SetActive(true);
              }
              soundCtrl.PlaySound("unfreeze");
              soundCtrl.PlaySound("fire");
              mat.SetColor( "_Color", originalColor );

    }

    public void Die(){
            soundCtrl.StopSound("fire");
            soundCtrl.PlaySound("hit");
              alive=false;
              StartCoroutine(MostrarExplosion());
              StartCoroutine(gameControl.OnLost() );
              metheorElements.SetActive(false);
              GameObject objectInstanced = GameObject.Instantiate(meteoriteModelFract,  meteoriteModel.transform.position  , meteoriteModel.transform.rotation ) as GameObject;
              StartCoroutine(DestroyMetheor(objectInstanced));
   }
    public void DieWithoutFracture(){
        soundCtrl.StopSound("fire");
              alive=false;
              StartCoroutine(MostrarExplosion());
              StartCoroutine(gameControl.OnLost() );
              StartCoroutine(DestroyCompleteMetheor(meteoriteModel));
   }
    public void Teleport(){

              alive=false;
              StartCoroutine(MostrarExplosion());
              StartCoroutine(gameControl.PassLevel() );
              StartCoroutine(DestroyCompleteMetheor(meteoriteModel));
   }

   public IEnumerator DestroyMetheor(GameObject metheor){
       Vector3 disminucionEscala = Vector3.one*0.16f;
       float timeBetwenChange = 0.1f;

       while(metheor.transform.GetChild(0).gameObject.transform.localScale.x>0){

                 for(int i = 0; i < metheor.transform.childCount; i++){
                            GameObject g = metheor.transform.GetChild(i).gameObject;
                            g.transform.localScale -= disminucionEscala;
                            if (g.transform.localScale.x<=0) {
                                    g.transform.localScale=Vector3.zero;
                                    Destroy(metheor);
                                    yield break;
                          }
                  }
                  yield return new WaitForSeconds(timeBetwenChange);

       }

   }
   public IEnumerator DestroyCompleteMetheor(GameObject metheor){
       Vector3 disminucionEscala = Vector3.one*0.16f;
       float timeBetwenChange = 0.1f;
       for (int i=0; i<meteorParticles.Length; i++) {
                 meteorParticles[i].SetActive(false);
       }
       while(metheor.transform.localScale.x>0){
                  metheor.transform.localScale -= disminucionEscala;
                  if (metheor.transform.localScale.x<=0) {
                          metheor.transform.localScale=Vector3.zero;
                    }
                    yield return new WaitForSeconds(timeBetwenChange);
       }
       //Destroy(metheor);
   }

}
