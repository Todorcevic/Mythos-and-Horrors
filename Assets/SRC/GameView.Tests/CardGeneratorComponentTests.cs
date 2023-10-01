//using System.Collections;
//using NUnit.Framework;
//using UnityEngine.TestTools;
//using Zenject;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//namespace GameView.Tests
//{
//    [TestFixture]
//    public class CardGeneratorComponentTests : SceneTestFixture
//    {
//        //[Inject] private readonly CardGeneratorComponent sut;

//        //[SetUp]
//        //public override void SetUp()
//        //{
//        //    base.SetUp();
//        //}

//        //[TearDown]
//        //public override void Teardown()
//        //{

//        //    base.SetUp();
//        //}

//        [UnityTest]
//        public IEnumerator CardGeneratorComponent_BuildCards()
//        {
//            //List<Card> cards = new()
//            //{
//            //    (Card)SceneContainer.Instantiate(typeof(Card00001), new object[] { new CardInfo() { Description = "Hola Mondo", Cost = 4, CardType = CardType.Adventurer, Code = "00001", Name = "First Adventurer" } }),
//            //    (Card)SceneContainer.Instantiate(typeof(Card00002), new object[] { new CardInfo() { Description = "AJJAJA", Cost = 0, CardType = CardType.Creature, Code = "00002", Name = "Montro2" } })
//            //};


//            yield return LoadScene("GamePlay");

//            yield return new WaitForSeconds(10);

//            //sut.BuildCards(cards);

//            //Assert.That(sut.transform.childCount, Is.EqualTo(2));

//            //CardView cardView = sut.transform.GetChild(0).GetComponent<CardView>();
//            //Assert.That(cardView.Card.Info.Name, Is.EqualTo("First Adventurer"));

//            //cardView = sut.transform.GetChild(1).GetComponent<CardView>();
//            //Assert.That(cardView.Card.Info.CardType, Is.EqualTo(CardType.Creature));

//            //CardGeneratorComponent component = Object.FindAnyObjectByType<CardGeneratorComponent>();
//            //component.BuildCards(cards);
//            yield return null;
//        }
//    }
//}
