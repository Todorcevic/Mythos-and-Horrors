using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class CardViewBuilder
    {
        [Inject] private readonly CardBuilder _cardBuilder;
        [Inject] private readonly CardViewGeneratorComponent _cardGenerator;

        /*******************************************************************/
        public CardView BuildRand() => _cardGenerator.BuildCardView(_cardBuilder.BuildRand());

        public CardView BuildWith(CardInfo cardInfo) => _cardGenerator.BuildCardView(_cardBuilder.BuildWith(cardInfo));

        public CardView[] BuildManyRandom(int count)
        {
            CardView[] cards = new CardView[count];
            for (int i = 0; i < count; i++)
            {
                cards[i] = BuildRand();
            }
            return cards;
        }
    }
}
