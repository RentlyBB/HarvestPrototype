using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.GridCore {
    [CreateAssetMenu(fileName = "LevelData", menuName = "Custom/LevelData", order = 0)]
    public class LevelData : ScriptableObject {
        public List<GridObject> tiles = new List<GridObject>();
    }
}