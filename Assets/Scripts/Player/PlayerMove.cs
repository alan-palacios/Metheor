using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    public int moveSpeed;
    public int rotationSpeed;
    public int modelRotationSpeed;
    public float incremmentOfScale;
    public Text scoreText;
    private Vector3 newCameraRot;
    private GameObject  cameraChild;
    private GameObject  meteoriteModel;
    private int score =0;

    void Start()
    {
          rb = GetComponent<Rigidbody>();
          UpdateScore ();
          cameraChild = transform.GetChild(1).gameObject;
          meteoriteModel = transform.GetChild(0).gameObject;
    }

    void FixedUpdate(){
              Input.gyro.enabled = true;
              float initialOrientationX = Input.gyro.rotationRateUnbiased.x;
              float initialOrientationY = Input.gyro.rotationRateUnbiased.y;

              Vector2 cameraRot = new Vector2( initialOrientationX, initialOrientationY );
              //Vector2 cameraRot = new Vector2( Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") );
              newCameraRot = new Vector3( 0, cameraRot.normalized.x*rotationSpeed, 0);
              transform.eulerAngles = transform.eulerAngles + newCameraRot;

              Vector3 direction =  transform.forward.normalized;
              rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

   }

    void Update(){

              meteoriteModel.transform.Rotate(  Time.deltaTime * modelRotationSpeed * new Vector3( 0.5f, 0, 1) );
              //meteoriteModel.transform.rotation = Quaternion.RotateTowards(meteoriteModel.transform.rotation,
                    //Quaternion.Euler(1, 0,1), modelRotationSpeed * Time.deltaTime);
              //meteoriteModel.transform.rotation= Quaternion.Lerp (meteoriteModel.transform.rotation, Vector3.up , modelRotationSpeed * 1f * Time.deltaTime);
   }

    void OnCollisionEnter(Collision collision)
    {
        //Ouput the Collision to the console
        if (transform.localScale.x<=collision.gameObject.transform.localScale.x) {
                  UpdateScore("You lost");
                  //UnityEditor.EditorApplication.isPlaying = false;
        }else{
                  score++;
                  float newScale = collision.gameObject.transform.localScale.x/incremmentOfScale;
                  transform.localScale+= new Vector3(newScale, newScale, newScale);
                  UpdateScore();
                  if( collision.gameObject.transform.parent.gameObject.GetComponent<SolarSystem>()!=null){
                            collision.gameObject.transform.parent.gameObject.GetComponent<SolarSystem>().BorrarPlaneta(collision.gameObject);
                  }
                  Destroy(collision.gameObject);
        }
    }

    void UpdateScore ()
    {
        scoreText.text = "Score: " + score.ToString ();
    }

    void UpdateScore (string txt)
    {
        scoreText.text = "Score: " + txt;
    }
}
