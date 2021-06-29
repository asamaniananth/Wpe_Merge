using System;
using System.Collections.Generic;
using WpeMergeCodeBehindBusinessLogic.Models;

namespace Wpe_Merge
{
    class Program
    {        
        public static void Main(string[] args)
        {
            WpeMergeCodeBehindBusinessLogic.BusinessLogic.WpeMergeCodeBehindBusinessLogic context = new WpeMergeCodeBehindBusinessLogic.BusinessLogic.WpeMergeCodeBehindBusinessLogic();
            EndpointAccountDetailsModel result = context.GetAccountDetailsFromEndpointByAccountId(12345);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
