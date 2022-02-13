using API.DeskBookService.Core.Contracts;
using API.DeskBookService.Core.Contracts.Requests;
using API.DeskBookService.Core.Domain;
using API.DeskBookService.Web;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace API.DeskBookService.Tests.IntegrationTests
{
    public class ControllerIntegrationTests
    {
        private TestServer _server;
        private HttpClient _client;
        private Mock<HttpRequest> _httpRequest;
        private Mock<HttpContext> _mockContext;
        private Mock<IHttpContextAccessor> _mockContextAccessor;

        public ControllerIntegrationTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .UseConfiguration(configuration);

            _client = new TestServer(builder).CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5001");
        }

        #region GET 

        [Fact]
        public async Task GetAllDesks_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/api/v1/desks");
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetAllDesksIncorrectUrl_ShouldReturnNotFound()
        {
            var response = await _client.GetAsync("");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetAllDesks_ShouldReturnCorrectCount()
        {
            var response = await _client.GetAsync("/api/v1/desks");
            var responseString = await response.Content.ReadAsStringAsync();
            var desks = JsonConvert.DeserializeObject<IEnumerable<Desk>>(responseString);
            desks.Count().Should().Be(6);
        }

        [Fact]
        public async Task GetIncorrectDeskId_ShouldReturnBadRequest()
        {
            var response = await _client.GetAsync("/api/v1/desks/1");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetDeskId_ShouldReturnOk()
        {
            var deskId = "61d98ee05466c8661d80ff65";
            var response = await _client.GetAsync("/api/v1/desks/" + deskId);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseString = await response.Content.ReadAsStringAsync();
            var desk = JsonConvert.DeserializeObject<Desk>(responseString);
            desk.Id.Should().Be(deskId);
        }

        #endregion

        #region POST

        [Fact]
        public async Task SaveDesk_ShouldFail()
        {
            var response = await _client.PostAsync("/api/v1/desks", null);
            response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
        }

        [Fact]
        public async Task SaveIncorrectObject_ShouldFail()
        {
            // Arrange
            var desk = new Desk();
            var content = JsonConvert.SerializeObject(desk);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/desks", stringContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should()
                .Contain("The Name field is required")
                .And.Contain("The Description field is required");
        }

        [Fact]
        public async Task SaveEmptyDeskSaveRequestObject_ShouldFail()
        {
            // Arrange
            var desk = new DeskSaveRequest();
            var content = JsonConvert.SerializeObject(desk);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/desks", stringContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should()
                .Contain("The Name field is required")
                .And.Contain("The Description field is required");
        }

        [Fact]
        public async Task SavePartialEmptyDeskSaveRequestObject_ShouldFail()
        {
            // Arrange
            var desk = new DeskSaveRequest {Name = "Test desk" };
            var content = JsonConvert.SerializeObject(desk);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/desks", stringContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("The Description field is required");
        }

        [Fact]
        public async Task SaveDeskSaveRequestObject_ShouldBeOk()
        {
            // Arrange
            var desk = new DeskSaveRequest {Name = "Test desk" , Description = "New description" };
            var content = JsonConvert.SerializeObject(desk);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/desks", stringContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseString = await response.Content.ReadAsStringAsync();
            var deskResponse = JsonConvert.DeserializeObject<Desk>(responseString);
            deskResponse.Name.Should().Be("Test desk");
            deskResponse.Description.Should().Be("New description");
        }

        #endregion

        #region PUT

        [Fact]
        public async Task UpdateDeskWrongDeskId_ShouldFail()
        {
            //Arrange
            var deskId = "1";
            var newDeskObject = new DeskUpdateRequest { Description = "New updated desciption", Name = "New Desk name" };
            var content = JsonConvert.SerializeObject(newDeskObject);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PutAsync("/api/v1/desks/" + deskId,stringContent);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain($"Unable to update Desk id:{deskId}");
        }

        [Fact]
        public async Task UpdateDeskWithEmptyDescription_ShouldFail()
        {
            //Arrange
            var deskId = "61d98ee05466c8661d80ff65";
            var newDeskObject = new DeskUpdateRequest { Description = "", Name = "New Desk name" };
            var content = JsonConvert.SerializeObject(newDeskObject);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PutAsync("/api/v1/desks/" + deskId,stringContent);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should()
                .Contain("The Description field is required");
        }

        [Fact]
        public async Task UpdateDeskWithEmptyName_ShouldFail()
        {
            //Arrange
            var deskId = "61d98ee05466c8661d80ff65";
            var newDeskObject = new DeskUpdateRequest { Description = "New Updated description", Name = "" };
            var content = JsonConvert.SerializeObject(newDeskObject);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PutAsync("/api/v1/desks/" + deskId,stringContent);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should()
                .Contain("The Name field is required");
        }

        [Fact]
        public async Task UpdateDeskWithEmptyDescriptionName_ShouldFail()
        {
            //Arrange
            var deskId = "61d98ee05466c8661d80ff65";
            var newDeskObject = new DeskUpdateRequest { Description = "", Name = "" };
            var content = JsonConvert.SerializeObject(newDeskObject);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PutAsync("/api/v1/desks/" + deskId,stringContent);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should()
                .Contain("The Description field is required")
                .And.Contain("The Name field is required");
        }

        [Fact]
        public async Task UpdateDes_ShouldReturnOk()
        {
            //Arrange
            var deskId = "61d98ee05466c8661d80ff65";
            var newDeskObject = new DeskUpdateRequest { Description = "New updated desciption", Name = "Desk 234" };
            var content = JsonConvert.SerializeObject(newDeskObject);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PutAsync("/api/v1/desks/" + deskId,stringContent);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should()
                .Contain($"Desk id:{deskId} successfully updated");
        }

        #endregion

        #region DELETE

        [Fact]
        public async Task DeleteDeskWrongDeskId_ShouldFail()
        {
            //Arrange
            var deskId = "1";

            //Act
            var response = await _client.DeleteAsync("/api/v1/desks/" + deskId);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain($"Unable to delete Desk id:{deskId}");
        }

        [Fact]
        public async Task DeleteBookedDesk_ShouldFail()
        {
            //Arrange
            var deskId = "61d98ee05466c8661d80ff65";

            //Act
            var response = await _client.DeleteAsync("/api/v1/desks/" + deskId);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain($"Unable to delete Desk id:{deskId}");
        }

        [Fact]
        public async Task DeleteDesk_ShouldReturnOk()
        {
            //Arrange
            var desk = new DeskSaveRequest { Name = "Test desk", Description = "New description" };
            var content = JsonConvert.SerializeObject(desk);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/v1/desks", stringContent);
            var responseString = await response.Content.ReadAsStringAsync();
            var deskResponse = JsonConvert.DeserializeObject<Desk>(responseString);
            var deskId = deskResponse.Id;

            //Act
            var deleteResponse = await _client.DeleteAsync("/api/v1/desks/" + deskId);

            //Assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var deleteResponseString = await deleteResponse.Content.ReadAsStringAsync();
            deleteResponseString.Should().Contain($"Desk id:{deskId} successfully deleted");
        }

        #endregion
    }
}
