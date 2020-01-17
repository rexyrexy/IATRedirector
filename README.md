# IATRedirector
Redirect all intermodular calls to your function in PE File

# How to use it
Create config.json at directory then just drag-drop your PE file into IATRedirector. It will patch and save the output as (filename)_patched.(extension).

# Config format
It's just JSON format. Here is an example !
```
{
	"GetModuleHandleA": "OwnGetModuleHandleImport",
	"ExitProcess": "Sleep",
  "AImport", "BImport"
}
```

# Dependencies
https://github.com/JamesNK/Newtonsoft.Json
https://github.com/secana/PeNet
https://github.com/Fody/Costura
