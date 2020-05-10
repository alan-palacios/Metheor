using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
public static class SaveSystem
{

          public static void SaveData<T>(T data, string fileName){
                    BinaryFormatter formatter = new BinaryFormatter();
                    string path = Path.Combine(Application.persistentDataPath, fileName);                    
                    using ( FileStream stream = new FileStream(path, FileMode.Create)){
                        formatter.Serialize(stream, data);
                    }
          }

          public static T LoadData<T>(string fileName){
                    string path = Path.Combine(Application.persistentDataPath, fileName);
                    if (File.Exists(path)) {
                              BinaryFormatter formatter = new BinaryFormatter();
                              using ( FileStream stream = new FileStream(path, FileMode.Open)){
                                  T data = (T)Convert.ChangeType(formatter.Deserialize(stream), typeof(T));
                                  return data;
                              }

                    }else{
                              Debug.Log("save file not found in "+path);
                              return default(T);
                    }
          }

          public static void DeleteFile(string path){
              File.Delete(path);
          }


}
