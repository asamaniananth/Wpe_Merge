using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using WpeMergeCodeBehindBusinessLogic.Models;
using WpeMergeCodeBehindBusinessLogic.Utils;

namespace WpeMergeCodeBehindBusinessLogic.BusinessLogic
{
    public class WpeMergeCodeBehindBusinessLogic
    {
        public List<CsvAccountModel> accountList = new List<CsvAccountModel>();
        public IConfigurationRoot configuration;
        private readonly string accountsListApi;
        private readonly string accountDetailsApi;

        public WpeMergeCodeBehindBusinessLogic()
        {
            var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appSettings.json");
            configuration = builder.Build();
            accountsListApi = configuration.GetSection(Constants.APP_SETTINGS_ACCOUNT_LIST_API).Value;
            accountDetailsApi = configuration.GetSection(Constants.APP_SETTINGS_ACCOUNT_DETAILS_API).Value;
        }

        public List<CsvAccountModel> GetAccountListFromCsv(string path)
        {
            using (var reader = new StreamReader(path))
            {
                string headers = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string[] values = reader.ReadLine().Split(',');
                    accountList.Add(new CsvAccountModel()
                    {
                        AccountId = long.Parse(values[0]),
                        AccountName = values[1],
                        FirstName = values[2],
                        CreatedOn = Convert.ToDateTime(values[3])
                    });
                }
            }
            return accountList;
        }

        public List<EndpointAccountDetailsModel> GetAccountListFromEndpoint()
        {
            string baseUrl = accountsListApi;
            EndpointAccountInfoModel _model = new EndpointAccountInfoModel();
            List<EndpointAccountDetailsModel> model = new List<EndpointAccountDetailsModel>();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync(baseUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        _model = JsonConvert.DeserializeObject<EndpointAccountInfoModel>(responseString);
                        foreach (var obj in _model.Results)
                        {
                            model.Add(obj);
                        }
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EndpointAccountDetailsModel GetAccountDetailsFromEndpointByAccountId(long id)
        {
            string baseUrl = accountDetailsApi + id;
            EndpointAccountDetailsModel model = new EndpointAccountDetailsModel();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync(baseUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        model = JsonConvert.DeserializeObject<EndpointAccountDetailsModel>(responseString);
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void MergeCsvDataWithApi()
        {
            List<CsvAccountModel> accountList = GetAccountListFromCsv("Nothing");
            StringBuilder sb = new StringBuilder();
            sb.Append(SetHeader());
            if (accountList.Count > 0)
            {
                foreach (var csvDetails in accountList)
                {
                    EndpointAccountDetailsModel apiDetails = GetAccountDetailsFromEndpointByAccountId(csvDetails.AccountId);
                    sb.Append(BuildCSVStringFromCsvAndApi(apiDetails, csvDetails));
                }
            }
        }

        public StringBuilder SetHeader()
        {
            //Account ID, First Name, Created On, Status, and Status Set On
            StringBuilder sb = new StringBuilder();
            sb.Append("Account ID" + ",");
            sb.Append("First Name" + ",");
            sb.Append("Created On" + ",");
            sb.Append("Status" + ",");
            sb.Append("Status Set On"); // from api
            sb.Append(Environment.NewLine);
            return sb;
        }

        public StringBuilder BuildCSVStringFromCsvAndApi(EndpointAccountDetailsModel apiDetails, CsvAccountModel csvDetails)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(apiDetails.Account_Id + ",");
            sb.Append(csvDetails.FirstName + ",");
            sb.Append(csvDetails.CreatedOn + ",");
            sb.Append(apiDetails.Status + ",");
            sb.Append(apiDetails.Created_On);
            sb.Append(Environment.NewLine);
            return sb;
        }


    }
}
