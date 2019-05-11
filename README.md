# Speak
Simple C# project that uses built-in text2speech engine on MS Windows

Usage: Speak.exe [--v <voice>] [--s <speed>] [--l <language>] Sentence to say

   --v <voice>    - select voice to synthetise from installed list
   --s <speed>    - range -10...10
   --l <language> - try to select voice based on language code. If not found - fallback to default.
   --f <file>     - write sentence as wav to file.
   --q            - be quiet. Must be first option specified.


Examples:

__Select language:__
> Speak.exe --l EN "Hello world"

> Speak.exe --l PL "W Szczebrzeszynie chrząszcz brzmi w trzcinie."

__Select voice:__
> Speak.exe --v "Microsoft Paulina Desktop" "Help me i can't speak english."

> Speak.exe --v "Microsoft Zira Desktop" "Polska język być trudna język."

__Adjust speed:__

Fastest:
> Speak.exe --s 10 --l PL "W Szczebrzeszynie chrząszcz brzmi w trzcinie."

Slowest:

> Speak.exe --s -10 --l PL "W Szczebrzeszynie chrząszcz brzmi w trzcinie."