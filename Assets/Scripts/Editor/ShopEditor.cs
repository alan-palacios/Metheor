using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (Shop))]
public class ShopEditor : Editor {

          public override void OnInspectorGUI () {
                    Shop shop = (Shop)target;

                   DrawDefaultInspector();

                   if(GUILayout.Button("Generar Lista de Productos")) {
                             shop.GenerateProducts();
                   }
              }
}
