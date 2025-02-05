using System;
using System.Collections.Generic;
using System.IO;
using _Scripts.UnitySingleton;
using UnityEngine;
using VHierarchy.Libs;
using VInspector;

namespace _Scripts.GameplayCore {
    public class GameDataHandler : PersistentMonoSingleton<GameDataHandler> {
        
        private const string FileNameGameData = "GameData.json";

        [Button]
        public void CreateNewWorld() {
            var loadGameData = LoadGameData() ?? new GameData();

            var world = new World();

            world.worldID = loadGameData.worlds.Count;
            world.isUnlocked = world.worldID == 0;

            loadGameData.worlds.Add(world);

            var json = JsonUtility.ToJson(loadGameData, true);
            var filePathGameData = Path.Combine(Application.persistentDataPath, FileNameGameData);
            File.WriteAllText(filePathGameData, json);

            Debug.Log("World Created");
        }

        [Button]
        public void AddLevelToWorld(int worldID) {
            
            var loadGameData = LoadGameData() ?? new GameData();
            
            // Check if world exists
            if(worldID > (loadGameData.worlds.Count - 1)) return;
            
            var level = new Level();
            level.levelID = loadGameData.worlds[worldID].levels.Count;
            level.isUnlocked = worldID == 0 && level.levelID == 0;
            level.levelType = level.levelID % 2 == 0 ? LevelType.Default : LevelType.Special;
            
            loadGameData.worlds[worldID].levels.Add(level);
            
            var json = JsonUtility.ToJson(loadGameData, true);
            var filePathGameData = Path.Combine(Application.persistentDataPath, FileNameGameData);
            File.WriteAllText(filePathGameData, json);

            Debug.Log("Level Created");
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
        

        public GameData LoadGameData() {
            var loadedGameData = new GameData();
            
            var filePathGameData = Path.Combine(Application.persistentDataPath, FileNameGameData);
            
            if (File.Exists(filePathGameData)) {
                var jsonString = File.ReadAllText(filePathGameData);
                JsonUtility.FromJsonOverwrite(jsonString, loadedGameData);
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
        public List<Level> levels = new List<Level>();
    }

    [Serializable]
    public class Level {
        public int levelID;
        public bool isUnlocked = false;
        public LevelType levelType;
    }

    [Serializable]
    public enum LevelType {
        Default,
        Special
    }

}