using Assets.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Rules.Round
{
    public class TennisRoundRules : RoundRules
    {
        FiniteStateMachine _fsm;

        private int _bounceCount;
        private int _bounceLimit;
        private bool IsBounceLegal(State state)
        {
            return _bounceCount <= _bounceLimit;
        }
        
        private bool HasBounced(State state)
        {
            return _bounceCount != 0;
        }

        private int _hitCount;
        private int _hitLimit;
        private bool IsHitLegal(State state)
        {
            return _hitCount <= _hitLimit;
        }

        public TennisRoundRules()
        {
            _fsm = new FiniteStateMachine();
            _hitLimit = 1;
            _bounceLimit = 1;
            _hitCount = 0;
            _bounceCount = 0;

            State to1 = new State("To1");
            State to2 = new State("To2");
            State win1 = new State("Win1");
            State win2 = new State("Win2");
            ConditionalState bounce1 = new ConditionalState("Bounce1", IsBounceLegal);
            ConditionalState hit1 = new ConditionalState("Hit1", IsHitLegal);
            ConditionalState bounce2 = new ConditionalState("Bounce2", IsBounceLegal);
            ConditionalState hit2 = new ConditionalState("Hit2", IsHitLegal);
            ConditionalState outside1 = new ConditionalState("Outside1", HasBounced);
            ConditionalState outside2 = new ConditionalState("Outside2", HasBounced);

            win1.OnEnter += Winner;
            win2.OnEnter += Winner;

            to1.AddEdge("Bounce2", win1);
            to1.AddEdge("Outside", outside1);
            to1.AddEdge("Hit2", hit2);
            to1.AddEdge("Bounce1", bounce1);
            to1.AddEdge("Hit1", to2);
            bounce1.AddEdge("true", to1);
            bounce1.AddEdge("false", win2);
            hit1.AddEdge("true", to2);
            hit1.AddEdge("false", win2);
            outside1.AddEdge("true", win2);
            outside1.AddEdge("false", win1);

            to2.AddEdge("Bounce1", win2);
            to2.AddEdge("Outside", outside2);
            to2.AddEdge("Hit1", hit1);
            to2.AddEdge("Bounce2", bounce2);
            to2.AddEdge("Hit2", to1);
            bounce2.AddEdge("true", to2);
            bounce2.AddEdge("false", win1);
            hit2.AddEdge("true", to1);
            hit2.AddEdge("false", win1);
            outside2.AddEdge("true", win1);
            outside2.AddEdge("false", win2);

            _fsm.Add(to1);
            _fsm.Add(to2);
            _fsm.Add(win1);
            _fsm.Add(win2);
            _fsm.Add(bounce1);
            _fsm.Add(hit1);
            _fsm.Add(bounce2);
            _fsm.Add(hit2);
            _fsm.Add(outside1);
            _fsm.Add(outside2);
        }

        public void OnStartRound(PlayerId player)
        {
            string[] state = {"", "To1", "To2"};

            _fsm.TrySetStateById(state[(int)player]);
            _hitCount = 0;
            _bounceCount = 0;
        }

        public void OnBallGroundBounce(PlayerId owner)
        {
            string[] edge = { "Bounce0", "Bounce1", "Bounce2" };

            if ((_fsm.Current.Id == "To1" && owner == PlayerId.Player1) ||
                (_fsm.Current.Id == "To2" && owner == PlayerId.Player2))
            {
                ++_bounceCount;
            }

            _fsm.TryPerformTransition(edge[(int)owner]);
        }

        public void OnBallOutside(Direction direction)
        {
            string[] edge = { "Outside", "Outside", "OutsideTop", "Outside" };

            _fsm.TryPerformTransition(edge[(int)direction]);
        }

        public void OnBallHit(PlayerId player)
        {
            string[] edge = { "Hit0", "Hit1", "Hit2" };

            if ((_fsm.Current.Id == "To1" && player == PlayerId.Player1) ||
                (_fsm.Current.Id == "To2" && player == PlayerId.Player2))
            {
                _hitCount = 0;
                _bounceCount = 0;
            }

            ++_hitCount;
            _fsm.TryPerformTransition(edge[(int)player]);
        }

        void Winner(State state)
        {
            PlayerId winner = PlayerId.Neutral;

            if (state.Id == "Win1")
                winner = PlayerId.Player1;
            else if (state.Id == "Win2")
                winner = PlayerId.Player2;

            OnWinner(winner);
        }

        public event OnWinnerHandler OnWinner = delegate { };
    }
}
