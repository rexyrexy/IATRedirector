using System.Collections.Generic;

namespace IATRedirector {
    internal static class Utils {
        private static unsafe List<long> IndexesOf(byte[] Haystack, byte[] Needle) {
            List<long> Indexes = new List<long>();
            fixed (byte* H = Haystack) fixed (byte* N = Needle) {
                long i = 0;
                for (byte* hNext = H, hEnd = H + Haystack.LongLength; hNext < hEnd; i++, hNext++) {
                    bool Found = true;
                    for (byte* hInc = hNext, nInc = N, nEnd = N + Needle.LongLength; Found && nInc < nEnd; Found = *nInc == *hInc, nInc++, hInc++) ;
                    if (Found) Indexes.Add(i);
                }
                return Indexes;
            }
        }

        public static IEnumerable<long> FindReferences(byte[] data, byte[] toFind) {
            return IndexesOf(data, toFind);
        }
    }
}
