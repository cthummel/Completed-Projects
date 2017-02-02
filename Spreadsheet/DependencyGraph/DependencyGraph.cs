// Skeleton implementation written by Joe Zachary for CS 3500, January 2017.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    public class DependencyGraph
    {
        private Dictionary<string, List<string>> Dependents;
        private Dictionary<string, List<string>> Dependees;
        private int graphsize;

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            Dependents = new Dictionary<string, List<string>>();
            Dependees = new Dictionary<string, List<string>>();
            graphsize = 0;
            
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return graphsize; }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependents(string s)
        {
            //var value = new List<string>();
            if (s == null)
            {
                throw new Exception("Input is a null string.");
            }
            else
            {
                //Potentially incorrect if the dependents of s is empty. Look over later.
                if (Dependents.ContainsKey(s))
                {
                    //if (value.Count != 0)
                    //{
                        return true;
                    //}
                    //else
                    //{
                    //    return false;
                    //}
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            //var value = new List<string>();
            if (s == null)
            {
                throw new Exception("Input is a null string.");
            }
            else
            {
                //Potentially incorrect if the dependents of s is empty. Look over later.
                if (Dependees.ContainsKey(s))
                {
                    //if(value.Count != 0)
                    //{
                        return true;
                    //}
                    //else
                    //{
                    //    return false;
                    //}
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            var value = new List<string>();
            
            if (Dependents.TryGetValue(s, out value))
            {
                //Check whether this works on empty lists. Ex.   dependents("c") = {}
                foreach (string str in value)
                {
                    yield return str;
                }
            }
            else
            {
                yield break;
            }
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            //first search the dependees master list for string s. pull that dependees list out and enumerate it.
            var value = new List<string>();
            
            if (Dependees.TryGetValue(s, out value))
            {
                foreach (string str in value)
                {
                    yield return str;
                }
            }
            else
            {
                yield break;
            }
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void AddDependency(string s, string t)
        {

            var dependent = new List<string>();
            var dependee = new List<string>();
            var temp = new List<string>();

            if (s == null || t == null)
            {
                throw new Exception("Adding a dependancy requires a non-null dependant and dependee.");
            }
            if (Dependents.TryGetValue(s, out dependent))
            {
                
                //If not included in the graph already
                if (!dependent.Contains(t))
                {

                    //Adding s to Dependents.
                    Dependents.Remove(s);
                    dependent.Add(t);
                    Dependents.Add(s, dependent);

                    //Adding t to Dependees.
                    Dependees.TryGetValue(t, out dependee);
                    if (dependee == null)
                    {
                        dependee = new List<string>();
                        Dependees.Remove(t);
                        dependee.Add(s);
                        Dependees.Add(t, dependee);
                        graphsize += 1;
                    }
                    
                    
                }
                
            }
            else
            {
                if (dependent == null)
                {
                    dependent = new List<string>();
                    dependent.Add(t);
                    dependee.Add(s);
                    Dependents.Add(s, dependent);

                    //If dependee is already in dictionary then we must deal with that before adding
                    if(Dependees.TryGetValue(t, out temp))
                    {
                        Dependees.Remove(t);
                        temp.Add(s);
                        Dependees.Add(t, temp);
                        graphsize += 1;
                    }
                    else
                    {
                        Dependees.Add(t, dependee);
                        graphsize += 1;
                    }
                    
                }
                
                
            }
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            var dependent = new List<string>();
            var dependee = new List<string>();

            if (s == null || t == null)
            {
                throw new Exception("Adding a dependancy requires a non-null dependant and dependee.");
            }
            if (Dependents.TryGetValue(s, out dependent))
            {

                //If (s,t) is included in the graph already.
                if (dependent.Contains(t))
                {

                    //Removing t from s's Dependents.
                    Dependents.Remove(s);
                    dependent.Remove(t);
                    if (dependent.Count != 0)
                    {
                        Dependents.Add(s, dependent);
                    }

                    //Removing s from t's Dependees.
                    Dependees.TryGetValue(t, out dependee);

                    Dependees.Remove(t);
                    dependee.Remove(s);

                    if (dependee.Count != 0)
                    {
                        Dependees.Add(t, dependee);
                    }

                    graphsize -= 1;
                }
                
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            var values = new List<string>();

            Dependents.Remove(s);
            foreach (string dep in newDependents)
            {
                values.Add(dep);
            }
            Dependents.Add(s, values);
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            var values = new List<string>();

            Dependees.Remove(t);
            foreach (string dep in newDependees)
            {
                values.Add(dep);
            }
            Dependents.Add(t, values);
        }
    }
}
