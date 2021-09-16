using System;
using System.Collections.Generic;

public static class Utility
{
    public static IEnumerable<T> Generator<T>(T seed, Func<T, T> func)
    {
        T accum = seed;

        while (true)
        {
            yield return accum;
            accum = func.Invoke(accum);
        }
    }
}
