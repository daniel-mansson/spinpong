using Assets.Rules.Game;
using Assets.Rules.Set;
using Assets.Rules.Round;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Game
{
    public class SimpleTennisGame : Game
    {
        public SimpleTennisGame(int setsPerGame, int roundsPerSet) :
            base(new SimpleScoreGameRules(setsPerGame), new SimpleScoreSetRules(roundsPerSet), new TennisRoundRules())
        {
            RoundRules.OnWinner += SetRules.OnRoundWinner;
            SetRules.OnWinner += GameRules.OnSetWinner;
            
        }
    }
}
