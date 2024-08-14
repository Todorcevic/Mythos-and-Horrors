using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CreatureConfrontAttackGameAction : PhaseGameAction
    {
        [Inject] private readonly CardsProvider _cardsProvider;

        /*******************************************************************/
        public override string Name => _textsProvider.GetLocalizableText("PhaseName_CreatureConfrontAttack");
        public override string Description => _textsProvider.GetLocalizableText("PhaseDescription_CreatureConfrontAttack");
        public override Phase MainPhase => Phase.Creature;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<SafeForeach<CardCreature>>().SetWith(AttackerCreatures, Attack).Execute();

            /*******************************************************************/
            IEnumerable<CardCreature> AttackerCreatures() => _cardsProvider.AttackerCreatures;

            async Task Attack(CardCreature cardCreature)
            {
                if (cardCreature is CardColosus colosus)
                    await _gameActionsProvider.Create<ColosusAttackGameAction>().SetWith(colosus).Execute();
                else await _gameActionsProvider.Create<CreatureAttackGameAction>().SetWith(cardCreature, cardCreature.ConfrontedInvestigator).Execute();
                await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(cardCreature.Exausted, true).Execute();
            }
        }
    }
}
