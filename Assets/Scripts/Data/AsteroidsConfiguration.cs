using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AsteroidsConfiguration : UpdatableData
{
          public GameObject [] asteroidsModels;
          public Material materials;
          public int scoreGived;
          public float incremmentOfScale;
          public int minNumOfAsteroids;
          public int maxNumOfAsteroids;

          public float AsteroidsMinScale;
          public float AsteroidsMaxScale;

          public float maxDst;
          public float minDst;


}
