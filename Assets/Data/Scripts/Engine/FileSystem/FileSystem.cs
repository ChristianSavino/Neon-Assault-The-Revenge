using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace Keru.Scripts.Engine.FileSystem
{
    public static class FileSystem
    {
        public static void SaveFile(object objectToJson, string nameFile)
        {
            var json = JsonConvert.SerializeObject(objectToJson);
            var path = $"{Application.persistentDataPath}/{nameFile}.json";

            File.WriteAllText(path, json);
        }

        public static string LoadFile(string nameFile)
        {
            var path = $"{Application.persistentDataPath}/{nameFile}.json";
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }

            return string.Empty;
        }

        public static void DeleteOldVersionFiles()
        {
            DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);

            foreach (FileInfo file in di.GetFiles())
            {
                if(file.Name.Contains("SavedGame"))
                {
                    file.Delete();
                }           
            }
        }
    }
}
