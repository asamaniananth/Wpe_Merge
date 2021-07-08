using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace Wpe_Merge_Test
{
    [TestClass]
    public class CsvAndApiHandlersTest
    {
        public IConfigurationRoot configuration;
        private readonly string inputEmptyCsvFilePath;
        private readonly string inputEmptyCsvFileAbsolutePath;
        public string CurrentPath { get; private set; }

        public CsvAndApiHandlersTest()
        {
            CurrentPath = Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile(Constants.APP_SETTINGS_FILE_NAME);
            configuration = builder.Build();
            inputEmptyCsvFilePath = configuration.GetSection(Constants.APP_SETTINGS_INPUT_EMPTY_CSV_FILE).Value;            
            inputEmptyCsvFileAbsolutePath = Path.GetFullPath(Path.Combine(CurrentPath, inputEmptyCsvFilePath));
        }

        [TestMethod]
        public void GetAccountListFromCsv_WhenNoRowsInCsvFile_ShouldReturnEmptyList()
        {
            // Arrange
            List<CsvAccountModel> expected = new List<CsvAccountModel>();
            // Act
            WpeMergeCodeBehindBusinessLogic.BusinessLogic.WpeMergeCodeBehindBusinessLogic context = new WpeMergeCodeBehindBusinessLogic.BusinessLogic.WpeMergeCodeBehindBusinessLogic();
            List<CsvAccountModel> actual = context.GetAccountListFromCsv(inputEmptyCsvFileAbsolutePath);
            // Assert            
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetAccountDetailsFromEndpoint_ShouldReturnSingleObject()
        {
            // Arrange
            EndpointAccountDetailsModel expected = new EndpointAccountDetailsModel
            {
                Account_Id = 12345,
                Status = "good",
                Created_On = "2011-01-12"
            };
            // Act
            WpeMergeCodeBehindBusinessLogic.BusinessLogic.WpeMergeCodeBehindBusinessLogic context = new WpeMergeCodeBehindBusinessLogic.BusinessLogic.WpeMergeCodeBehindBusinessLogic();
            EndpointAccountDetailsModel actual = context.GetAccountDetailsFromEndpointByAccountId(12345);
            // Assert 
            Assert.AreEqual(expected.Account_Id, actual.Account_Id);
            Assert.AreEqual(expected.Status, actual.Status);
            Assert.AreEqual(expected.Created_On, actual.Created_On);            
        }
    }
}
