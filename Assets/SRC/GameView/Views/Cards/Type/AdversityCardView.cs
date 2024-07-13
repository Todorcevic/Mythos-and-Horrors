//using MythosAndHorrors.GameRules;
//using Sirenix.OdinInspector;
//using UnityEngine;

//namespace MythosAndHorrors.GameView
//{
//    public class AdversityCardView : CardView
//    {
//        [Title(nameof(AdversityCardView))]
//        [SerializeField, Required, ChildGameObjectsOnly] private StatView _extraStat;

//        /*******************************************************************/
//        protected override void SetSpecific()
//        {
//            SetStats();
//        }

//        private void SetStats()
//        {
//            if (Card.ExtraStat != null)
//            {
//                _extraStat.gameObject.SetActive(true);
//                _extraStat.SetStat(Card.ExtraStat);
//            }
//        }
//    }
//}
