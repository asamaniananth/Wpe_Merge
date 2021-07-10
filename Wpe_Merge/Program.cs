using System;
using System.IO;

namespace Wpe_Merge
{
    class Program
    {
        public static void Main(string[] args)
        {
            string inputPath = args[0], outputPath = args[1];

            string inputAbsolutePath = Path.GetFullPath(inputPath);
            string outputAbsolutePath = Path.GetFullPath(outputPath);
            string result = "";

            try
            {
                WpeMergeCodeBehindBusinessLogic.BusinessLogic.WpeMergeCodeBehindBusinessLogic context = new WpeMergeCodeBehindBusinessLogic.BusinessLogic.WpeMergeCodeBehindBusinessLogic();
                if (!context.CheckWhetherCSVFile(inputAbsolutePath))
                    Console.WriteLine("Input file is not csv.");
                if (!context.CheckWhetherCSVFile(outputAbsolutePath))
                    Console.WriteLine("Output file is not csv.");
                if (File.Exists(inputAbsolutePath) && context.CheckWhetherCSVFile(inputAbsolutePath) && context.CheckWhetherCSVFile(outputAbsolutePath))
                {
                    result = context.MergeCsvDataWithApi(inputAbsolutePath, outputAbsolutePath);
                    context.WriteDataToOutputFilePath(outputAbsolutePath, result, inputAbsolutePath);
                }
                else
                {
                    Console.WriteLine("Input csv file is not found in specified directory.");
                    Console.Write("Checked for csv input file in the directory: {0}", inputAbsolutePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }            
            Console.ReadKey();
        }
    }
}
