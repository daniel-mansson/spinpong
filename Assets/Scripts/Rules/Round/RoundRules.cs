using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Rules.Round
{
 
    public enum Direction
    {
        Left = 0,
        Right = 1,
        Top = 2,
        Bottom = 3,
    }

    public interface RoundRules
    {
        void OnStartRound(PlayerId player);

        void OnBallGroundBounce(PlayerId owner);
        void OnBallOutside(Direction direction);
        void OnBallHit(PlayerId player);

        event OnWinnerHandler OnWinner;
    }
}
