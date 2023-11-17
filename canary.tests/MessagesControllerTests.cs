using Xunit;
using canary.Controllers;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using VRDR;
using canary.Models;

namespace canary.tests
{
  public class MessagesControllerTests
  {
    private MessagesController _messagesController;
    public MessagesControllerTests()
    {
      _messagesController = new MessagesController();
    }

    [Fact]
    public void NewMessagePostCreatesNewCodingResponseMessageObject()
    {
      var stream = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(FixturePath("fixtures/json/CauseOfDeathCodingMessage.json"))));
      var httpContext = new DefaultHttpContext()
      {
        Request = { Body = stream, ContentLength = stream.Length }
      };
      _messagesController.ControllerContext.HttpContext = httpContext;
      var response = _messagesController.NewMessagePost();
      var message = response.Result.message;
      var issues = response.Result.issues;
      Assert.Equal(new List<Dictionary<string, string>>(), issues);
      Assert.IsType<canary.Models.Message>(message);
      Assert.IsAssignableFrom<CauseOfDeathCodingMessage>(message.GetMessage());
    }

    [Fact]
    public void BadNewMessagePostReturnsIssues()
    {
      var stream = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(FixturePath("fixtures/json/EmptyMessage.json"))));
      var httpContext = new DefaultHttpContext()
      {
        Request = { Body = stream, ContentLength = stream.Length }
      };
      _messagesController.ControllerContext.HttpContext = httpContext;
      var response = _messagesController.NewMessagePost();
      var message = response.Result.message;
      var issues = response.Result.issues[0];
      Assert.Equal(2, issues.Count);
      Assert.Contains(new KeyValuePair<string, string>("severity", "error"), issues);
      Assert.Contains(new KeyValuePair<string, string>("message", "Failed to find a Bundle Entry containing a Resource of type MessageHeader. Error occurred at VRDR.BaseMessage in function Parse."), issues);
    }

    [Fact]
    public void TestUnknownDescriptionMessageProperties()
    {
            string desc = Message.GetDescriptionFor("AUTOP");

            Assert.Equal("Was Autopsy performed", desc, true, true, true, true);
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
