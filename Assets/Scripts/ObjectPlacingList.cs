﻿using System.Collections;
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

          //Noise mode settings          
          public bool generateCollider;
          public GameObject [] modelos;

          public float startHeight;
          public float endHeight;
          public float offsetHeight;

          //PDS mode settings
          public float radius;
          public int rejectionSamples;


          public bool randomMaterial;
          public Material [] materiales;

          public bool randomScale;
          public float minScale;
          public float maxScale;
}
