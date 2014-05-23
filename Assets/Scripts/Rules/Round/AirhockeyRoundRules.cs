using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Rules.Round
{
    public class AirhockeyRoundRules : RoundRules
    {
        PlayerId _lastHit = PlayerId.Neutral;

        public void OnStartRound(PlayerId player)
        {
            _lastHit = player;
        }

        public void OnBallGroundBounce(PlayerId owner)
        {

        }

        public void OnBallOutside(Direction direction)
        {
            PlayerId winner = PlayerId.Neutral;

            if (direction == Direction.Right)
                winner = PlayerId.Player1;
            else if (direction == Direction.Left)
                winner = PlayerId.Player2;
            else
                winner = (PlayerId)(3 - (int)_lastHit);

            OnWinner(winner);
        }

        public void OnBallHit(PlayerId player)
        {
            _lastHit = player;
        }

        public event OnWinnerHandler OnWinner = delegate { };
    }
}
