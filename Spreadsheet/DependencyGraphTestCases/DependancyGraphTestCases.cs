using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies;

namespace DependenciesTest
{
    [TestClass]
    public class DependancyGraphTestCases
    {
        private string a = "a";
        private string b = "b";
        private string c = "c";
        private string d = "d";
        /// <summary>
        /// Tests constructor.
        /// </summary>
        [TestMethod]
        public void Test1()
        {
            DependencyGraph graph = new DependencyGraph();
            Assert.AreEqual(0, graph.Size);
        }

        /// <summary>
        /// Tests adding the first dependency
        /// </summary>
        [TestMethod]
        public void Test2()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            Assert.AreEqual(1, graph.Size);
        }

        /// <summary>
        /// Tests adding 2 dependencies.
        /// </summary>
        [TestMethod]
        public void Test3()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            graph.AddDependency(c, d);
            Assert.AreEqual(2, graph.Size);
        }
        /// <summary>
        /// Tests adding the same dependency.
        /// </summary>
        [TestMethod]
        public void Test4()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            graph.AddDependency(a, b);
            Assert.AreEqual(1, graph.Size);
        }
        /// <summary>
        /// Tests adding the same dependency twice, once forwards and once in reverse. This creates a loop which is problematic. This passes but is bad.
        /// </summary>
        [TestMethod]
        public void Test5()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            graph.AddDependency(b, a);
            Assert.AreEqual(2, graph.Size);
        }
        /// <summary>
        /// Tests adding the 2 dependees dependency.
        /// </summary>
        [TestMethod]
        public void Test6()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            graph.AddDependency(a, c);
            Assert.AreEqual(2, graph.Size);
            
        }
        /// <summary>
        /// Tests adding the 2 dependees dependency.
        /// </summary>
        [TestMethod]
        public void Test7()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            graph.AddDependency(c, b);
            Assert.AreEqual(2, graph.Size);

        }
        /// <summary>
        /// Tests adding 3 dependencies.
        /// </summary>
        [TestMethod]
        public void Test8()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            graph.AddDependency(b, c);
            graph.AddDependency(c, d);
            Assert.AreEqual(3, graph.Size);

        }
        /// <summary>
        /// Tests adding many duplicate dependencies.
        /// </summary>
        [TestMethod]
        public void Test9()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            graph.AddDependency(a, b);
            graph.AddDependency(a, b);
            graph.AddDependency(a, b);
            graph.AddDependency(a, b);
            graph.AddDependency(a, b);
            graph.AddDependency(a, b);
            graph.AddDependency(a, b);
            Assert.AreEqual(1, graph.Size);

        }
        /// <summary>
        /// Tests has dependents when true.
        /// </summary>
        [TestMethod]
        public void Test10()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            Assert.IsTrue(graph.HasDependents(a));

        }
        /// <summary>
        /// Tests has dependents
        /// </summary>
        [TestMethod]
        public void Test11()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            Assert.IsFalse(graph.HasDependents(b));

        }
        /// <summary>
        /// Tests has dependees when true.
        /// </summary>
        [TestMethod]
        public void Test12()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            Assert.IsTrue(graph.HasDependees(b));

        }
        /// <summary>
        /// Tests HasDependees when answer is false.
        /// </summary>
        [TestMethod]
        public void Test13()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            Assert.IsFalse(graph.HasDependees(a));

        }
        /// <summary>
        /// Test removing when graph is empty.
        /// </summary>
        [TestMethod]
        public void Test14()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.RemoveDependency(a, b);
            Assert.AreEqual(0, graph.Size);

        }
        /// <summary>
        /// Test removing the graph's only element.
        /// </summary>
        [TestMethod]
        public void Test15()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            graph.RemoveDependency(a, b);
            Assert.AreEqual(0, graph.Size);

        }
        /// <summary>
        /// Test removing elements not in the graph.
        /// </summary>
        [TestMethod]
        public void Test16()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            graph.RemoveDependency(c, d);
            graph.RemoveDependency(b, c);
            graph.RemoveDependency(c, d);
            graph.RemoveDependency(d, c);
            Assert.AreEqual(1, graph.Size);

        }
        /// <summary>
        /// Test removing a non-unique dependent.
        /// </summary>
        [TestMethod]
        public void Test17()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            graph.AddDependency(a, c);
            graph.RemoveDependency(a, b);
            Assert.AreEqual(1, graph.Size);

        }
        /// <summary>
        /// Test removing a non-unique dependee.
        /// </summary>
        [TestMethod]
        public void Test18()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            graph.AddDependency(c, b);
            graph.RemoveDependency(a, b);
            Assert.AreEqual(1, graph.Size);

        }
        /// <summary>
        /// Tests GetDependants.
        /// </summary>
        [TestMethod]
        public void Test19()
        {
            var dependents = new List<string>();
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            graph.AddDependency(a, c);
            graph.AddDependency(a, d);
            foreach (string str in graph.GetDependents(a))
            {
                dependents.Add(str);
            }

            Assert.AreEqual("b", dependents.ElementAt(0));
            Assert.AreEqual("c", dependents.ElementAt(1));
            Assert.AreEqual("d", dependents.ElementAt(2));

        }
        /// <summary>
        /// Tests GetDependents when there are none.
        /// </summary>
        [TestMethod]
        public void Test20()
        {
            var dependents = new List<string>();
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            graph.AddDependency(a, c);
            graph.AddDependency(a, d);
            foreach (string str in graph.GetDependents(b))
            {
                dependents.Add(str);
            }

            Assert.AreEqual(0, dependents.Count);
            //Assert.AreEqual("c", dependents.ElementAt(1));
            //Assert.AreEqual("d", dependents.ElementAt(2));
        }
        /// <summary>
        /// Tests GetDependees.
        /// </summary>
        [TestMethod]
        public void Test21()
        {
            var dependees = new List<string>();
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            graph.AddDependency(c, b);
            graph.AddDependency(d, b);
            foreach (string str in graph.GetDependees(b))
            {
                dependees.Add(str);
            }

            Assert.AreEqual("a", dependees.ElementAt(0));
            Assert.AreEqual("c", dependees.ElementAt(1));
            Assert.AreEqual("d", dependees.ElementAt(2));

        }
        /// <summary>
        /// Tests GetDependees when there are none.
        /// </summary>
        [TestMethod]
        public void Test22()
        {
            var dependees = new List<string>();
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, c);
            graph.AddDependency(c, d);
            graph.AddDependency(d, "e");
            foreach (string str in graph.GetDependees(b))
            {
                dependees.Add(str);
            }

            Assert.AreEqual(0, dependees.Count);

        }
        /// <summary>
        /// Tests ReplaceDependants.
        /// </summary>
        [TestMethod]
        public void Test23()
        {
            var olddep = new List<string>();
            var newdep = new List<string>();
            olddep.Add("e");
            olddep.Add("f");
            olddep.Add("g");
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            graph.AddDependency(a, c);
            graph.AddDependency(c, d);
            graph.AddDependency(c, b);
            graph.AddDependency(d, b);

            graph.ReplaceDependents(a,olddep);

            foreach (string str in graph.GetDependents(a))
            {
                newdep.Add(str);
            }

            Assert.AreSame("e", newdep.ElementAt(0));
            Assert.AreSame("f", newdep.ElementAt(1));
            Assert.AreSame("g", newdep.ElementAt(2));

        }
        /// <summary>
        /// Tests ReplaceDependees.
        /// </summary>
        [TestMethod]
        public void Test24()
        {
            var olddep = new List<string>();
            var newdep = new List<string>();
            olddep.Add("e");
            olddep.Add("f");
            olddep.Add("g");
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            graph.AddDependency(a, c);
            graph.AddDependency(c, d);
            graph.AddDependency(c, b);
            graph.AddDependency(d, b);

            graph.ReplaceDependees(b, olddep);

            foreach (string str in graph.GetDependees(b))
            {
                newdep.Add(str);
            }

            Assert.AreSame("e", newdep.ElementAt(0));
            Assert.AreSame("f", newdep.ElementAt(1));
            Assert.AreSame("g", newdep.ElementAt(2));

        }
        /// <summary>
        /// Bit of an adding stress test.
        /// </summary>
        [TestMethod]
        public void Test25()
        {
            DependencyGraph graph = new DependencyGraph();
            int count = 100000;
            for(int i=0; i < count; i++)
            {
                graph.AddDependency(i.ToString(), (i+1).ToString());
            }
            Assert.AreEqual(count, graph.Size);
        }
        /// <summary>
        /// Another stress test.
        /// </summary>
        [TestMethod]
        public void Test26()
        {
            DependencyGraph graph = new DependencyGraph();
            int count = 100000;
            for (int i = 0; i < count; i++)
            {
                graph.AddDependency(i.ToString(), (i + 1).ToString());
            }
            for(int i = 0; i < count; i+=2)
            {
                graph.RemoveDependency(i.ToString(), (i + 1).ToString());
            }
            Assert.AreEqual((count/2), graph.Size);
        }




    }
}
