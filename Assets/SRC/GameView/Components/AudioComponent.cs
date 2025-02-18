using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class AudioComponent : SerializedMonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private AudioSource _audioSourceFX;
        [SerializeField, Required, ChildGameObjectsOnly] private AudioSource _audioSourceFXWithStop;
        [SerializeField, Required, ChildGameObjectsOnly] private AudioSource _audioSourceBackground;
        [SerializeField, Required] private Dictionary<string, AudioClip> _audios;
        [SerializeField, Required, AssetsOnly] private List<PlayAnimationSO> _allCardAudios;
        private Dictionary<string, PlayAnimationSO> _dictionaryCardAudios;

        /*******************************************************************/
        private void Awake()
        {
            _dictionaryCardAudios = _allCardAudios.ToDictionary(playAnimation => playAnimation.name);
        }

        /*******************************************************************/
        public AudioClip GetAudioEffect(CardEffect cardEffect)
        {
            _dictionaryCardAudios.TryGetValue(cardEffect.CardOwner.Info.Code, out PlayAnimationSO animation);
            return animation?.GetAudioByCode(cardEffect.Localization.Code);
        }

        public async Task PlayAudioAsync(AudioClip audioClip)
        {
            if (audioClip == null) return;
            _audioSourceBackground.PlayOneShot(audioClip);
            int duration = Mathf.CeilToInt(audioClip.length * 1000);
            await Task.Delay(duration);
        }

        public Tween DOPlayAudio(AudioClip audioclip)
        {
            if (audioclip == null) return null;
            return DOTween.Sequence().OnStart(() => _audioSourceFX.PlayOneShot(audioclip)).AppendInterval(audioclip.length);
        }

        public void PlayAudio(AudioClip audioClip, bool withStop = false, float volume = 1)
        {
            if (audioClip == null) return;
            if (withStop) _audioSourceFXWithStop.PlayOneShot(audioClip, volume);
            else _audioSourceFX.PlayOneShot(audioClip, volume);
        }

        public void StopAudio()
        {
            _audioSourceFXWithStop.Stop();
        }

        public void HideAudio()
        {
            _audioSourceBackground.DOFade(0, ViewValues.DEFAULT_TIME_ANIMATION).OnComplete(ResetAudioSource); ;

            /*******************************************************************/
            void ResetAudioSource()
            {
                _audioSourceBackground.Stop();
                _audioSourceBackground.volume = 1;
            }
        }

        public void PlayIdleCard()
        {
            _audioSourceBackground.loop = true;
            _audioSourceBackground.volume = 0;
            _audioSourceBackground.clip = _audios["Idle"];
            _audioSourceBackground.Play();
            _audioSourceBackground.DOFade(1f, ViewValues.DEFAULT_TIME_ANIMATION);
        }

        public void PlayMoveCardAudio() => PlayAudio(_audios["BasicMove"]);
        public void PlayMoveCardCenterAudio() => PlayAudio(_audios["CenterMove"]);
        public void PlayMoveDeckAudio(bool withDelay) => PlayAudio(withDelay ? _audios["DeckMove"] : _audios["BasicMove"], withStop: true);
    }
}