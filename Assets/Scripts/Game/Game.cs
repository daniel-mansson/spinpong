using Assets.Rules.Game;
using Assets.Rules.Round;
using Assets.Rules.Set;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Game
{
    public class Game
    {
        private RoundRules _roundRules;
        public RoundRules RoundRules
        {
            get { return _roundRules; }
            private set { _roundRules = value; }
        }

        private SetRules _setRules;
        public SetRules SetRules
        {
            get { return _setRules; }
            private set { _setRules = value; }
        }

        private GameRules _gameRules;
        public GameRules GameRules
        {
            get { return _gameRules; }
            private set { _gameRules = value; }
        }

        public Game(GameRules gameRules, SetRules setRules, RoundRules roundRules)
        {
            RoundRules = roundRules;
            SetRules = setRules;
            GameRules = gameRules;
        }
    }
}
