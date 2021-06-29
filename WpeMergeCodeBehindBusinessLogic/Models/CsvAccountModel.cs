using System;
using System.Collections.Generic;
using System.Text;

namespace WpeMergeCodeBehindBusinessLogic.Models
{
    public class CsvAccountModel
    {
        public long AccountId { get; set; }
        public string AccountName { get; set; }
        public  string FirstName { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class EndpointAccountDetailsModel
    {
        public int Account_Id { get; set; }
        public string Status { get; set; }
        public string Created_On { get; set; }
    }

    public class EndpointAccountInfoModel
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public object Previous { get; set; }
        public List<EndpointAccountDetailsModel> Results { get; set; }
    }
}
