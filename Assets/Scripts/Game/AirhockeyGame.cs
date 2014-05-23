using Assets.Rules.Game;
using Assets.Rules.Round;
using Assets.Rules.Set;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Game
{
    public class AirhockeyGame : Game
    {
        public AirhockeyGame(int setsPerGame, int roundsPerSet) :
            base(new SimpleScoreGameRules(setsPerGame), new SimpleScoreSetRules(roundsPerSet), new AirhockeyRoundRules())
        {
            RoundRules.OnWinner += SetRules.OnRoundWinner;
            SetRules.OnWinner += GameRules.OnSetWinner;
        }
    }
}

