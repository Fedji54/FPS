using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WinterUniverse
{
    public class WorldMapManager : MonoBehaviour
    {
        [SerializeField] private List<MapConfig> _maps = new();

        private MapConfig _oldMap;
        private MapConfig _newMap;

        public void LoadMap(MapConfig oldMap, MapConfig newMap)
        {
            _oldMap = oldMap;
            _newMap = newMap;
            StartCoroutine(LoadMap());
        }

        public void LoadMap(string newMap)
        {
            foreach (MapConfig map in _maps)
            {
                if (map.DisplayName == newMap)
                {
                    _newMap = map;
                    break;
                }
            }
            SceneManager.LoadScene(_newMap.DisplayName);
            if (_newMap.AmbientClips.Count > 0)
            {
                WorldManager.StaticInstance.SoundManager.ChangeAmbient(_newMap.AmbientClips);
            }
            else
            {
                WorldManager.StaticInstance.SoundManager.ChangeAmbient();
            }
            if (_newMap.SoundClips.Count > 0)
            {
                WorldManager.StaticInstance.SoundManager.ChangeSound(_newMap.SoundClips, _newMap.MinSoundDelay, _newMap.MaxSoundDelay);
            }
            else
            {
                WorldManager.StaticInstance.SoundManager.ChangeSound();
            }
        }

        private IEnumerator LoadMap()
        {
            //WorldManager.StaticInstance.PlayerManager.DisableAll();
            SceneManager.LoadScene(_newMap.DisplayName);
            yield return new WaitForSeconds(1f);
            if (_newMap.AmbientClips.Count > 0)
            {
                WorldManager.StaticInstance.SoundManager.ChangeAmbient(_newMap.AmbientClips);
            }
            else
            {
                WorldManager.StaticInstance.SoundManager.ChangeAmbient();
            }
            if (_newMap.SoundClips.Count > 0)
            {
                WorldManager.StaticInstance.SoundManager.ChangeSound(_newMap.SoundClips, _newMap.MinSoundDelay, _newMap.MaxSoundDelay);
            }
            else
            {
                WorldManager.StaticInstance.SoundManager.ChangeSound();
            }
            yield return new WaitForSeconds(1f);
            WorldManager.StaticInstance.PlayerManager.transform.SetPositionAndRotation(
                LocalManager.StaticInstance.MapManager.GetEntryPoint(_oldMap).position,
                LocalManager.StaticInstance.MapManager.GetEntryPoint(_oldMap).rotation);
            yield return new WaitForSeconds(1f);
            //WorldManager.StaticInstance.PlayerManager.EnableAll();
        }
    }
}