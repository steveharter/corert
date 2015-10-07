﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

namespace ILToNative.DependencyAnalysisFramework
{
    /// <summary>
    /// Api for dependency analyzer. The expected use pattern is that a set of nodes will be added to the
    /// graph as roots. These nodes will internally implement the various dependency details
    /// so that if MarkedNodeList is called, it can produce the complete graph. For the case
    /// where nodes have deferred computation the ComputeDependencyRoutine even will be triggered
    /// to fill in data.
    /// 
    /// The Log visitor logic can be called at any time, and should log the current set of marked 
    /// nodes and edges in the analysis. (Notably, if its called before MarkedNodeList is evaluated
    /// it will contain only roots, if its called during, the edges/nodes may be incomplete, and
    /// if called after MarkedNodeList is computed it will be a complete graph.
    /// 
    /// </summary>
    /// <typeparam name="DependencyContextType"></typeparam>
    public abstract class DependencyAnalyzerBase<DependencyContextType>
    {
        /// <summary>
        /// Add a root node
        /// </summary>
        public abstract void AddRoot(DependencyNodeCore<DependencyContextType> rootNode, string reason);

        /// <summary>
        /// Return the marked node list. Do not modify this list, as it will cause unexpected behavior.
        /// </summary>
        public abstract ImmutableArray<DependencyNodeCore<DependencyContextType>> MarkedNodeList
        {
            get;
        }

        /// <summary>
        /// This event is triggered when a node is added to the graph.
        /// </summary>
        public abstract event Action<DependencyNodeCore<DependencyContextType>> NewMarkedNode;

        /// <summary>
        /// This event is triggered when the algorithm requires that dependencies of some set of
        /// nodes be computed.
        /// </summary>

        public abstract event Action<List<DependencyNodeCore<DependencyContextType>>> ComputeDependencyRoutine;

        /// <summary>
        /// Used to walk all nodes that should be emitted to a log. Not intended for other purposes.
        /// </summary>
        /// <param name="logNodeVisitor"></param>
        public abstract void VisitLogNodes(IDependencyAnalyzerLogNodeVisitor logNodeVisitor);

        /// <summary>
        /// Used to walk the logical edges in the graph as part of log building.
        /// </summary>
        /// <param name="logEdgeVisitor"></param>
        public abstract void VisitLogEdges(IDependencyAnalyzerLogEdgeVisitor logEdgeVisitor);
    }
}
