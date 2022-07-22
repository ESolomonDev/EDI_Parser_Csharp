# EDI_Parser_Csharp
code in C# along with bash commands to compile into an .exe that will parse a edi file into an C# object in memory

This script is specifically for EDI-850 but the same general logic can be reworked to parse other EDI files. The logic in the script uses the index of the characters to find the delimiters (in this case * for segments and ~ for line endings)... can search for "/n : new line" characters too, slightly clever if the file is formatted that way. 
The code was created following a tutorial online, which went on to show ways to write the C# object to disk as an XML file.
The blog of online instructer is: https://mylifeismymessage.net/
He has some pretty neat coding snippits there and is a fairly seasoned dev. Has tutorials on UDEMY which includes scripts containing the same functionailty in Python and JS. 

I have been coding in python lately but missed JAVA from uni days and have always been meaning to expose myself to C# more.


Bash command to compile script - make sure .NET framework is installed on machine ya trying to compile it on

C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe /t:exe /out:EDIX12Parser2.exe EDIX12Parser.cs