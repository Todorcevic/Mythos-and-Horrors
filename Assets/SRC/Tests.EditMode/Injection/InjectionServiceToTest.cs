using MythosAndHorrors.GameRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zenject;

namespace MythosAndHorrors.EditMode.Tests
{
    public class InjectionServiceToTest : Installer
    {
        public override void InstallBindings()
        {
            //Container.Bind<PrepareGameToTest>().AsSingle();
            BindAllFakePresenters();
        }

        private void BindAllFakePresenters()
        {
            IEnumerable<Type> gameActionTypes = typeof(GameAction).Assembly.GetTypes()
                .Where(type => type.IsClass && type.BaseType == typeof(GameAction));

            foreach (Type type in gameActionTypes)
            {
                BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
                FieldInfo[] campos = type.GetFields(flags);

                foreach (FieldInfo campo in campos)
                {
                    if (campo.FieldType.IsGenericType &&
                        campo.FieldType.GetGenericTypeDefinition() == typeof(IPresenter<>) && campo.FieldType.GetGenericArguments()[0] == type)
                    {
                        Type genericToBind = typeof(FakeMoveCardsGamePresenter<>).MakeGenericType(type);
                        Container.Unbind(campo.FieldType);
                        Container.Bind(campo.FieldType).To(genericToBind).AsCached();
                    }
                }
            }
        }
    }
}