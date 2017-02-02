using System;
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
        /// Tests adding the same dependency twice, once forwards and once in reverse. This creates a loop which is problematic.
        /// </summary>
        [TestMethod]
        public void Test5()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(a, b);
            graph.AddDependency(b, a);
            Assert.AreEqual(1, graph.Size);
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




    }
}
