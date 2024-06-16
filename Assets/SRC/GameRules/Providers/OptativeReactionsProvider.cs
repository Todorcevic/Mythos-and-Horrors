using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class OptativeReactionsProvider
    {
        [Inject] private readonly DiContainer _diContainer;

        private readonly List<IReaction> _optativeReactions = new();

        List<IReaction> Before => _optativeReactions.Where(realReaction => realReaction.Time == GameActionTime.Before).ToList();
        List<IReaction> After => _optativeReactions.Where(realReaction => realReaction.Time == GameActionTime.After).ToList();

        /*******************************************************************/
        public async Task WhenBegin(GameAction gameAction)
        {
            foreach (IReaction reaction in Before)
                await reaction.React(gameAction);
        }

        public async Task WhenFinish(GameAction gameAction)
        {
            foreach (IReaction reaction in After)
                await reaction.React(gameAction);
        }

        /*******************************************************************/
        public void CreateReaction(IReaction realReaction)
        {
            _diContainer.Inject(realReaction);
            _optativeReactions.Add(realReaction);
        }
    }
}
