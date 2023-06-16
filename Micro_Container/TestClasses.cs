using System;

namespace Simple_Container
{
    public class TestClasses
    {
        public class RandomGenerator : IRandomGenerator
        {
            public Guid RandomGuid { get; private set; } = Guid.NewGuid();

            public string GetRandomGuid() => RandomGuid.ToString();

            public RandomGenerator()
            {

            }

        }

        public interface IRandomGenerator
        {
            Guid RandomGuid { get; }
            string GetRandomGuid();
        }


        public class NumberClass : INumberClass
        {
            public int Value { get; set; } = 10;
            public NumberClass() 
            {

            }

        }

        public interface INumberClass 
        {
            int Value { get; set; }
        }

    }
}
