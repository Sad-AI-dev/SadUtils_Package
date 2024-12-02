using System.Collections.Generic;

namespace SadUtils
{
    public static class EnumerableExtensions
    {
        public static T[] ToArray<T>(this IEnumerable<T> source)
        {
            T[] array = new T[source.Count()];

            int index = 0;
            foreach (T element in source)
            {
                array[index] = element;
                index++;
            }

            return array;
        }

        // Identical to System.Linq implementation
        public static int Count<T>(this IEnumerable<T> source)
        {
            if (source is ICollection<T> collection)
                return collection.Count;

            int counter = 0;

            using (IEnumerator<T> enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                    counter++;
            }

            return counter;
        }
    }
}
