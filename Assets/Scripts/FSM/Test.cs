using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.FSM
{
    class Test
    {
        private static bool verbose;
        private static bool result;
        private static void print(string Id, bool test)
        {
            if(verbose)
                Debug.Log(Id + " : " + test);
            if(!test)
                result = false;
        }

        public static void Test1()
        {
            int value = 5;
            State s1 = new State("s1");
            State s2 = new State("s2");
            State s3 = new State("s3");
            State s4 = new State("s4");
            State s5 = new ConditionalState("s5", (state) => value < 10);

            s1.AddEdge("e1", s2);
            s1.AddEdge("e2", s3);
            s1.AddEdge("e3", s5);

            s2.AddEdge("e5", s4);

            s3.AddEdge("e3", s2);
            s3.AddEdge("e4", s4);

            s4.AddEdge("e1", s1);

            s5.AddEdge("true", s2);
            s5.AddEdge("false", s4);

            verbose = true;
            result = true;
            if (verbose)
                Debug.Log("Start test 1");
            State cur = s1;
            cur = cur.Step("e2");
            print("s1 =e2=> s3", cur == s3);

            cur = cur.Step("e3");
            print("s3 =e3=> s2", cur == s2);

            cur = cur.Step("e5");
            print("s2 =e5=> s4", cur == s4);

            cur = cur.Step("e1");
            print("s4 =e1=> s1", cur == s1);

            cur = cur.Step("e2");
            print("s1 =e2=> s3", cur == s3);

            cur = cur.Step("e4");
            print("s3 =e4=> s4", cur == s4);

            Debug.Log("Result test 1 : " + result);

            result = true;
            if (verbose)
                Debug.Log("Start test 2");

            value = 5;
            cur = s1;
            cur = cur.Step("e3");
            print("s1 =e3/if(5<10)=> s2 ", cur == s2);

            value = 15;
            cur = s1;
            cur = cur.Step("e3");
            print("s1 =e3/if(15<10)=> s4", cur == s4);

            Debug.Log("Result test 2 : " + result);
        }
    }
}
