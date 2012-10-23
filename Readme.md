# Empty Catches detector

I hate when I see empty catch blocks in code so I have created this tool that will parse c# source code and look for bad usages of try catch blocks and excepton rethrows.

I am using ICSharpCode.NRefactory as my CSharp Parser.

Any contributions are welcome.

This tool is distributed under the MIT License.

I have added .net 4 and .net 4.5 support other versions to follow.

## Usage

Empty catches only accepts one parameter which the folder location to search from.

It will get all the *.cs files in that location to scan.

	emptycatches.exe "c:\solution folder"

Hmm... muffins..
