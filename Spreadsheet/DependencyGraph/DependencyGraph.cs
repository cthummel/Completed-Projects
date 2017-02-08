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
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            //var value = new List<string>();
            if (s == null)
            {
                throw new ArgumentNullException("Input is a null string.");
            }
            else
            {
                //Potentially incorrect if the dependents of s is empty. Look over later.
                if (Dependents.ContainsKey(s))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            //var value = new List<string>();
            if (s == null)
            {
                throw new ArgumentNullException("Input is a null string.");
            }
            else
            {
                //Potentially incorrect if the dependents of s is empty. Look over later.
                if (Dependees.ContainsKey(s))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("Input is a null string.");
            }
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
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("Input is a null string.");
            }
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
        /// </summary>
        public void AddDependency(string s, string t)
        {

            var dependent = new List<string>();
            var dependee = new List<string>();
            var temp = new List<string>();

            if (s == null || t == null)
            {
                throw new ArgumentNullException("Adding a dependancy requires a non-null dependant and dependee.");
            }
            if (Dependents.TryGetValue(s, out dependee))
            {

                //If not included in the graph already
                if (!dependee.Contains(t))
                {
                    Dependents.Remove(s);
                    dependee.Add(t);
                    Dependents.Add(s, dependee);

                    //Adding t to Dependees.
                    if(Dependees.TryGetValue(t, out dependent))
                    {
                        Dependees.Remove(t);
                        dependent.Add(s);
                        Dependees.Add(t, dependent);
                        graphsize += 1;
                    }
                    
                    else
                    {
                        dependee = new List<string>();
                        dependee.Add(s);
                        Dependees.Add(t, dependee);
                        graphsize += 1;
                    }
                }
            }
            else
            {
                dependee = new List<string>();
                dependee.Add(t);
                Dependents.Add(s, dependee);

                //If dependee is already in dictionary then we must deal with that before adding
                if (Dependees.TryGetValue(t, out temp))
                {
                    Dependees.Remove(t);
                    temp.Add(s);
                    Dependees.Add(t, temp);
                    graphsize += 1;
                }
                else
                {
                    dependent.Add(s);
                    Dependees.Add(t, dependent);
                    graphsize += 1;
                }
            }
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            var dependent = new List<string>();
            var dependee = new List<string>();

            if (s == null || t == null)
            {
                throw new ArgumentNullException("Removing a dependancy requires a non-null dependant and dependee.");
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
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (s == null || newDependents == null)
            {
                throw new ArgumentNullException("Replacing requires a non-null string and IEnumerable.");
            }
            var values = new List<string>();
            var oldvalues = new List<string>();

            Dependents.TryGetValue(s, out oldvalues);

            if (oldvalues == null)
            {
                return;
            }
            else
            {
                graphsize -= oldvalues.Count;
            }
           


            //Need to have dependents match the changes to dependees.
            foreach (string dependee in oldvalues)
            {
                //Finds dependee t as a value in the Dependant dictionary and removes it from the right lists.
                var tempdependents = new List<string>();
                Dependees.TryGetValue(dependee, out tempdependents);
                tempdependents.Remove(s);
                Dependees.Remove(dependee);
                Dependees.Add(dependee, tempdependents);
            }

            //Adds new Dependents to list with t as dependee as needed.
            foreach (string dependee in newDependents)
            {
                var dependent = new List<string>();

                if (Dependees.ContainsKey(dependee))
                {
                    Dependees.TryGetValue(dependee, out dependent);
                    dependent.Add(s);
                    Dependees.Remove(dependee);
                    Dependees.Add(dependee, dependent);

                }
                else
                {
                    dependent.Add(s);
                    Dependees.Add(dependee, dependent);
                }
            }

            Dependents.Remove(s);

            foreach (string dep in newDependents)
            {
                values.Add(dep);
            }

            Dependents.Add(s, values);
            graphsize += values.Count;
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if (t == null || newDependees == null)
            {
                throw new ArgumentNullException("Replacing requires a non-null string and IEnumerable.");
            }
            var values = new List<string>();
            var oldvalues = new List<string>();
            Dependees.TryGetValue(t, out oldvalues);

            if(oldvalues == null)
            {
                oldvalues = new List<string>();
            }
            graphsize -= oldvalues.Count;



            //Removes all (r,t) from Dependents dictionary.
            foreach (string dep in oldvalues)
            {
                var tempdependees = new List<string>();
                Dependents.TryGetValue(dep, out tempdependees);
                tempdependees.Remove(t);
                Dependents.Remove(dep);
                Dependents.Add(dep, tempdependees);
            }

            

            //Adds all new (s,t) to Dependents dictionary.
            foreach (string dependent in newDependees)
            {
                var tempdependee = new List<string>();
                //If s is in the dictionary already.
                if (Dependents.ContainsKey(dependent))
                {
                    
                    Dependents.TryGetValue(dependent, out tempdependee);
                    tempdependee.Add(t);
                    Dependents.Remove(dependent);
                    Dependents.Add(dependent, tempdependee);
                }
                else
                {
                    tempdependee.Add(t);
                    Dependents.Add(dependent, tempdependee);
                }
            }

            //Removes all (r,t) from Dependees dictionary.
            Dependees.Remove(t);

            //Adds all new (s,t) to Dependees dictionary.
            foreach (string dep in newDependees)
            {
                values.Add(dep);
            }
            Dependees.Add(t, values);
            graphsize += values.Count;

        }
    }
}
