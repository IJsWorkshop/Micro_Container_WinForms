using static Simple_Container.TestClasses;
using System.Diagnostics;
using Simple_Container;

namespace Developement_App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var Container = new MicroContainer();
            
            Container.RegisterSingleton<IRandomGenerator, RandomGenerator>();
            Container.RegisterTransient<NumberClass>();
            

            var tnc = Container.Get<NumberClass>();

            var rg = Container.Get<IRandomGenerator>();
            var randomG = rg.GetRandomGuid();

            Debug.WriteLine("rg GUID Value " + randomG);

            Debug.WriteLine("tnc Value " + tnc.Value);

        }
    }
}
