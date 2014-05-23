using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Rules
{
    public enum PlayerId
    {
        Neutral = 0,
        Player1 = 1,
        Player2 = 2
    }

    public delegate void OnWinnerHandler(PlayerId player);

}
