using System;
using System.Collections.Generic;
using System.IO;
using WpeMergeCodeBehindBusinessLogic.Models;

namespace Wpe_Merge
{
    class Program
    {        
        public static void Main(string[] args)
        {
            string inputPath = args[0], outputPath = args[1];
            //string inputPath = @"C:\Users\anantha\Desktop\Interview\WP_Engine\input - input.csv";
            //string inputPath = @"..\..\..\..\..\..\input - input.csv";
            //string inputPath = @"..\input - input.csv";
            //string outputPath = @"C:\Users\anantha\Desktop\Interview\WP_Engine\output.csv";
            //string outputPath = @"..\..\output.csv";

            string inputAbsolutePath = Path.GetFullPath(inputPath);
            string outputAbsolutePath = Path.GetFullPath(outputPath);
            string result = "";

            //string inputRelativePath = Path.GetRelativePath()
            //string outputRelativePath = Path.GetFullPath(outputPath);

            string outputFilename = Path.GetFileName(outputAbsolutePath);

            if(File.Exists(inputAbsolutePath))
            {
                WpeMergeCodeBehindBusinessLogic.BusinessLogic.WpeMergeCodeBehindBusinessLogic context = new WpeMergeCodeBehindBusinessLogic.BusinessLogic.WpeMergeCodeBehindBusinessLogic();
                result = context.MergeCsvDataWithApi(inputAbsolutePath);
                File.WriteAllText(outputAbsolutePath, result);
            }           
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
