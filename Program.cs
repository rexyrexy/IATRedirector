using Newtonsoft.Json;
using PeNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IATRedirector {
    class Program {
        static string GetCurrentDirectory() {
            return Path.GetDirectoryName(typeof(Program).Assembly.Location);
        }

        static byte[] PatchFile(IATManager manager, string originalFunc, string hookFunc) {
            var original = manager.GetFunction(originalFunc);
            if (original == null)
                throw new Exception($"Original: {originalFunc} not found !");

            var hook = manager.GetFunction(hookFunc);
            if (hook == null)
                throw new Exception($"Hook: {hookFunc} not found !");

            var iatThunk = manager.GetIATThunk(original);
            var delta = hook.IATOffset - original.IATOffset;
            var fullAddress = iatThunk + delta;
            return manager.PatchIAT(iatThunk, fullAddress);
        }

        static void Main(string[] args) {
            var file = args.FirstOrDefault();
            var baseDir = Path.GetDirectoryName(file);
            var outputPath = Path.Combine(baseDir, Path.GetFileNameWithoutExtension(file) + "_patched" + Path.GetExtension(file));

            var pe = new PeFile(file);
            var manager = new IATManager(pe);
            var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(Path.Combine(GetCurrentDirectory(), "config.json")));

            byte[] output = Array.Empty<byte>();
            foreach (var c in config)
                output = PatchFile(manager, c.Key, c.Value);

            File.WriteAllBytes(outputPath, output);
        }
    }
}
