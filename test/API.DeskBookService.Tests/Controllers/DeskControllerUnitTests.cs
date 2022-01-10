using API.DeskBookService.Core.Contracts;
using API.DeskBookService.Core.Contracts.Requests;
using API.DeskBookService.Core.Domain;
using API.DeskBookService.Core.Services;
using API.DeskBookService.Web.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace API.DeskBookService.Tests.Controllers
{
    public class DeskControllerUnitTests
    {
        private Mock<IDeskService> _mockService;
        private List<Desk> _deskList;
        private Mock<HttpContext> _mockContext;
        private Mock<IHttpContextAccessor> _mockContextAccessor;
        private Mock<HttpRequest> _httpRequest;

        public DeskControllerUnitTests()
        {
            _mockService = new Mock<IDeskService>();
            _deskList = new List<Desk> {
                new Desk{ Id="617488496c2be155553d0763",Name="Desk 1",Description="Corner desk"},
                new Desk{ Id="617542f3a04d6f73f239a706",Name="Desk 2",Description="New desk"},
                new Desk{ Id="617542f3a04d6f73f239a777",Name="Desk 3",Description="Another new desk"}
            };
            _httpRequest = new Mock<HttpRequest>();
            _httpRequest.Setup(r => r.Scheme).Returns("https");
            _httpRequest.Setup(r => r.Host).Returns(new HostString("localhost"));
            _mockContext = new Mock<HttpContext>();
            _mockContext.Setup(r => r.Request).Returns(_httpRequest.Object);
            _mockContextAccessor = new Mock<IHttpContextAccessor>();
            _mockContextAccessor.Setup(mock => mock.HttpContext).Returns(_mockContext.Object);
        }

        [Fact]
        public async Task GetAllDesks_ShouldReturnAllDesks()
        {
            _mockService.Setup(s => s.GetAll()).ReturnsAsync(_deskList.Select(x => x));
            var _controller = new DesksController(_mockService.Object, _mockContextAccessor.Object);

            //Act
            var result = await _controller.GetDesksAsync();

            //Assert
            var resultOk = result.Should().BeOfType<OkObjectResult>().Subject;
            var desks = resultOk.Value.Should().BeAssignableTo<IEnumerable<Desk>>().Subject;
            desks.Count().Should().Be(3);
        }

        [Fact]
        public async Task GetDeskById_ShouldReturnDesk()
        {
            //Arrange
            var _deskId = "617488496c2be155553d0763";
            _mockService.Setup(repo => repo.Get(It.IsAny<string>())).ReturnsAsync((string i) =>
                _deskList.SingleOrDefault(x => x.Id == i));
            var _controller = new DesksController(_mockService.Object, _mockContextAccessor.Object);

            //Act
            var result = await _controller.GetDeskAsync(_deskId);

            //Assert
            var resultOk = result.Should().BeOfType<OkObjectResult>().Subject;
            var desk = resultOk.Value.Should().BeOfType<Desk>().Subject;
            desk.Id.Should().Be(_deskId);
        }

        [Fact]
        public async Task GetDeskById_ShouldReturnNorFound()
        {
            //Arrange
            var _deskId = "1";
            _mockService.Setup(repo => repo.Get(It.IsAny<string>())).ReturnsAsync((string i) =>
                _deskList.SingleOrDefault(x => x.Id == i));
            var _controller = new DesksController(_mockService.Object, _mockContextAccessor.Object);

            //Act
            var result = await _controller.GetDeskAsync(_deskId);

            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            //result.Should().BeOfType<Response>();
        }

        [Fact]
        public async Task SaveDesk_ShouldReturnOk()
        {
            var _desk = new Desk
            {
                Name = "Desk 11",
                Description = "New description"
            };            
            
            var _desk2 = new DeskSaveRequest
            {
                Name = "Desk 11",
                Description = "New description"
            };

            _mockService.Setup(repo => repo.Save(It.IsAny<Desk>())).ReturnsAsync(_desk);
            var _controller = new DesksController(_mockService.Object, _mockContextAccessor.Object);

            //Act
            var result = await _controller.SaveDeskAsync(_desk2);

            //Assert
            result.Should().BeOfType<CreatedResult>();
            //result.Should().BeOfType<Response>();

        }

        [Fact]
        public async Task UpdateDesk_ShouldUpdateOk()
        {
            //Arrange
            var _deskId = "617488496c2be155553d0763";
            var _deskIn = new DeskUpdateRequest
            {
                Name = "Desk 11",
                Description = "New description"
            };

            _mockService.Setup(repo => repo.Update(It.IsAny<string>(), _deskIn)).ReturnsAsync(true);
            var _controller = new DesksController(_mockService.Object, _mockContextAccessor.Object);

            //Act
            var result = await _controller.UpdateDeskAsync(_deskId, _deskIn);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task UpdateDesk_ShouldReturnBadRequest()
        {
            //Arrange
            var _deskId = "1";
            var _deskIn = new DeskUpdateRequest
            {
                Name = "Desk 11",
                Description = "New description"
            };

            _mockService.Setup(repo => repo.Update(It.IsAny<string>(), _deskIn)).ReturnsAsync(false);
            var _controller = new DesksController(_mockService.Object, _mockContextAccessor.Object);

            //Act
            var result = await _controller.UpdateDeskAsync(_deskId, _deskIn);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            //result.Should().BeOfType<Response>();
        }

        [Fact]
        public async Task DeleteDesk_ShouldDeleteOk()
        {
            //Arrange
            var _deskId = "617488496c2be155553d0763";
            _mockService.Setup(repo => repo.Remove(It.IsAny<string>()))
                .ReturnsAsync(_deskList.RemoveAll(r => r.Id.Equals(_deskId)) == 1);
            var _controller = new DesksController(_mockService.Object, _mockContextAccessor.Object);

            //Act
            var result = await _controller.DeleteDeskAsync(_deskId);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            //result.Should().BeOfType<Response>();
        }

        [Fact]
        public async Task DeleteDesk_ShouldReturnBadRequest()
        {
            //Arrange
            var _deskId = "1";
            _mockService.Setup(repo => repo.Remove(It.IsAny<string>()))
                .ReturnsAsync(_deskList.RemoveAll(r => r.Id.Equals(_deskId)) == 1);
            var _controller = new DesksController(_mockService.Object, _mockContextAccessor.Object);

            //Act
            var result = await _controller.DeleteDeskAsync(_deskId);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            //result.Should().BeOfType<Response>();
        }

    }
}
