using UnityEngine;

namespace WinterUniverse
{
    public class WorldManager : Singleton<WorldManager>
    {
        private WorldDataManager _dataManager;
        private WorldInputManager _inputManager;
        private WorldLayerManager _layerManager;
        private WorldMapManager _mapManager;
        private PlayerController _playerManager;
        private WorldSaveLoadManager _saveLoadManager;
        private WorldSoundManager _soundManager;
        private WorldTimeManager _timeManager;

        public WorldDataManager DataManager => _dataManager;
        public WorldInputManager InputManager => _inputManager;
        public WorldLayerManager LayerManager => _layerManager;
        public WorldMapManager MapManager => _mapManager;
        public PlayerController PlayerManager => _playerManager;
        public WorldSaveLoadManager SaveLoadManager => _saveLoadManager;
        public WorldSoundManager SoundManager => _soundManager;
        public WorldTimeManager TimeManager => _timeManager;

        protected override void Awake()
        {
            base.Awake();
            _dataManager = GetComponentInChildren<WorldDataManager>();
            _inputManager = GetComponentInChildren<WorldInputManager>();
            _layerManager = GetComponentInChildren<WorldLayerManager>();
            _mapManager = GetComponentInChildren<WorldMapManager>();
            _playerManager = GetComponentInChildren<PlayerController>();
            _saveLoadManager = GetComponentInChildren<WorldSaveLoadManager>();
            _soundManager = GetComponentInChildren<WorldSoundManager>();
            _timeManager = GetComponentInChildren<WorldTimeManager>();
            _timeManager.Initialize();
            _saveLoadManager.LoadGame();
        }
    }
}