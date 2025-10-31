using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagnosticoWeb.Code 
{
    public static class Lista
    {
        public static IEnumerable<IEnumerable<TSource>> ChunkData<TSource>(this IEnumerable<TSource> source, int chunkSize)
        {
            for (int i = 0; i < source.Count(); i += chunkSize)
                yield return source.Skip(i).Take(chunkSize);
        } 
    }
}