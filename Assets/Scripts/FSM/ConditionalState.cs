using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.FSM
{
    public class ConditionalState : State
    {
        Func<State, bool> _condition;

        public ConditionalState(string id, Func<State, bool> condition) :
            base(id)
        {
            _condition = condition;
        }

        public override State GoHere()
        {
            State state;
            if (_condition(this))
            {
                if (Edges.TryGetValue("true", out state))
                    return state.GoHere();
            }
            else
            {
                if (Edges.TryGetValue("false", out state))
                    return state.GoHere(); 
            }

            return null;
        }
    }
}
