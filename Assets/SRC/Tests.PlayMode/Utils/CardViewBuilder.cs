using Zenject;

namespace MythsAndHorrors.PlayMode.Tests
{
    public class CardViewBuilder
    {
        [Inject] private readonly CardBuilder _cardBuilder;
        [Inject] private readonly CardViewGeneratorComponent _cardGenerator;

        /*******************************************************************/
        public CardView BuildOne() => _cardGenerator.BuildCard(_cardBuilder.SingleCard);

        public CardView[] BuildManySame(int count)
        {
            CardView[] cards = new CardView[count];
            for (int i = 0; i < count; i++)
            {
                cards[i] = BuildOne();
            }
            return cards;
        }

        public CardView BuildRand() => _cardGenerator.BuildCard(_cardBuilder.BuildRand());

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
