using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace _Scripts.SOs {
    [CreateAssetMenu(fileName = "Levels", menuName = "Custom/Levels", order = 2)]
    public class Levels_old : ScriptableObject {
        public List<GridLevelData_old> levels = new List<GridLevelData_old>();
    }
}
