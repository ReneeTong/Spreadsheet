using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        private Dictionary<string, HashSet<string>> dependents;
        private Dictionary<string, HashSet<string>> dependees;
        private int count;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new();
            dependees = new();
            count = 0;
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return count; }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                return getList(dependees, s).Count;
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            HashSet<string>? depends;
            if (dependents.TryGetValue(s, out depends))
            {
                if (depends.Count == 0)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            HashSet<string>? depends;
            if (dependees.TryGetValue(s, out depends))
            {
                if (depends.Count == 0)
                {
                    return false;
                }
                    return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            return getList(dependents, s);
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            return getList(dependees, s);
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            int prevCount = getList(dependents, s).Count;
            getList(dependents, s).Add(t);
            getList(dependees, t).Add(s);
            if(prevCount < getList(dependents, s).Count)
            {
                count++;
            }



        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            removePair(dependents, s, t);
            removePair(dependees, t, s);
            count--;
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {

            HashSet<string>? depees;
            if (dependents.TryGetValue(s, out depees))
            {
                foreach (string v in depees)
                {
                    removePair(dependees, v, s);
                    count--;
                }
            }

            //replace it to a new list
            dependents.Remove(s);
            HashSet<string> depends = new();

            foreach (string v in newDependents)
            {
                depends.Add(v);
                getList(dependees, v).Add(s);
                count++;
            }

            dependents.Add(s, depends);
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {

            HashSet<string>? depees;
            if (dependees.TryGetValue(s, out depees))
            {
                foreach (string v in depees)
                {
                    removePair(dependents, v, s);
                    count--;
                }
            }

            //replace it to a new list
            dependees.Remove(s);
            HashSet<string> depends = new();

            foreach (string v in newDependees)
            {
                depends.Add(v);
                getList(dependents, v).Add(s);
                count++;
            }


            dependees.Add(s, depends);

        }


        /// <summary>
        /// This method check if value exist for the given dictionary key, if not, retrun
        /// an empty list
        /// </summary>
        private HashSet<string> getList(Dictionary<string, HashSet<string>> set, string s)
        {
            HashSet<string>? depends;
            if (set.TryGetValue(s, out depends))
            {
                return depends;
            }
            else
            {
                HashSet<string> temp = new();
                set.Add(s, temp);
                return temp;
            }
        }

        /// <summary>
        /// This class remove pairs for given dictionary (dependents/dependees)
        /// </summary>
        private void removePair(Dictionary<string, HashSet<string>> set, string s, string t)
        {
            HashSet<string>? depends;
            if (set.TryGetValue(s, out depends))
            {
                depends.Remove(t);
            }
        }

    }
}