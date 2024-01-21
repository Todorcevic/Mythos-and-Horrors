#if !NOT_UNITY3D

using UnityEngine;
using UnityEngine.Serialization;

namespace Zenject
{
    public class ZenjectBinding : MonoBehaviour
    {
        public enum BindId
        {
            None,
            SelectorZone,
            OutZone,
            CenterShowZone,
        }

        public enum BindTypes
        {
            Self,
            AllInterfaces,
            AllInterfacesAndSelf,
            BaseType
        }

        [Tooltip("The component to add to the Zenject container")]
        [SerializeField]
        Component[] _components = null;

        [Tooltip("Note: Autocomplete identifier with BindId enum (BindId is in this class)")]
        [SerializeField]
        BindId _bindId = BindId.None;

        [Tooltip("When set, this will bind the given components to the SceneContext.  It can be used as a shortcut to explicitly dragging the SceneContext into the Context field.  This is useful when using ZenjectBinding inside GameObjectContext.  If your ZenjectBinding is for a component that is not underneath GameObjectContext then it is not necessary to check this")]
        [SerializeField]
        bool _useSceneContext = false;

        [Tooltip("Note: This value is optional and can be ignored in most cases.  This value will determine what container the component gets added to.  If unset, the component will be bound on the most 'local' context.  In most cases this will be the SceneContext, unless this component is underneath a GameObjectContext, or ProjectContext, in which case it will bind to that instead by default.  You can also override this default by providing the Context directly.  This can be useful if you want to bind something that is inside a GameObjectContext to the SceneContext container.")]
        [SerializeField]
        [FormerlySerializedAs("_compositionRoot")]
        Context _context = null;

        [Tooltip("This value is used to determine how to bind this component.  When set to 'Self' is equivalent to calling Container.FromInstance inside an installer. When set to 'AllInterfaces' this is equivalent to calling 'Container.BindInterfaces<MyMonoBehaviour>().ToInstance', and similarly for InterfacesAndSelf")]
        [SerializeField]
        BindTypes _bindType = BindTypes.Self;

        public bool UseSceneContext
        {
            get { return _useSceneContext; }
        }

        public Context Context
        {
            get { return _context; }
            set { _context = value; }
        }

        public Component[] Components
        {
            get { return _components; }
        }

        public object Identifier
        {
            get { return _bindId == BindId.None ? null : _bindId; }
        }

        public BindTypes BindType
        {
            get { return _bindType; }
        }

        /*******************************************************************/
        public void Start()
        {
            // Define this method so we expose the enabled check box
        }
    }
}

#endif
