using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

          public static void SaveData(PlayerData data, string fileName){
                    BinaryFormatter formatter = new BinaryFormatter();
                    string path = Path.Combine(Application.persistentDataPath, fileName);                    
                    using ( FileStream stream = new FileStream(path, FileMode.Create)){
                        formatter.Serialize(stream, data);
                    }
          }

          public static PlayerData LoadData(string fileName){
                    string path = Path.Combine(Application.persistentDataPath, fileName);
                    if (File.Exists(path)) {
                              BinaryFormatter formatter = new BinaryFormatter();
                              using ( FileStream stream = new FileStream(path, FileMode.Open)){
                                  PlayerData data = formatter.Deserialize(stream) as PlayerData;
                                  return data;
                              }

                    }else{
                              Debug.Log("save file not found in "+path);
                              return null;
                    }
          }


}
