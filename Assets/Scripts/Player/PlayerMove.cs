using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    float originalSpeed;
    float maxWidthScreenController;
    public float noActionRange;
    public float moveSpeed;
    public float rotationSpeed;
    public float speedOfChange;
    public int modelRotationSpeed;
    public float incremmentOfScale;
    public float impulseVelocityAdded;
    public float impulseTime;
    public Text scoreText;
    private Vector3 newCameraRot;
    private GameObject  cameraChild;
    public GameObject  restartButton;
    public GameObject  pauseButton;
    public GameObject  inGameRestartButton;
    private GameObject  meteoriteModel;
    private int score =0;
    private bool paused=false;
    void Start()
    {
          originalSpeed = moveSpeed;
          QualitySettings.vSyncCount = 0;
          //Time.captureFramerate = 30;
          Application.targetFrameRate = 60;
          rb = GetComponent<Rigidbody>();
          UpdateScore ();
          cameraChild = transform.GetChild(1).gameObject;
          meteoriteModel = transform.GetChild(0).gameObject;

          restartButton = cameraChild.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
          pauseButton = cameraChild.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject;
          inGameRestartButton = cameraChild.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject;

          restartButton.GetComponent<Button>().onClick.AddListener(RestartGame);
          pauseButton.GetComponent<Button>().onClick.AddListener(PauseGame);
          inGameRestartButton.GetComponent<Button>().onClick.AddListener(RestartGame);

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
                                UpdateScore("You lost");
                                Time.timeScale = 0;
                                restartButton.SetActive(true);
                                //UnityEditor.EditorApplication.isPlaying = false;
                      }else{

                                StartCoroutine( EstablecerVelocidadOriginal(impulseVelocityAdded) );
                                score+=GroupSolarSystem.GetComponent<SolarSystem>().solarSystemConfiguration.solarSystemData.scoreGived;
                                float newScale = GroupOfPlanets.transform.localScale.x/incremmentOfScale;
                                transform.localScale+= new Vector3(newScale, newScale, newScale);
                                UpdateScore();
                                objColl.SetActive(false);
                                GroupOfPlanets.transform.GetChild(1).gameObject.SetActive(true);
                                StartCoroutine( GroupSolarSystem.GetComponent<SolarSystem>().DestruirPlaneta(GroupOfPlanets) );

                      }
             }
             else if (objColl.tag == "Asteroid"){
                       //StartCoroutine( EstablecerVelocidadOriginal(impulseVelocityAdded) );
                       GameObject GroupOfAsteroids = objColl.transform.parent.gameObject;
                      //StartCoroutine( EstablecerVelocidadOriginal(impulseVelocityAdded) );
                      score+=GroupOfAsteroids.GetComponent<Asteroids>().asteroidsConfiguration.scoreGived;
                      float newScale = 1/GroupOfAsteroids.GetComponent<Asteroids>().asteroidsConfiguration.incremmentOfScale;
                      transform.localScale+= new Vector3(newScale, newScale, newScale);
                      UpdateScore();
                      StartCoroutine( GroupOfAsteroids.GetComponent<Asteroids>().DestruirAsteroide(objColl) );
             }

    }

    public IEnumerator EstablecerVelocidadOriginal(float addedVelocity){
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

              moveSpeed=originalSpeed;

    }
    void RestartGame()
    {
              Time.timeScale = 1;
             restartButton.SetActive(true);
              SceneManager.LoadScene("MainScene");
    }

    void PauseGame()
    {
              Time.timeScale = paused?1:0;
              paused = !paused;
    }
    void UpdateScore ()
    {
        scoreText.text = "Score: " + score.ToString ();
    }

    void UpdateScore (string txt)
    {
        scoreText.text =  txt;
    }
}
