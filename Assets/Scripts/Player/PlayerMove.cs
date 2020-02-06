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
    public float incremmentOfScale;
    public Text scoreText;
    private Vector3 newCameraRot;
    private GameObject  cameraChild;
    private int score =0;

    void Start()
    {
          rb = GetComponent<Rigidbody>();
          UpdateScore ();
          cameraChild = transform.GetChild(1).gameObject;
    }

    void FixedUpdate(){
              Vector2 cameraRot = new Vector2( Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") );
              newCameraRot = new Vector3( 0, cameraRot.normalized.x*rotationSpeed, 0);
              transform.eulerAngles = transform.eulerAngles + newCameraRot;

              Vector3 direction =  transform.forward.normalized;
              rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

   }
    // Update is called once per frame

    void OnCollisionEnter(Collision collision)
    {
        //Ouput the Collision to the console
        if (transform.localScale.x<=collision.gameObject.transform.localScale.x) {
                  UpdateScore("You lost");
                  UnityEditor.EditorApplication.isPlaying = false;
        }else{
                  score++;
                  float newScale = collision.gameObject.transform.localScale.x/incremmentOfScale;
                  transform.localScale+= new Vector3(newScale, newScale, newScale);
                  UpdateScore();
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
