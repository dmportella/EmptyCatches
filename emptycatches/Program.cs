using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.PatternMatching;
using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace emptycatches
{
    internal sealed class Program
    {
        internal static int Main(string[] args)
        {
            Console.WriteLine("Empty try catch finder, version {0}.", Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine("Copyright © dmportella 2012 all right reserved.");
            Console.WriteLine(Environment.NewLine);

            int errorCount = 0;
            
            CSharpParser parser = new CSharpParser();
            
            foreach(string fileName in Directory.GetFiles(args[0], "*.cs", SearchOption.AllDirectories))
            {
                SyntaxTree syntaxTree = null;

                using (FileStream file = File.OpenRead(fileName))
                {
                    syntaxTree = parser.Parse(file);
                }

                foreach (CatchClause node in syntaxTree.Descendants.OfType<CatchClause>())
                {
                    if (node.Body.Statements.Count == 0)
                    {
                        FileInfo inf = new FileInfo(fileName);

                        Console.WriteLine("Error: Found a empty catch on {0} line {1}.", inf.FullName, node.StartLocation.Line);

                        errorCount++;
                    }
                    else
                    {
                        if (args.Contains("--enforce-logging"))
                        {
                            var catchHasLogging = false;
                            foreach (var catchStatement in node.Body.Statements)
                            {
                                catchHasLogging = catchHasLogging || catchStatement.GetText().ToLower().Contains("log");
                            }
                            if (!catchHasLogging)
                            {
                                var fileInfo = new FileInfo(fileName);
                                Console.WriteLine("Error: Found a catch without logging on {0} line {1}:", fileInfo.FullName, node.StartLocation.Line);
                                Console.WriteLine(string.Join(Environment.NewLine, node.Body.Statements));
                                Console.WriteLine();
                                errorCount++;
                            }
                        }

                        //TODO just scanning the first instance will change that to loop through them all at some point.
                        ThrowStatement throwStatement = node.Descendants.OfType<ThrowStatement>().FirstOrDefault();

                        if (throwStatement != null && (throwStatement.Expression as IdentifierExpression) != null)
                        {
                            IdentifierExpression ex = throwStatement.Expression as IdentifierExpression;

                            if (string.Compare(node.VariableName, ex.Identifier, false) == 0)
                            {
                                FileInfo inf = new FileInfo(fileName);

                                Console.WriteLine("Error: Found a catch block with a bad rethrow on {0} line {1}.", inf.FullName, node.StartLocation.Line);

                                errorCount++;
                            }
                        }
                    }
                }
            }

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Number of errors found: {0}", errorCount);

            return errorCount;
        }
    }
}
