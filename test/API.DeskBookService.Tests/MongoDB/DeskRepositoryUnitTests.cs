using API.DeskBookService.Core.Domain;
using API.DeskBookService.Data.Context;
using API.DeskBookService.Data.Repository;
using MongoDB.Driver;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace API.DeskBookService.Tests.MongoDB
{
    public class DeskRepositoryUnitTests
    {
        private Mock<IMongoCollection<Desk>> _mockCollection;
        private Mock<DeskBookerDataContext> _mockContext;
        private Desk _desk;

        public DeskRepositoryUnitTests()
        {
            _desk = new Desk
            {
                Name = "Desk test",
                Description = "Desk description"
            };

            _mockCollection = new Mock<IMongoCollection<Desk>>();
            _mockContext = new Mock<DeskBookerDataContext>();
        }

        [Fact]
        public async Task SaveNewDek_ShouldSucceed()
        {
            //Arrange
            _mockCollection.Setup(op => op.InsertOneAsync(_desk, null, default(CancellationToken))).Returns(Task.CompletedTask);
            _mockContext.Setup(c => c.GetCollection<Desk>(typeof(Desk).Name)).Returns(_mockCollection.Object);
            var deskRepo = new DeskRepository(_mockContext.Object);

            //Act
            var result = await deskRepo.Save(_desk);

            //Assert 
            //Verify if InsertOneAsync is called once 
            _mockCollection.Verify(c => c.InsertOneAsync(_desk, null, default(CancellationToken)), Times.Once);
        }
    }
}
