using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BlackHole : MonoBehaviour
{

    public Material [] colors;
    public int particlesOfSameColor;
    public GameObject particleObject;
    public float minRadius;
    public float radiusIncrement;

    public float minSpeed;
    public float speedIncrement;

    public float minSize;
    public float sizeIncrement;

    public float gravityConstant;

    Vector3 direction;
    float angleIncremment;
    bool atrayendoObjeto;
    GameObject metheor;

    void Start()
    {
              angleIncremment=360/particlesOfSameColor;
              float initAngle=0;
              float shapeRadius=minRadius;
              float partSize=minSize;

              Vector3 angles = new Vector3(0,0,initAngle);

              for (int i=0; i<colors.Length; i++) {
                        initAngle=0;
                        for (int j=0; j<particlesOfSameColor; j++) {
                                  angles = new Vector3(0,0,initAngle);
                                  GameObject particleInst = GameObject.Instantiate( particleObject, Vector3.zero, particleObject.transform.rotation ) as GameObject;
                                  particleInst.transform.SetParent(transform,false);
                                  ParticleSystem partSys = particleInst.GetComponent<ParticleSystem>();
                                  particleInst.GetComponent<Renderer>().sharedMaterial= colors[i];

                                  var main = partSys.main;
                                  main.startSize = partSize;

                                  var shape = partSys.shape;
                                  shape.rotation = angles;
                                  shape.radius=shapeRadius;

                                  initAngle+=angleIncremment;
                        }
                        shapeRadius+=radiusIncrement;
                        partSize+=sizeIncrement;
              }
    }

    void FixedUpdate(){
              if (atrayendoObjeto) {
                        direction =  transform.position - metheor.transform.position;
                        float gravityAceleration = gravityConstant/Vector3.Distance(transform.position,metheor.transform.position);
                        metheor.transform.Translate(direction * (gravityConstant)* Time.fixedDeltaTime, Space.World );
              }
   }

    public void AtraerObjeto(Transform transform){
              atrayendoObjeto=true;
              this.metheor = transform.gameObject;

   }

}
