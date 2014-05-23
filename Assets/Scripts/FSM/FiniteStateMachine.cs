using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.FSM
{
    public class FiniteStateMachine
    {
        private State _current;
        public State Current
        {
            get { return _current; }
            set { _current = value; }
        }

        private List<State> _states;

        public FiniteStateMachine()
        {
            _states = new List<State>();
            _current = null;
        }

        public void Add(State state)
        {
            _states.Add(state);
        }

        public bool TrySetStateById(string id)
        {
            foreach(State s in _states)
            {
                if (s.Id == id)
                {
                    Current = s;
                    //Debug.Log("Enter:  " + Current.Id);
                    Current.NotifyOnEnter();
                    return true;
                }
            }

            return false;
        }

        public bool TryPerformTransition(string edge)
        {
            if (Current == null)
                return false;

            State state = Current.Step(edge);
            if (state == null)
                return false;

            Current = state;
           // Debug.Log("Enter(" + edge + "):  " + Current.Id);
            Current.NotifyOnEnter();
            return true;
        }
    }
}
