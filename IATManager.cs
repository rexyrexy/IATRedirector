using PeNet;
using System;
using System.Linq;

namespace IATRedirector {
    public class IATManager {
        public PeFile PeFile;
        public IATManager(PeFile file) {
            this.PeFile = file;
        }

        public ImportFunction GetFunction(string name) {
            return this.PeFile.ImportedFunctions.Where(x => x.Name == name).FirstOrDefault();
        }

        public uint GetIATThunk(ImportFunction function) {
            const int IAT_DIRECTORY_INDEX = 12;
            return (uint)this.PeFile.ImageNtHeaders.OptionalHeader.ImageBase +
                this.PeFile.ImageNtHeaders.OptionalHeader.DataDirectory[IAT_DIRECTORY_INDEX].VirtualAddress +
                function.IATOffset;
        }

        public unsafe byte[] PatchIAT(uint thunk, uint fullAddress) {
            var data = this.PeFile.Buff;
            var matches = Utils.FindReferences(data, BitConverter.GetBytes(thunk));
            fixed (byte* ptr = data) {
                foreach (var m in matches)
                    *(uint*)(ptr + m) = fullAddress;
            }

            return data;
        }
    }
}
