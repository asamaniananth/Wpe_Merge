using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly string input3RecordsCsvFilePath;
        private readonly string input3RecordsCsvFileAbsolutePath;
        private readonly string inputNonCsvFilePath;
        private readonly string inputNonCsvFileAbsolutePath;
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
            input3RecordsCsvFilePath = configuration.GetSection(Constants.APP_SETTINGS_INPUT_3_RECORDS_CSV_FILE).Value;
            input3RecordsCsvFileAbsolutePath = Path.GetFullPath(Path.Combine(CurrentPath, input3RecordsCsvFilePath));
            inputNonCsvFilePath = configuration.GetSection(Constants.APP_SETTINGS_INPUT_NON_CSV_FILE).Value;
            inputNonCsvFileAbsolutePath = Path.GetFullPath(Path.Combine(CurrentPath, inputNonCsvFilePath));
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
        public void GetAccountListFromCsv_When3RowsInCsvFile_ShouldReturnListOf3()
        {
            // Arrange
            List<CsvAccountModel> expected = new List<CsvAccountModel>
            {
                new CsvAccountModel{AccountId=12345,AccountName="lexcorp",CreatedOn=DateTime.Parse("1/12/2011").Date,FirstName="Lex"},
                new CsvAccountModel{AccountId=8172,AccountName="latveriaembassy",CreatedOn=DateTime.Parse("11/19/2014").Date,FirstName="Victor"},
                new CsvAccountModel{AccountId=1924,AccountName="brotherhood",CreatedOn=DateTime.Parse("2/29/2012").Date,FirstName="Max"}
            };
            // Act
            WpeMergeCodeBehindBusinessLogic.BusinessLogic.WpeMergeCodeBehindBusinessLogic context = new WpeMergeCodeBehindBusinessLogic.BusinessLogic.WpeMergeCodeBehindBusinessLogic();
            List<CsvAccountModel> actual = context.GetAccountListFromCsv(input3RecordsCsvFileAbsolutePath);
            // Assert 
            int i = 0;
            while (i < actual.Count)
            {
                Assert.AreEqual(expected[i].AccountId, actual[i].AccountId);
                Assert.AreEqual(expected[i].AccountName, actual[i].AccountName);
                Assert.AreEqual(expected[i].CreatedOn, actual[i].CreatedOn);
                Assert.AreEqual(expected[i].FirstName, actual[i].FirstName);
                i++;
            }            
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

        [TestMethod]
        public void CheckWhetherCSVFile_WhenNonCsvFile_ShouldReturnFalse()
        {
            // Arrange
            bool expected = false;
            // Act
            WpeMergeCodeBehindBusinessLogic.BusinessLogic.WpeMergeCodeBehindBusinessLogic context = new WpeMergeCodeBehindBusinessLogic.BusinessLogic.WpeMergeCodeBehindBusinessLogic();
            bool actual = context.CheckWhetherCSVFile(inputNonCsvFileAbsolutePath);
            // Assert            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckWhetherCSVFile_WhenCsvFile_ShouldReturnTrue()
        {
            // Arrange
            bool expected = true;
            // Act
            WpeMergeCodeBehindBusinessLogic.BusinessLogic.WpeMergeCodeBehindBusinessLogic context = new WpeMergeCodeBehindBusinessLogic.BusinessLogic.WpeMergeCodeBehindBusinessLogic();
            bool actual = context.CheckWhetherCSVFile(inputEmptyCsvFileAbsolutePath);
            // Assert            
            Assert.AreEqual(expected, actual);
        }
    }
}
