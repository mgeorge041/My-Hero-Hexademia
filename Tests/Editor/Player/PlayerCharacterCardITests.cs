using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Players.UTests;
using Characters.UTests;

namespace Players.UTests
{
    public class PlayerCharacterCardITests
    {
        private Player player;
        private CharacterCard characterCard;

        // Setup
        [SetUp]
        public void Setup()
        {
            player = PlayerUTests.CreateTestPlayer();
            characterCard = CharacterCardUTests.CreateTestCharacterCard();
        }

        // Teardown
        [TearDown]
        public void Teardown()
        {
            C.Destroy(player);
            C.Destroy(characterCard);
        }

        // Test whether can play character card
        [Test]
        public void GetsCanPlayCharacterCard_EnoughPoints()
        {
            player.AddHeroPoints(100);
            bool canPlayCharacterCard = player.CanPlayCharacterCard(characterCard);
            Assert.IsTrue(canPlayCharacterCard);
        }

        // Test whether can play character card
        [Test]
        public void GetsCanPlayCharacterCard_NotEnoughPoints()
        {
            bool canPlayCharacterCard = player.CanPlayCharacterCard(characterCard);
            Assert.IsFalse(canPlayCharacterCard);
        }

        // Test play character card
        [Test]
        public void PlaysCharacterCard()
        {
            player.AddHeroPoints(100);
            player.characterCard = characterCard;
            player.PlayCharacterCard();
            Assert.IsNull(player.characterCard);
            Assert.AreEqual(75, player.heroPoints);
        }
    }
}