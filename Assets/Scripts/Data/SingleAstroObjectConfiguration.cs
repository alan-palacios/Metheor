using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SingleAstroObjectConfiguration : UpdatableData
{
          //Noise mode settings
          public bool generateCollider;
          public GameObject [] models;

          public bool randomScale;
          public float minScale;
          public float maxScale;
}
