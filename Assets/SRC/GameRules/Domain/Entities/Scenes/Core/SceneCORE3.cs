﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class SceneCORE3 : Scene
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;

        public CardCreature Drew => _cardsProvider.GetCard<Card01137>();
        public CardCreature Herman => _cardsProvider.GetCard<Card01138>();
        public CardCreature Peter => _cardsProvider.GetCard<Card01139>();
        public CardCreature Victoria => _cardsProvider.GetCard<Card01140>();
        public CardCreature Ruth => _cardsProvider.GetCard<Card01141>();
        public CardCreature MaskedHunter => _cardsProvider.GetCard<Card01121>();
        public CardCreature GhoulPriest => _cardsProvider.GetCard<Card01116>();
        public List<Card> Cultists => new() { Drew, Herman, Peter, Victoria, Ruth, MaskedHunter };
        public CardPlace MainPath => _cardsProvider.GetCard<Card01149>();
        public CardPlace Forest1 => _cardsProvider.GetCard<Card01150>();
        public CardPlace Forest2 => _cardsProvider.GetCard<Card01151>();
        public CardPlace Forest3 => _cardsProvider.GetCard<Card01152>();
        public CardPlace Forest4 => _cardsProvider.GetCard<Card01153>();
        public CardPlace Forest5 => _cardsProvider.GetCard<Card01154>();
        public CardPlace Forest6 => _cardsProvider.GetCard<Card01155>();
        public CardPlace Ritual => _cardsProvider.GetCard<Card01156>();
        public Card01157 Khargath => _cardsProvider.GetCard<Card01157>();
        public Card01117 Lita => _cardsProvider.TryGetCard<Card01117>();
        public IEnumerable<Card> Haunteds => _cardsProvider.GetCards<Card01598>();
        public IEnumerable<Card> Hastur => _cardsProvider.GetCards<Card01175>().Cast<Card>().Concat(_cardsProvider.GetCards<Card01176>());
        public IEnumerable<Card> Yog => _cardsProvider.GetCards<Card01177>().Cast<Card>().Concat(_cardsProvider.GetCards<Card01178>());
        public IEnumerable<Card> Shub => _cardsProvider.GetCards<Card01179>().Cast<Card>().Concat(_cardsProvider.GetCards<Card01180>());
        public IEnumerable<Card> Cthulhu => _cardsProvider.GetCards<Card01181>().Cast<Card>().Concat(_cardsProvider.GetCards<Card01182>());
        public IEnumerable<Card> AllAgents => Hastur.Concat(Yog).Concat(Shub).Concat(Cthulhu);
        public override IEnumerable<Card> StartDeckDangerCards => _chaptersProvider.CurrentChapter.IsRegistered(CORERegister.PriestGhoulLive) ?
            DangerCards.Except(AllAgents.Except(AgentSelected)).Except(Haunteds).Except(Cultists) :
            DangerCards.Except(AllAgents.Except(AgentSelected)).Except(Haunteds).Except(Cultists).Except(new[] { GhoulPriest });

        public List<CardCreature> CultistsNotInterrogate()
        {
            List<CardCreature> creatures = new();
            if (!_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.DrewInterrogate)) creatures.Add(Drew);
            if (!_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.HermanInterrogate)) creatures.Add(Herman);
            if (!_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.PeterInterrogate)) creatures.Add(Peter);
            if (!_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.RuthInterrogate)) creatures.Add(Ruth);
            if (!_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.VictoriaInterrogate)) creatures.Add(Victoria);
            if (!_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.MaskedHunterInterrogate)) creatures.Add(MaskedHunter);
            return creatures;
        }

        public int AmountInterrogate => 6 - CultistsNotInterrogate().Count;

        public List<CardPlace> Forests => new() { Forest1, Forest2, Forest3, Forest4, Forest5, Forest6 };

        private List<CardPlace> _forestsToPlace;
        public List<CardPlace> ForestsToPlace => _forestsToPlace ??= Forests.Rand(4).ToList();

        private IEnumerable<Card> _agentSelected;
        public IEnumerable<Card> AgentSelected => _agentSelected ??= new List<IEnumerable<Card>> { Hastur, Yog, Shub, Cthulhu }.Rand().ToList();

        /*******************************************************************/
        public override void Init()
        {
            base.Init();
            ThrowLita();
        }

        private void ThrowLita()
        {
            if (Lita == null) return;
            Khargath.CreateActivation(1, ThrowLitaActivate, ThrowLitaConditionToActivate,
                   PlayActionType.Activate, new Localization("Activation_Card01157", Lita.CurrentName, Khargath.CurrentName), cardAffected: Lita);

            Lita.CreateActivation(1, ThrowLitaActivate, ThrowLitaConditionToActivate, PlayActionType.Activate, new Localization("Activation_Card01157", Lita.CurrentName, Khargath.CurrentName), cardAffected: Khargath);

            bool ThrowLitaConditionToActivate(Investigator investigator)
            {
                if (Khargath.IsInPlay.IsFalse) return false;
                if (Lita?.IsInPlay.IsFalse ?? true) return false;
                if (Lita?.CurrentPlace != Khargath.CurrentPlace) return false;
                if (Khargath.CurrentPlace != investigator.CurrentPlace) return false;
                return true;
            }

            async Task ThrowLitaActivate(Investigator investigator)
            {
                await _gameActionsProvider.Create<FinalizeGameAction>().SetWith(_chaptersProvider.CurrentScene.FullResolutions[3]).Execute();
            }
        }

        public override async Task PrepareScene()
        {
            await ShowHistory();
            await PlacePlaces();
            await PlaceDangerDeck();
            await PlacePlotAndGoal();
            await PlaceInvestigators();
            await CheckDiscard();
        }

        /*******************************************************************/
        private async Task ShowHistory()
        {
            await _gameActionsProvider.Create<ShowHistoryGameAction>().SetWith(Descriptions[0]).Execute();
        }

        private async Task PlacePlaces()
        {
            Dictionary<Card, Zone> allPlaces = new()
            {
                { MainPath, GetPlaceZone(1, 3) },
                { ForestsToPlace[0], GetPlaceZone(0, 2) },
                { ForestsToPlace[1], GetPlaceZone(0, 4) },
                { ForestsToPlace[2], GetPlaceZone(2, 2) },
                { ForestsToPlace[3], GetPlaceZone(2, 4) },
            };

            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(allPlaces).Execute();
        }

        private async Task PlaceDangerDeck()
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(StartDeckDangerCards, DangerDeckZone, isFaceDown: true).Execute();
            await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(DangerDeckZone).Execute();
        }

        private async Task PlacePlotAndGoal()
        {
            await _gameActionsProvider.Create<PlacePlotGameAction>().SetWith(FirstPlot).Execute();
            await _gameActionsProvider.Create<PlaceGoalGameAction>().SetWith(FirstGoal).Execute();

            int totaL = 0;
            if (AmountInterrogate < 2) totaL = 3;
            else if (AmountInterrogate < 4) totaL = 2;
            else if (AmountInterrogate < 6) totaL = 1;
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(FirstPlot.Eldritch, totaL).Execute();
        }

        private async Task CheckDiscard()
        {
            if (_chaptersProvider.CurrentChapter.IsRegistered(CORERegister.IsMidknigh))
                await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(InvestigatorsWithCards, Discard).Execute();

            IEnumerable<Investigator> InvestigatorsWithCards() => _investigatorsProvider.AllInvestigatorsInPlay
                .Where(investigator => investigator.HandZone.Cards.Any());

            async Task Discard(Investigator investigator)
            {
                Card cardToDiscard = investigator.HandZone.Cards.Rand();
                if (cardToDiscard == null) return;
                await _gameActionsProvider.Create<DiscardGameAction>().SetWith(cardToDiscard).Execute();
                cardToDiscard = investigator.HandZone.Cards.Rand();
                if (cardToDiscard == null) return;
                await _gameActionsProvider.Create<DiscardGameAction>().SetWith(cardToDiscard).Execute();
            }
        }

        private async Task PlaceInvestigators()
        {
            await _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(_investigatorsProvider.AllInvestigators, MainPath).Execute();
        }

        /*******************************************************************/
        protected override async Task Resolution0()
        {
            await _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.UmordhothWin, true).Execute();
            await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(AllInvestigatorsInPlay, Defeated).Execute();

            /*******************************************************************/
            IEnumerable<Investigator> AllInvestigatorsInPlay() => _investigatorsProvider.AllInvestigatorsInPlay;
            async Task Defeated(Investigator investigator) =>
                await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(investigator.Defeated, true).Execute();

        }

        protected override async Task Resolution1()
        {
            await _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.UmordhothWin, false).Execute();
            await TakeShock();
            await _gameActionsProvider.Create<GainSceneXpGameAction>().Execute();

            async Task TakeShock()
            {
                Dictionary<Stat, int> statsWithValues = new();
                foreach (var investigator in _investigatorsProvider.AllInvestigatorsInPlay)
                {
                    statsWithValues.Add(investigator.Shock, 2);
                }
                await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(statsWithValues).Execute();
            }
        }

        protected override async Task Resolution2()
        {
            await _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.UmordhothWin, false).Execute();
            await TakeInjuriesAndShock();
            await _gameActionsProvider.Create<GainSceneXpGameAction>().Execute();

            async Task TakeInjuriesAndShock()
            {
                Dictionary<Stat, int> statsWithValues = new();
                foreach (var investigator in _investigatorsProvider.AllInvestigatorsInPlay)
                {
                    statsWithValues.Add(investigator.Injury, 2);
                    statsWithValues.Add(investigator.Shock, 2);
                }
                await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(statsWithValues).Execute();
            }
        }

        protected override async Task Resolution3()
        {
            await _gameActionsProvider.Create<RegisterChapterGameAction>().SetWith(CORERegister.LitaSacrifice, true).Execute();
            await TakeInjuriesAndShock();
            await TakeFlaw();
            await _gameActionsProvider.Create<GainSceneXpGameAction>().Execute();

            async Task TakeInjuriesAndShock()
            {
                Dictionary<Stat, int> statsWithValues = new();
                foreach (var investigator in _investigatorsProvider.AllInvestigatorsInPlay)
                {
                    statsWithValues.Add(investigator.Injury, 2);
                    statsWithValues.Add(investigator.Shock, 2);
                }
                await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(statsWithValues).Execute();
            }

            async Task TakeFlaw()
            {
                await Task.CompletedTask; //TODO: Implement this method Una carta IFlaw a diseñar (LitaSacrifice) para todos los investigadores
            }
        }

        /*******************************************************************/
        public override ChallengeToken GetNewCreatureToken() => new(ChallengeTokenType.Creature, value: CreatureValue, effect: CreatureEffect, description: (_) => CreatureTokenDescriptionNormal);
        public override ChallengeToken GetNewDangerToken() => new(ChallengeTokenType.Danger, value: DangerValue, effect: DangerEffect, description: (_) => DangerTokenDescriptionNormal);
        public override ChallengeToken GetNewCultistToken() => new(ChallengeTokenType.Cultist, value: CultistValue, effect: CultistEffect, description: (_) => CultistTokenDescriptionNormal);
        public override ChallengeToken GetNewAncientToken() => new(ChallengeTokenType.Ancient, value: AncientValue, effect: AncientEffect, description: (_) => AncientTokenDescriptionNormal);

        private int CreatureValue(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                return CreatureNormalValue();
            else return CreatureHardValue();

            /*******************************************************************/
            int CreatureNormalValue() => _cardsProvider.GetCards<CardCreature>()
                .Where(creature => creature.IsInPlay.IsTrue && creature.HasThisTag(Tag.Monster)).Count() * -1
;

            int CreatureHardValue() => -3;
        }

        private async Task CreatureEffect(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                await CreatureNormalEffect();
            else await CreatureHardEffect();

            /*******************************************************************/
            async Task CreatureNormalEffect() => await Task.CompletedTask;
            async Task CreatureHardEffect()
            {
                Reaction<ChallengePhaseGameAction> drawGhouReaction = null;
                drawGhouReaction = _reactionablesProvider.CreateReaction<ChallengePhaseGameAction>(condition: DrawMonsterCondition, logic: DrawMonster, GameActionTime.After);
                await Task.CompletedTask;

                /*******************************************************************/
                async Task DrawMonster(ChallengePhaseGameAction challengePhaseGameAction)
                {
                    Card monster = DangerDeckZone.Cards.Concat(DangerDiscardZone.Cards).FirstOrDefault(card => card.Tags.Contains(Tag.Monster));

                    await _gameActionsProvider.Create<DrawGameAction>().SetWith(_gameActionsProvider.CurrentChallenge.ActiveInvestigator, monster).Execute();
                    await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(DangerDeckZone).Execute();
                }

                bool DrawMonsterCondition(ChallengePhaseGameAction challengePhaseGameAction)
                {
                    _reactionablesProvider.RemoveReaction(drawGhouReaction);
                    if (challengePhaseGameAction.IsSucceed) return false;
                    return true;
                }
            }
        }

        private int CultistValue(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                return CultistNormalValue();
            else return CultistHardValue();

            /*******************************************************************/
            int CultistNormalValue() => -2;
            int CultistHardValue() => -4;
        }

        private async Task CultistEffect(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                await CultistNormalEffect();
            else await CultistHardEffect();

            /*******************************************************************/
            async Task CultistNormalEffect()
            {
                IEldritchable nearestCreature = _gameActionsProvider.CurrentChallenge.ActiveInvestigator.NearestCreatures
                    .OfType<IEldritchable>().FirstOrDefault();
                if (nearestCreature != null) await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(nearestCreature.Eldritch, 1).Execute();
            }

            async Task CultistHardEffect()
            {
                IEldritchable nearestCreature = _gameActionsProvider.CurrentChallenge.ActiveInvestigator.NearestCreatures
                  .OfType<IEldritchable>().FirstOrDefault();
                if (nearestCreature != null) await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(nearestCreature.Eldritch, 2).Execute();
            }
        }

        private int DangerValue(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                return DangerNormalValue();
            else return DangerHardValue();

            /*******************************************************************/
            int DangerNormalValue() => -3;
            int DangerHardValue() => -5;
        }

        private async Task DangerEffect(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                await DangerNormalEffect();
            else await DangerHardEffect();

            /*******************************************************************/
            async Task DangerNormalEffect()
            {
                if (!_gameActionsProvider.CurrentChallenge.ActiveInvestigator.CreaturesInSamePlace.Any()) return;
                await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(_gameActionsProvider.CurrentChallenge.ActiveInvestigator, _gameActionsProvider.CurrentChallenge.CardToChallenge, amountDamage: 1).Execute();
            }

            async Task DangerHardEffect()
            {
                if (!_gameActionsProvider.CurrentChallenge.ActiveInvestigator.CreaturesInSamePlace.Any()) return;
                await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(_gameActionsProvider.CurrentChallenge.ActiveInvestigator, _gameActionsProvider.CurrentChallenge.CardToChallenge, amountDamage: 1, amountFear: 1).Execute();
            }
        }

        private int AncientValue(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                return AncientNormalValue();
            else return AncientHardValue();

            /*******************************************************************/
            int AncientNormalValue() => -5;

            int AncientHardValue() => -7;
        }

        private Task AncientEffect(Investigator investigator)
        {
            if (_chaptersProvider.CurrentDificulty == Dificulty.Easy || _chaptersProvider.CurrentDificulty == Dificulty.Normal)
                return AncientNormalEffect();
            else return AncientHardEffect();

            /*******************************************************************/
            async Task AncientNormalEffect()
            {
                if (!_cardsProvider.GetCards<CardCreature>()
                    .Where(creature => creature.IsInPlay.IsTrue && creature.HasThisTag(Tag.AncientOne)).Any()) return;
                await _gameActionsProvider.Create<RevealRandomChallengeTokenGameAction>().SetWith(investigator).Execute();
            }

            async Task AncientHardEffect()
            {
                if (!_cardsProvider.GetCards<CardCreature>()
                   .Where(creature => creature.IsInPlay.IsTrue && creature.HasThisTag(Tag.AncientOne)).Any()) return;
                await _gameActionsProvider.Create<RevealRandomChallengeTokenGameAction>().SetWith(investigator).Execute();
            }
        }
    }
}
