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
          //type of generation
          public enum GenerationMode { PDS};
          public GenerationMode generationMode;
          //type of object
          public enum TypeOfStelarObject { Planet, SolarSystem, Asteroids, SingleAstroObject};
          public TypeOfStelarObject typeOfStelarObject;

          public GameObject objectParent;
          //PDS mode settings
          public float radius;
          public int rejectionSamples;

          /*public bool randomScale;
          public float minScale;
          public float maxScale;*/


}
