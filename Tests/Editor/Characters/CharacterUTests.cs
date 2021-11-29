using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Characters.UTests;

namespace Characters.UTests
{
    public class CharacterUTests
    {
        private Character character;

        // Create test character
        public static Character CreateTestCharacter()
        {
            Character newCharacter = Character.CreateCharacter("Test");
            return newCharacter;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            character = CreateTestCharacter();
        }

        // Teardown
        [TearDown]
        public void Teardown()
        {

        }

        // Test create character
        [Test]
        public void CreatesCharacter()
        {
            Assert.IsNotNull(character);
        }

        // Test set speeds
        [Test]
        public void SetsSpeeds()
        {
            int xSpeed = 1;
            int ySpeed = 1;
            character.SetSpeeds(xSpeed, ySpeed);
            Assert.AreEqual(xSpeed, character.xSpeed);
            Assert.AreEqual(ySpeed, character.ySpeed);
        }

        // Test animator states
        [Test]
        public void SetsAnimatorState_Down()
        {
            character.SetSpeeds(0, -1);
            character.animator.Update(0.1f);
            Assert.AreEqual("Down", character.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        }

        // Test animator states
        [Test]
        public void SetsAnimatorState_Up()
        {
            character.SetSpeeds(0, 1);
            character.animator.Update(0.1f);
            Assert.AreEqual("Up", character.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        }

        // Test animator states
        [Test]
        public void SetsAnimatorState_UR()
        {
            character.SetSpeeds(1, 1);
            character.animator.Update(0.1f);
            Assert.AreEqual("UR", character.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        }

        // Test animator states
        [Test]
        public void SetsAnimatorState_UL()
        {
            character.SetSpeeds(-1, 1);
            character.animator.Update(0.1f);
            Assert.AreEqual("UL", character.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        }

        // Test animator states
        [Test]
        public void SetsAnimatorState_DR()
        {
            character.SetSpeeds(1, -1);
            character.animator.Update(0.1f);
            Assert.AreEqual("DR", character.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        }

        // Test animator states
        [Test]
        public void SetsAnimatorState_DL()
        {
            character.SetSpeeds(-1, -1);
            character.animator.Update(0.1f);
            Assert.AreEqual("DL", character.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        }

        // Test animator states
        [Test]
        public void SetsAnimatorState_Idle()
        {
            character.SetSpeeds(0, 0);
            character.animator.Update(0.1f);
            Assert.AreEqual("Idle", character.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        }

        // Test sets can't move
        [Test]
        public void SetsCantMoveWhenOutOfSpeed()
        {
            character.DecrementSpeed(character.remainingSpeed);
            Assert.IsFalse(character.canMove);
        }

        // Test has no actions after using full move and attack
        [Test]
        public void HasNoActionsAfterMoveAndAttack()
        {
            character.DecrementSpeed(character.remainingSpeed);
            character.canAttack = false;
            Assert.IsFalse(character.hasActions);
        }

        // Test starts turn
        [Test]
        public void StartsTurn()
        {
            character.StartTurn();
            Assert.IsTrue(character.hasActions);
            Assert.IsTrue(character.canMove);
            Assert.IsTrue(character.canAttack);
        }

        // Test ends turn
        [Test]
        public void EndsTurn()
        {
            character.EndTurn();
            Assert.IsFalse(character.hasActions);
            Assert.AreEqual(new Color(0.5f, 0.5f, 0.5f) ,character.spriteRenderer.color);
        }

        // Test takes damage
        [Test]
        public void TakesDamage()
        {
            int health = character.health;
            character.TakeDamage(5);
            Assert.AreEqual(health - 5, character.currentHealth);
        }

        // Test deals damage
        [Test]
        public void DealsDamage()
        {
            Character character2 = CreateTestCharacter();
            int health = character2.currentHealth;
            character.DealDamage(character2);
            Assert.AreEqual(health - 5, character2.currentHealth);
            Assert.IsFalse(character.hasActions);
        }

        // Test sets default ability
        [Test]
        public void SetsDefaultAbility()
        {
            Assert.AreEqual("Fireball", character.ability1.abilityName);
        }
    }
}