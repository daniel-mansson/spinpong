using Assets.Rules.Round;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Rules
{
    public class Test
    {
        private static bool verbose;
        private static bool result;
        private static void print(string Id, bool test)
        {
            if (verbose)
                Debug.Log(Id + " : " + test);
            if (!test)
                result = false;
        }

        public static void Test1()
        {
            RoundRules round = new TennisRoundRules();

            PlayerId winner = PlayerId.Neutral;
            round.OnWinner += (player) => { winner = player; if(verbose)Debug.Log("Winner " + player + "!"); };

            verbose = true;
            verbose = false;

            result = true;
            winner = PlayerId.Neutral;
            if (verbose)
                Debug.Log("Start test 1");
            round.OnStartRound(PlayerId.Player1);
            round.OnBallHit(PlayerId.Player1);
            round.OnBallGroundBounce(PlayerId.Player1);
            print("Win 2 (" + winner + ")", winner == PlayerId.Player2);
            Debug.Log("Result test 1 : " + result);

            result = true;
            winner = PlayerId.Neutral;
            if (verbose)
                Debug.Log("Start test 2");
            round.OnStartRound(PlayerId.Player1);
            round.OnBallHit(PlayerId.Player1);
            round.OnBallGroundBounce(PlayerId.Player2);
            round.OnBallHit(PlayerId.Player2);
            round.OnBallHit(PlayerId.Player1);
            round.OnBallHit(PlayerId.Player2);
            round.OnBallGroundBounce(PlayerId.Player2);
            print("Win 1 (" + winner + ")", winner == PlayerId.Player1);
            Debug.Log("Result test 2 : " + result);

            result = true;
            winner = PlayerId.Neutral;
            if (verbose)
                Debug.Log("Start test 3");
            round.OnStartRound(PlayerId.Player1);
            round.OnBallHit(PlayerId.Player1);
            round.OnBallOutside(Direction.Right);
            print("Win 2 (" + winner + ")", winner == PlayerId.Player2);
            Debug.Log("Result test 3 : " + result);

            result = true;
            winner = PlayerId.Neutral;
            if (verbose)
                Debug.Log("Start test 4");
            round.OnStartRound(PlayerId.Player1);
            round.OnBallHit(PlayerId.Player1);
            round.OnBallOutside(Direction.Top);
            round.OnBallHit(PlayerId.Player2);
            round.OnBallOutside(Direction.Right);
            print("Win 1 (" + winner + ")", winner == PlayerId.Player1);
            Debug.Log("Result test 4 : " + result);

            result = true;
            winner = PlayerId.Neutral;
            if (verbose)
                Debug.Log("Start test 5");
            round.OnStartRound(PlayerId.Player1);
            round.OnBallHit(PlayerId.Player1);
            round.OnBallHit(PlayerId.Player1);
            print("Win 2 (" + winner + ")", winner == PlayerId.Player2);
            Debug.Log("Result test 5 : " + result);

            result = true;
            winner = PlayerId.Neutral;
            if (verbose)
                Debug.Log("Start test 6");
            round.OnStartRound(PlayerId.Player1);
            round.OnBallHit(PlayerId.Player1);
            round.OnBallGroundBounce(PlayerId.Player2);
            round.OnBallGroundBounce(PlayerId.Player2);
            print("Win 1 (" + winner + ")", winner == PlayerId.Player1);
            Debug.Log("Result test 6 : " + result);
        }
    }

}