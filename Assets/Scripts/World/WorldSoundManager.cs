using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace WinterUniverse
{
    public class WorldSoundManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioSource _ambientSource;
        [SerializeField] private AudioSource _soundSource;

        [SerializeField] private float _ambientFadeSpeed = 0.5f;
        [SerializeField] private List<AudioClip> _ambientClips = new();
        [SerializeField] private List<AudioClip> _soundClips = new();

        [SerializeField] private List<TextureSound> _footstepClips = new();

        private Coroutine _ambientCoroutine;
        private Coroutine _soundCoroutine;

        public void SetMasterVolume(float value)
        {
            _audioMixer.SetFloat("VolumeMaster", value);
        }

        public void SetAmbientVolume(float value)
        {
            _audioMixer.SetFloat("VolumeAmbient", value);
        }

        public void SetSoundVolume(float value)
        {
            _audioMixer.SetFloat("VolumeSound", value);
        }

        public AudioClip ChooseRandomClip(List<AudioClip> clips)
        {
            return clips[Random.Range(0, clips.Count)];
        }

        public void PlaySFX(AudioClip clip, bool randomizePitch = true, float volume = 1f, float minPitch = 0.9f, float maxPitch = 1.1f)
        {
            if (clip == null)
            {
                return;
            }
            _soundSource.volume = volume;
            _soundSource.pitch = randomizePitch ? Random.Range(minPitch, maxPitch) : 1f;
            _soundSource.PlayOneShot(clip);
        }

        public AudioClip GetFootstepClip(Transform ground)
        {
            //if (ground.TryGetComponent(out SurfaceManager sm))
            //{

            //}
            //if (ground.TryGetComponent(out Terrain terrain))
            //{

            //}
            if (ground.TryGetComponent(out MeshRenderer meshRenderer))
            {
                foreach (TextureSound ts in _footstepClips)
                {
                    if (meshRenderer.material.mainTexture == ts.Texture)
                    {
                        return ChooseRandomClip(ts.Clips);
                    }
                }
            }
            return null;
        }

        public void ChangeAmbient()
        {
            ChangeAmbient(_ambientClips);
        }

        public void ChangeAmbient(List<AudioClip> clips)
        {
            if (_ambientCoroutine != null)
            {
                StopCoroutine(_ambientCoroutine);
            }
            _ambientCoroutine = StartCoroutine(PlayAmbientTimer(clips));
        }

        public void ChangeSound()
        {
            ChangeSound(_soundClips);
        }

        public void ChangeSound(List<AudioClip> clips, float minDelay = 10f, float maxDelay = 60f)
        {
            if (_soundCoroutine != null)
            {
                StopCoroutine(_soundCoroutine);
            }
            _soundCoroutine = StartCoroutine(PlaySoundTimer(clips, minDelay, maxDelay));
        }

        private IEnumerator PlayAmbientTimer(List<AudioClip> clips)
        {
            WaitForSeconds delay = new(5f);
            while (true)
            {
                while (_ambientSource.volume != 0f)
                {
                    _ambientSource.volume -= _ambientFadeSpeed * Time.deltaTime;
                    yield return null;
                }
                _ambientSource.volume = 0f;
                _ambientSource.clip = clips[Random.Range(0, clips.Count)];
                _ambientSource.Play();
                while (_ambientSource.volume != 1f)
                {
                    _ambientSource.volume += _ambientFadeSpeed * Time.deltaTime;
                    yield return null;
                }
                _ambientSource.volume = 1f;
                while (_ambientSource.isPlaying)
                {
                    yield return delay;
                }
            }
        }

        private IEnumerator PlaySoundTimer(List<AudioClip> clips, float minDelay, float maxDelay)
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
                PlaySFX(ChooseRandomClip(clips));
            }
        }
    }
}