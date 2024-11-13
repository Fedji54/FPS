using UnityEngine;

namespace WinterUniverse
{
    public class LocalManager : Singleton<LocalManager>
    {
        private LocalMapManager _mapManager;

        public LocalMapManager MapManager => _mapManager;

        protected override void Awake()
        {
            base.Awake();
            _mapManager = GetComponentInChildren<LocalMapManager>();
        }
    }
}