﻿using MythosAndHorrors.GameRules;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class InvestigatorLoaderUseCase
    {
        [Inject] private readonly FilesPath _filesPath;
        [Inject] private readonly DiContainer _diContainer;
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly DataSaveLoaderUseCase _saveDataLoaderUseCase;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;

        /*******************************************************************/
        public void Execute()
        {
            foreach (string investigatorCode in _saveDataLoaderUseCase.DataSave.InvestigatorsSelected)
            {
                Investigator newInvestigator = _jsonService.CreateDataFromFile<Investigator>(_filesPath.JSON_INVESTIGATOR_PATH(investigatorCode));

                _diContainer.Inject(newInvestigator);
                _investigatorsProvider.AddInvestigator(newInvestigator);
                _avatarViewsManager.GetVoid().Init(newInvestigator);
            }
        }
    }
}