namespace Services.Common.Extensions
{
    public static class SortedSetExtension
    {
        public static T? ElementAfter<T>(this SortedSet<T> set, T element)
        {
            return set.ElementAfter(element, set.Comparer);
        }

        public static T? ElementBefore<T>(this SortedSet<T> set, T element)
        {
            return set.ElementBefore(element, set.Comparer);
        }

        public static T? ElementAfter<T>(
            this SortedSet<T> set, T element, IComparer<T> comparer)
        {
            return set.FirstOrDefault(e => comparer.Compare(e, element) > 0);
        }

        public static T? ElementBefore<T>(
            this SortedSet<T> set, T element, IComparer<T> comparer)
        {
            return set.Reverse().FirstOrDefault(e => comparer.Compare(e, element) < 0);
        }
    }
}
