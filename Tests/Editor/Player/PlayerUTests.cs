using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Characters.UTests;

namespace Players.UTests
{
    public class PlayerUTests
    {
        private Player player;

        // Create test player
        public static Player CreateTestPlayer()
        {
            Player newPlayer = TestFunctions.CreateClassObject<Player>(ENV.PLAYER_PREFAB_FULL_PATH);
            return newPlayer;
        }

        // Create test player
        public static Player CreateTestPlayer(int id)
        {
            Player newPlayer = TestFunctions.CreateClassObject<Player>(ENV.PLAYER_PREFAB_FULL_PATH);
            newPlayer.id = id;
            return newPlayer;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            player = CreateTestPlayer();
        }

        // Teardown
        [TearDown]
        public void Teardown()
        {
            C.Destroy(player);
        }

        // Test player created
        [Test]
        public void CreatesPlayer()
        {
            Assert.IsNotNull(player);
        }

        // Test player resets
        [Test]
        public void Resets()
        {
            player.Reset();
            Assert.AreEqual(0, player.heroPoints);
            Assert.AreEqual(0, player.villainPoints);
        }

        // Test update hero points
        [Test]
        public void AddsHeroPoints()
        {
            int points = 10;
            player.AddHeroPoints(points);
            Assert.AreEqual(points, player.heroPoints);
            Assert.AreEqual(0.1f, player.playerUi.heroBar.fillAmount);
        }

        // Test update villain points
        [Test]
        public void AddsVillainPoints()
        {
            int points = 10;
            player.AddVillainPoints(points);
            Assert.AreEqual(points, player.villainPoints);
            Assert.AreEqual(0.1f, player.playerUi.villainBar.fillAmount);
        }
    }
}