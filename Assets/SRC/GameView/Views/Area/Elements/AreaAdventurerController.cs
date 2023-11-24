using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class AreaAdventurerController : MonoBehaviour
    {
        [Inject] private readonly AreaViewsManager _areaViewsManager;
        [SerializeField, Required, SceneObjectsOnly] private List<AreaAdventurerView> _allAreas;
        [SerializeField, Required, SceneObjectsOnly] private Transform _cameraPosition;
        [SerializeField, Required, SceneObjectsOnly] private Transform _rightPosition;
        [SerializeField, Required, SceneObjectsOnly] private Transform _leftPosition;

        /*******************************************************************/

        public void Move()
        {
            //var adventurerArea = _areaViewsManager.Get();
            //var adventurerAreaPosition = adventurerArea.transform.position;
            //var cameraPosition = _cameraPosition.position;
            //var rightPosition = _rightPosition.position;
            //var leftPosition = _leftPosition.position;

            //if (adventurerAreaPosition.x < cameraPosition.x)
            //{
            //    adventurerArea.transform.position = new Vector3(rightPosition.x, adventurerAreaPosition.y, adventurerAreaPosition.z);
            //}
            //else if (adventurerAreaPosition.x > cameraPosition.x)
            //{
            //    adventurerArea.transform.position = new Vector3(leftPosition.x, adventurerAreaPosition.y, adventurerAreaPosition.z);
            //}
        }
    }
}
