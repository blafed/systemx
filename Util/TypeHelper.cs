using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using Random = UnityEngine.Random;
public static class TypeHelper
{
    public static IEnumerable<Type> AllOfInterface(Type interfaceType)
    {
        var result =
        from assembly in System.AppDomain.CurrentDomain.GetAssemblies()
        from type in assembly.GetTypes()
        where type.GetInterfaces().Any(predicate: x => x == interfaceType)
        select type;
        return result.Select(x => (Type)x);
    }
    public static IEnumerable<Type> AllOfClass(Type classType)
    {
        var result =
        from assembly in System.AppDomain.CurrentDomain.GetAssemblies()
        from type in assembly.GetTypes()
        where type.IsSubclassOf(classType)
        select type;
        return result.Select(x => (Type)x);
    }
}