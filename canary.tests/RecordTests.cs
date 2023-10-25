using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using canary.Controllers;
using System.IO;
using Microsoft.AspNetCore.Http;
using VRDR;

using canary.Models;

namespace canary.tests
{
    public class RecordTests
    {
        string documentTypePayload = "";
        string messageTypePayload = "";
        string emptyTypePayload = "";

        public RecordTests()
        {
            documentTypePayload = File.ReadAllText(FixturePath("fixtures/json/DocumentTypePayload.json"));
            messageTypePayload = File.ReadAllText(FixturePath("fixtures/json/MessageTypePayload.json"));
            emptyTypePayload = File.ReadAllText(FixturePath("fixtures/json/EmptyTypePayload.json"));
        }


        [Fact]
        public void TestDocumentTypePayload()
        {
            var resultData = canary.Models.Record.CheckGet(documentTypePayload, true);
            Assert.NotNull(resultData);

            StringBuilder issueList = new StringBuilder();
            foreach(var issue in resultData.issues)
            {
                issueList.Append(string.Join("\n", issue.Select(p => "K=" + p.Key + ",L=" + p.Value)));
            }

            Assert.False(issueList.ToString().Contains("error"));
        }

        [Fact]
        public void TestNonDocumentTypePayload()
        {
            var resultData = canary.Models.Record.CheckGet(messageTypePayload, true);
            Assert.NotNull(resultData);

            StringBuilder issueList = new StringBuilder();
            foreach (var issue in resultData.issues)
            {
                issueList.Append(string.Join("\n", issue.Select(p => "K=" + p.Key + ",L=" + p.Value)));
            }

            Assert.True(issueList.ToString().Contains("error"));

        }
        [Fact]
        public void TestMissingTypePayload()
        {
            var resultData = canary.Models.Record.CheckGet(emptyTypePayload, true);
            Assert.NotNull(resultData);

            StringBuilder issueList = new StringBuilder();
            foreach (var issue in resultData.issues)
            {
                issueList.Append(string.Join("\n", issue.Select(p => "K=" + p.Key + ",L=" + p.Value)));
            }

            Assert.True(issueList.ToString().Contains("error"));
        }
        private string FixturePath(string filePath)
        {
            if (Path.IsPathRooted(filePath))
            {
                return filePath;
            }
            else
            {
                return Path.GetRelativePath(Directory.GetCurrentDirectory(), filePath);
            }
        }


    }

}
