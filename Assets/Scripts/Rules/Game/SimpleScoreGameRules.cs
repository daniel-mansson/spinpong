using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Rules.Game
{
    public class SimpleScoreGameRules : GameRules
    {
        private int[] _scores;
        private int _limit;

        public SimpleScoreGameRules(int limit)
        {
            _limit = limit;
            _scores = new int[3];
        }

        public void OnGameStart()
        {
            for (int i = 0; i < _scores.Length; ++i)
                _scores[i] = 0;
        }

        public void OnSetWinner(PlayerId player)
        {
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
    }
}
