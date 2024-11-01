using UnityEngine;
using UnitySingleton;

namespace _Scripts.GridCore {
    [RequireComponent(typeof(GridInitializer))]
    public class GridManager : PersistentMonoSingleton<GridManager> {

        public GridInitializer gridInitializer;

        protected override void Awake() {
            base.Awake();
            gridInitializer = GetComponent<GridInitializer>();
        }

    }
}