using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Map", menuName = "Winter Universe/World/Map/New Map")]
    public class MapConfig : ScriptableObject
    {
        [SerializeField] private string _displayName = "Name";
        [SerializeField, TextArea] private string _description = "Description";
        [SerializeField] private List<AudioClip> _ambientClips = new();
        [SerializeField] private List<AudioClip> _soundClips = new();
        [SerializeField] private float _minSoundDelay = 10f;
        [SerializeField] private float _maxSoundDelay = 60f;

        public string DisplayName => _displayName;
        public string Description => _description;
        public List<AudioClip> AmbientClips => _ambientClips;
        public List<AudioClip> SoundClips => _soundClips;
        public float MinSoundDelay => _minSoundDelay;
        public float MaxSoundDelay => _maxSoundDelay;
    }
}