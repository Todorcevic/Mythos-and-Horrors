using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public abstract class Scene
    {
        [Inject] public SceneInfo Info { get; }

        public abstract Task PrepareScene();
    }
}
