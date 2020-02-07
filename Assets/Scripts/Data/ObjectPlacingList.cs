using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ObjectPlacingList : UpdatableData
{
    public ObjectData [] objectsSettings;
}


[System.Serializable]
public struct ObjectData
{
          public string nombre;
          public enum GenerationMode { PDS};
          public GenerationMode generationMode;

          //PDS mode settings
          public float radius;
          public int rejectionSamples;

          //type of object
          public enum TypeOfStelarObject { Planet, SolarSystem};
          public TypeOfStelarObject typeOfStelarObject;
          public GameObject solarSystemParent;

          //Noise mode settings
          public bool generateCollider;
          public GameObject [] modelos;

          public bool randomScale;
          public float minScale;
          public float maxScale;

          public float startHeight;
          public float endHeight;
          public float offsetHeight;

          public bool randomMaterial;
          public Material [] materiales;

}
