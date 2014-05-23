using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Rules.Set
{
    public interface SetRules
    {
        void OnSetStart(PlayerId startingPlayer);
        void OnRoundWinner(PlayerId player);

        PlayerId GetServingPlayer();

        int GetRawScore(PlayerId player);
        string GetFormattedScore(PlayerId player);

        event OnWinnerHandler OnWinner;
    }
}
