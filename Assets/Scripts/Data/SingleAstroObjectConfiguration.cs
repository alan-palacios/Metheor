using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SingleAstroObjectConfiguration : UpdatableData
{
          //Noise mode settings
          public bool generateCollider;
          public GameObject [] models;
          public Material [] materials;
          public bool randomScale;
          public float minScale;
          public float maxScale;
          public float zRotation;
          public int scoreGived;
          public float decremmentOfScale;

          public bool hasImpulse;
          public float impulsePower;
}
