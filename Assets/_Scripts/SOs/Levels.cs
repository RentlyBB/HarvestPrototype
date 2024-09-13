using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace _Scripts.SOs {
    [CreateAssetMenu(fileName = "Levels", menuName = "Custom/Levels", order = 2)]
    public class Levels : ScriptableObject {
        public List<GridLevelData> levels = new List<GridLevelData>();
    }
}
