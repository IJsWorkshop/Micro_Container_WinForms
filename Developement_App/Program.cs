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

            Container.RegisterSingleton(typeof(NumberClass));

            Container.RegisterTransient<RandomGenerator>();

            var tnc1 = Container.Get<RandomGenerator>();
            var tnc2 = Container.Get<RandomGenerator>();


            Debug.WriteLine("tnc1 Value " + tnc1.GetRandomGuid());
            Debug.WriteLine("tnc2 Value " + tnc2.GetRandomGuid());

            Console.ReadLine();
       }
    }
}
