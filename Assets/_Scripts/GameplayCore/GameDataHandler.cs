using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VHierarchy.Libs;
using VInspector;

namespace _Scripts.GameplayCore {
    public class GameDataHandler : MonoBehaviour {
        
        private const string FileNameGameData = "GameData.json";

        [Button]
        public void CreateNewWorld() {
            var loadGameData = LoadGameData() ?? new GameData();

            var world = new World();

            world.worldID = loadGameData.worlds.Count;
            world.isUnlocked = false;
            
            loadGameData.worlds.Add(world);

            var json = JsonUtility.ToJson(loadGameData, true);
            
            var filePathGameData = Path.Combine(Application.persistentDataPath, FileNameGameData);
            
            File.WriteAllText(filePathGameData, json);

            Debug.Log("World Created");
        }
        
        [Button]
        public void RemoveLastWorld() {
            
            var loadWorlds = LoadGameData() ?? new GameData();

            loadWorlds.worlds.RemoveLast();
            var json = JsonUtility.ToJson(loadWorlds, true);
            
            var filePathGameData = Path.Combine(Application.persistentDataPath, FileNameGameData);
            File.WriteAllText(filePathGameData, json);

            Debug.Log("World Removed");
        }
        

        private GameData LoadGameData() {
            var loadedGameData = new GameData();
            
            var filePathGameData = Path.Combine(Application.persistentDataPath, FileNameGameData);
            
            if (File.Exists(filePathGameData)) {
                var jsonString = File.ReadAllText(filePathGameData);
                JsonUtility.FromJsonOverwrite(jsonString, loadedGameData);
                // Or if you want a new instance, you can use a different approach:
                // var loadedData = JsonUtility.FromJson<LevelData>(jsonString);
            }

            return loadedGameData;
        }

        
        // [Button]
        // public void ClearWorld() {
        //     string filePath = Path.Combine(Application.persistentDataPath, "levelData.json");
        //
        //     // Overwrite with an empty JSON object: "{}"
        //     // or an empty array: "[]"
        //     File.WriteAllText(filePath, "{}");
        // }
    }

    [Serializable]
    public class GameData {
        public List<World> worlds = new List<World>();
    }
    
    [Serializable]
    public class World {
        public int worldID;
        public bool isUnlocked = false;
        public WorldCollectibles collectibles = new WorldCollectibles();
    }

    [Serializable]
    public class WorldCollectibles {
        public int collectToUnlock = 0;
        public int maxCollectibleCount = 0;
        public int collectibleCollected = 0;
    }

   
}