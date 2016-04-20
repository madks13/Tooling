using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Random
{
    public class StaticRandom
    {
        private System.Random _random = new System.Random();

        // static holder for instance, need to use lambda to construct since constructor private
        private static readonly Lazy<StaticRandom> _instance = new Lazy<StaticRandom>(() => new StaticRandom());
        
        // private to prevent direct instantiation.
        private StaticRandom()
        {
        }

        // accessor for instance
        public static StaticRandom Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public int Random()
        {
            return _random.Next();
        }
    }
}
