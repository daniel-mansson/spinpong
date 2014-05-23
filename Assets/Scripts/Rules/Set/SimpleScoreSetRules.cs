using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Rules.Set
{
    public class SimpleScoreSetRules : SetRules
    {
        private int[] _scores;
        private int _limit;
        private PlayerId _servingPlayer;
        
        public SimpleScoreSetRules(int limit)
        {
            _scores = new int[3];
            _limit = limit;
            _servingPlayer = PlayerId.Neutral;
        }

        public void OnRoundWinner(PlayerId player)
        {
            _servingPlayer = player;// (PlayerId)(3 - (int)_servingPlayer);

            switch (player)
            { 
                case PlayerId.Player1:
                case PlayerId.Player2:
                    _scores[(int)player] += 1;
                    break;
                default: 
                    _scores[(int)PlayerId.Neutral] += 1;
                    break;
            }

            for (int i = 1; i <= 2; ++i)
            {
                if (_scores[i] >= _limit)
                    OnWinner((PlayerId)i);
            }

        }

        public int GetRawScore(PlayerId player)
        {
            return _scores[(int)player];
        }

        public string GetFormattedScore(PlayerId player)
        {
            return _scores[(int)player].ToString();
        }

        public event OnWinnerHandler OnWinner = delegate { };

        public void OnSetStart(PlayerId startingPlayer)
        {
            for (int i = 0; i < _scores.Length; ++i)
                _scores[i] = 0;

            _servingPlayer = startingPlayer;
        }

        public PlayerId GetServingPlayer()
        {
            return _servingPlayer;
        }

    }
}
