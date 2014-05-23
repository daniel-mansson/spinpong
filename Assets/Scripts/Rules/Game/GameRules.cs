using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Rules.Game
{
    public interface GameRules
    {
        void OnGameStart();
        void OnSetWinner(PlayerId player);

        int GetRawScore(PlayerId player);
        string GetFormattedScore(PlayerId player);

        event OnWinnerHandler OnWinner;
    }
}
