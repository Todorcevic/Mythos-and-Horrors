using Sirenix.OdinInspector;
using System.Threading.Tasks;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class AudioComponent : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private AudioSource _audioSource;
        [SerializeField, Required, AssetsOnly] private AudioClip _moveCardAudio;
        [SerializeField, Required, AssetsOnly] private AudioClip _moveCardCenterAudio;
        [SerializeField, Required, AssetsOnly] private AudioClip _moveDeckAudio;

        /*******************************************************************/
        public async Task PlayAudioAsync(AudioClip audioClip)
        {
            if (audioClip == null) return;
            _audioSource.PlayOneShot(audioClip);
            int duration = Mathf.CeilToInt(audioClip.length * 1000);
            await Task.Delay(duration);
        }

        public void PlayAudio(AudioClip audioClip)
        {
            if (audioClip == null) return;
            _audioSource.PlayOneShot(audioClip);
        }

        public void StopAudio()
        {
            _audioSource.Stop();
        }

        public void PlayMoveCardAudio() => PlayAudio(_moveCardAudio);
        public void PlayMoveCardCenterAudio() => PlayAudio(_moveCardCenterAudio);
        public void PlayMoveDeckAudio() => PlayAudio(_moveDeckAudio);
    }
}