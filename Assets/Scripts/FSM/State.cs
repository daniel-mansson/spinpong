using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.FSM
{
    public class State
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            private set { _id = value; }
        }

        private Dictionary<string, State> _edges;
        public Dictionary<string, State> Edges
        {
            get { return _edges; }
            private set { _edges = value; }
        }
        
        public State(string id)
        {
            Id = id;
            Edges = new Dictionary<string, State>();
        }

        public void AddEdge(string edge, State state)
        {
            Edges.Add(edge, state);
        }

        public State Step(string edge)
        { 
            State state;
            if (_edges.TryGetValue(edge, out state))
                return state.GoHere();

            return null;
        }

        public virtual State GoHere()
        {
            return this;
        }

        public event OnEnterHandler OnEnter = delegate { };
        public delegate void OnEnterHandler(State state);

        public void NotifyOnEnter()
        {
            OnEnter(this);
        }
    }
}
