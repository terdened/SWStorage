using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace SWStorage.Core
{
    class RdfStorage
    {
        private static IGraph _Graph;

        public RdfStorage()
        {
            _Graph = new Graph();
        }

        public RdfStorage(String path)
        {
            _Graph = new Graph();
            FileLoader.Load(_Graph, path);
        }

    }
}
