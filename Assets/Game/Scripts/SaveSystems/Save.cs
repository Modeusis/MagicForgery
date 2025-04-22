using System;
using System.Collections.Generic;
using System.IO;
using UI;
using UnityEngine;

namespace Game.Scripts.SaveSystems
{
    [Serializable]
    public class Save
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");
        public Vector3 PlayerPosition { get; private set; }
        public List<string> PlayerItems { get; private set; }
        public int PlayerCustomersServed { get; private set; }

        public Save()
        {
            
        }
        
        public Save(Vector3 playerPos, List<string> playerItems, int playerCustomersServed)
        {
            PlayerPosition = playerPos;
            
            PlayerItems = playerItems;
            
            PlayerCustomersServed = playerCustomersServed;
            
            ExportSaveToFile();
        }

        public static bool TryLoadSaveFromFile(out Save saveData)
        {
            if (!File.Exists(SavePath))
            {
                saveData = null;

                return false;
            }

            try
            {
                string json = File.ReadAllText(SavePath);
                saveData = JsonUtility.FromJson<Save>(json);

                if (saveData == null)
                {
                    return false;
                }
                
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Load failed: {e.Message}");
                saveData = null;

                return false;
            }
        }
        
        public void ExportSaveToFile()
        {
            try
            {
                string json = JsonUtility.ToJson(this, true);
                File.WriteAllText(SavePath, json);
                Debug.Log($"Game saved to: {SavePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Save failed: {e.Message}");
            }
        }
    }
}