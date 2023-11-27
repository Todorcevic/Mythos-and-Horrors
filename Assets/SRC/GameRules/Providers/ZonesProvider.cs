namespace MythsAndHorrors.GameRules
{
    public class ZonesProvider
    {
        public Zone DangerDeckZone { get; } = new Zone();
        public Zone DangerDiscardZone { get; } = new Zone();
        public Zone GoalZone { get; } = new Zone();
        public Zone PlotZone { get; } = new Zone();
        public Zone VictoryZone { get; } = new Zone();
        public Zone LimboZone { get; } = new Zone();
        public Zone OutZone { get; } = new Zone();
        public Zone SelectorZone { get; } = new Zone();
        public Zone[,] PlaceZone { get; } = new Zone[3, 7];

        /*******************************************************************/
        private ZonesProvider()
        {
            InitializePlaceZones();
        }

        /*******************************************************************/
        public void InitializePlaceZones()
        {
            for (int i = 0; i < PlaceZone.GetLength(0); i++)
            {
                for (int j = 0; j < PlaceZone.GetLength(1); j++)
                {
                    PlaceZone[i, j] = new Zone();
                }
            }
        }
    }
}
