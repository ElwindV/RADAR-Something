using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class RPSTest
    {
        [Test]
        public void RPSTestSimplePasses()
        {
            Assert.AreEqual(RPS.Play(RPSType.Rock, RPSType.Rock), RPSWinner.Draw);
            Assert.AreEqual(RPS.Play(RPSType.Rock, RPSType.Paper), RPSWinner.Right);
            Assert.AreEqual(RPS.Play(RPSType.Rock, RPSType.Scissors), RPSWinner.Left);

            Assert.AreEqual(RPS.Play(RPSType.Paper, RPSType.Rock), RPSWinner.Left);
            Assert.AreEqual(RPS.Play(RPSType.Paper, RPSType.Paper), RPSWinner.Draw);
            Assert.AreEqual(RPS.Play(RPSType.Paper, RPSType.Scissors), RPSWinner.Right);

            Assert.AreEqual(RPS.Play(RPSType.Scissors, RPSType.Rock), RPSWinner.Right);
            Assert.AreEqual(RPS.Play(RPSType.Scissors, RPSType.Paper), RPSWinner.Left);
            Assert.AreEqual(RPS.Play(RPSType.Scissors, RPSType.Scissors), RPSWinner.Draw);
        }

    }
}
