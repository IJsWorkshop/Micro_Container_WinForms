using System.Collections.Generic;
using System.Linq;
using System;

namespace Simple_Container
{
    public class MicroContainer
    {
        readonly Dictionary<Type, MicroService> ServiceDescriptions = new Dictionary<Type, MicroService>();

        public MicroContainer()
        {
            ServiceDescriptions = new Dictionary<Type, MicroService>();
        }

        void FindType<T>(MicroService sd)
        {
            if (!ServiceDescriptions.TryGetValue(typeof(T), out var NotImportantPleaseIgnore))
                ServiceDescriptions.Add(typeof(T), sd);
            else 
                throw new Exception($"Service already registered -> {typeof(T).ToString()}");
        }

        void FindType(Type type, MicroService sd)
        {
            if (!ServiceDescriptions.TryGetValue(type, out var NotImportantPleaseIgnore))
                ServiceDescriptions.Add(type, sd);
            else
                throw new Exception($"Service already registered -> {type.ToString()}");
        }

        public void RegisterSingleton(object implementation) => FindType(implementation.GetType(), new MicroService(implementation, MicroLifeTime.Singleton));

        public void RegisterSingleton<T>() => FindType<T>(new MicroService(typeof(T), MicroLifeTime.Singleton));

        public void RegisterSingleton<Tin, Tout>() => FindType(typeof(Tin), new MicroService(typeof(Tout), MicroLifeTime.Singleton));

        public void RegisterTransient(object implementation) => FindType(implementation.GetType(), new MicroService(implementation, MicroLifeTime.Transient));

        public void RegisterTransient<T>() => FindType<T>(new MicroService(typeof(T), MicroLifeTime.Transient));

        public void RegisterTransient<Tin, Tout>() => FindType(typeof(Tin), new MicroService(typeof(Tout), MicroLifeTime.Transient));

        T CreateInstance<T>()
        {
            var constructor = typeof(T).GetConstructors().OrderByDescending(o => o.GetParameters().Length).FirstOrDefault();

            var args = constructor.GetParameters().Select(_ => Get<T>(_.ParameterType)).ToArray();

            object implementation = null;

            // if has constructor NO args
            if (args == null || args.Length == 0)
                implementation = (T)Activator.CreateInstance<T>();
            else // if has constructor and args
                implementation = (T)Activator.CreateInstance(typeof(T), args);

            return (T)implementation;
        }

        object CreateInstance(Type type)
        {
            var constructor = type.GetConstructors().OrderByDescending(o => o.GetParameters().Length).FirstOrDefault();

            if (constructor is null) throw new Exception("constructor is null");

            var args = constructor.GetParameters().Select(_ => Get(_.ParameterType)).ToArray();

            object implementation = null;

            // if has constructor NO args
            if (args == null)
                implementation = Activator.CreateInstance(type);
            else // if has constructor and args
                implementation = Activator.CreateInstance(type, args);

            return implementation;
        }

        public object Get(Type type) => Get(type);

        public T Get<T>() => (T)Get<T>(typeof(T));

        T Get<T>(Type type)
        {
            if (ServiceDescriptions.TryGetValue(type, out var ServiceImplementation))
            {
                if (ServiceImplementation.Implementation != null)
                {
                    // TODO: remove this and throw as we dont want any random instance being created
                    // if registered - do this (default condition)
                    //if (ServiceImplementation.Lifetime == ServiceLifeTime.Singleton)  return (T)ServiceImplementation.Implementation;

                    //TODO: this is the scoped reference which is not implemented yet .... 
                    //TODO: if registered - remove and renew (not implemented yet) 

                    if (ServiceImplementation.LifeTime == MicroLifeTime.Transient)
                        ServiceImplementation.Implementation = CreateInstance<T>();
                }
                else
                {
                    // if first time registering do this
                    if (ServiceImplementation.LifeTime == MicroLifeTime.Singleton)
                        ServiceImplementation.Implementation = CreateInstance(ServiceImplementation.ServiceType);
                    else if (ServiceImplementation.LifeTime == MicroLifeTime.Transient)
                        ServiceImplementation.Implementation = CreateInstance<T>();
                }

                return (T)ServiceImplementation.Implementation;
            }
            else
            {
                throw new Exception($"Type {typeof(T)} is not registered with MicroContainer");
            }
        }
    }

    public class MicroService
    {
        public Type ServiceType { get; set; }
        public object Implementation { get; set; }
        public MicroLifeTime LifeTime { get; set; }

        public MicroService(object implementation, MicroLifeTime serviceLifeTime)
        {
            ServiceType = implementation.GetType();
            Implementation = implementation;
            LifeTime = serviceLifeTime;
        }

        public MicroService(Type serviceType, MicroLifeTime serviceLifeTime)
        {
            ServiceType = serviceType;
            Implementation = null;
            LifeTime = serviceLifeTime;
        }
    }

    public enum MicroLifeTime
    {
        Singleton,
        Transient
        // Scoped
    }

}
