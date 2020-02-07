using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SolarSystemConfiguration : UpdatableData
{
    public SolarSystemData solarSystemData;
}

[System.Serializable]
public struct SolarSystemData
{

          //Noise mode settings
          public bool generateCollider;
          public GameObject [] suns;
          public GameObject [] planets;

          public int minNumOfPlanets;
          public int maxNumOfPlanets;

          public float maxDst;
          public float minDst;

          public bool randomScale;
          public float PlanetsMinScale;
          public float PlanetsMaxScale;

          public float SunMinScale;
          public float SunMaxScale;
}
