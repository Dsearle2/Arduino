using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class ArduinoUtils {

    public static IEnumerable<Type> GetTypes<T>() {
        var q = typeof(T).Assembly.GetTypes()
            .Where(x => !x.IsAbstract)
            .Where(x => !x.IsGenericTypeDefinition)
            .Where(x => typeof(T).IsAssignableFrom(x));

        //q = q.AppendWith(typeof(C1<>).MakeGenericType(typeof(GameObject)));
        //q = q.AppendWith(typeof(C1<>).MakeGenericType(typeof(AnimationCurve)));
        //q = q.AppendWith(typeof(C1<>).MakeGenericType(typeof(List<float>)));

        return q;
    }

}
