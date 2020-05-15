using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class EnemyConfiguration : UpdatableData
{

        public bool multipleObjects;

        [Header("Multiple Objects")]
        public GameObject multipleObjectsParent;
        public GameObject [] objectsModels;
        public int minNumOfObjects;
        public int maxNumOfObjects;
        public float maxDst;
        public float minDst;

        [Header("Single Object")]
        public GameObject [] models;

        [Header("General")]
        public bool randomScale;
        public float minScale;
        public float maxScale;
        public float zRotation;
        public float timeOfLife;
        public float timeBetwenChange;
        public float movePower;
        public int rotationVelocity;



}
