﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{

    public class SkillIconsController : MonoBehaviour
    {
        private const float Z_OFFSET = -0.0001f;
        [SerializeField, Required, AssetsOnly] private SkillIconView _skillIconPrefab;
        private int totalIcons;

        /*******************************************************************/
        public void SetSkillIconView(int amount, Sprite icon, Sprite holder)
        {
            ClearAll();
            for (int i = 0; i < amount; i++)
            {
                SkillIconView skillIconInstantiate = Instantiate(_skillIconPrefab, transform);
                skillIconInstantiate.SetSkillIcon(icon, holder);
                skillIconInstantiate.transform.localPosition += new Vector3(0, 0, totalIcons++ * Z_OFFSET);
            }
        }

        private void ClearAll()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}