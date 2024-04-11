namespace MythosAndHorrors.GameRules
{
    public class GameText
    {
        public string MULLIGAN_EFFECT1 { get; init; }
        public string MULLIGAN_EFFECT2 { get; init; }

        public string CARD01501_BUFF { get; init; }

        /************************* PHASES **********************************/
        public string START_CHAPTER_PHASE_NAME { get; init; }
        public string START_CHAPTER_PHASE_DESCRIPTION { get; init; }
        public string PREPARE_SCENE_PHASE_NAME { get; init; }
        public string PREPARE_SCENE_PHASE_DESCRIPTION { get; init; }
        public string PREPARE_INVESTIGATOR_PHASE_NAME { get; init; }
        public string PREPARE_INVESTIGATOR_PHASE_DESCRIPTION { get; init; }
        public string MULLIGAN_PHASE_NAME { get; init; }
        public string MULLIGAN_PHASE_DESCRIPTION { get; init; }
        public string CREATURE_PHASE_NAME { get; init; }
        public string CREATURE_PHASE_DESCRIPTION { get; init; }
        public string INVESTIGATOR_PHASE_NAME { get; init; }
        public string INVESTIGATOR_PHASE_DESCRIPTION { get; init; }
        public string RESTORE_PHASE_NAME { get; init; }
        public string RESTORE_PHASE_DESCRIPTION { get; init; }
        public string SCENE_PHASE_NAME { get; init; }
        public string SCENE_PHASE_DESCRIPTION { get; init; }



        public string DEFAULT_VOID_TEXT => "<color=red>VOID TEXT </color> ";

    }
}
