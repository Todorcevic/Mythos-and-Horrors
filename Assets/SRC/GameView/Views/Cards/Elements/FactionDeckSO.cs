﻿using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    [CreateAssetMenu(fileName = "Faction", menuName = "ScriptableObjects/DeckElements")]
    public class FactionDeckSO : ScriptableObject
    {
        [SerializeField, Required] public Faction _faction;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _templateDeckFront;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _badget;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _titleHolder;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _skillHolder;
    }
}