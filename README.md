# Simple_Container

<h2>Simple DI container built for Winforms</h2>

<summary>This Container is very basic and designed as a learning tool there are (every other container) better and more advanced containers that handle this topic more gracefully for example :- Entity Framework, Castle Windsor, Ninject ... the list goes on.</summary>

<summary>Examples</summary>

```C#
var Container = new MicroContainer();

Container.RegisterSingleton(typeof(RandomGenerator));
Container.RegisterTransient<NumberClass>();

var tnc1 = Container.Get<RandomGenerator>();
var tnc2 = Container.Get<RandomGenerator>();

Debug.WriteLine("tnc1 Value " + tnc1.GetRandomGuid());
Debug.WriteLine("tnc2 Value " + tnc2.GetRandomGuid());

