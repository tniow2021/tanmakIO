using sex.Networking;
using sex.Pooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sex
{
    public static class Test
    {
        public static void AllTest()
        {
            PoolTest();
            UserIOTest();
        }
        public static void PoolTest()
        {
            PoolEngine pm = Root.root.poolEngine;
            pm.PrintPoolStatistics();
            Func<abc> aa = () => { return new abc(); };
            for (int i = 0; i < 30; i++)
            {
                IPool<abc> abcPool = Root.root.poolEngine.CreatePool<abc>(aa, 4000, 2 + i);
                abc a;
                for (int j = 0; j < 800; j++)
                {
                    a = abcPool.GetBlock();
                }
                for (int j = 0; j < 1000; j++)
                {
                    abcPool.RepayBlock(new abc());
                }
            }
            pm.PrintPoolStatistics();
        }
        public static void UserIOTest()
        {
            //UserIO a=new UserIO();
        }
        public class abc
        {
            int a, b, c;
        }
        public class dfg:abc
        {
            int r, g, s;
        }
    }
}
