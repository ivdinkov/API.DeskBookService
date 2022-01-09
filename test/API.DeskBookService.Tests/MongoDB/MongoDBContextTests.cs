using API.DeskBookService.Core.Domain;
using API.DeskBookService.Data.Context;
using API.DeskBookService.Data.DataSettings;
using API.DeskBookService.Data.Repository;
using FluentAssertions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace API.DeskBookService.Tests.MongoDB
{
    public class MongoDBContextTests
    {
        private Mock<IOptions<DeskDatabaseSettings>> _mockOptions;
        private Mock<IMongoDatabase> _mockDB;
        private Mock<IMongoClient> _mockClient;

        public MongoDBContextTests()
        {
            _mockOptions = new Mock<IOptions<DeskDatabaseSettings>>();
            _mockDB = new Mock<IMongoDatabase>();
            _mockClient = new Mock<IMongoClient>();
        }

        [Fact]
        public void MongoDB_Constructor_Success()
        {
            var settings = new DeskDatabaseSettings()
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "DeskBookDB"
            };

            _mockOptions.Setup(s => s.Value).Returns(settings);
            _mockClient.Setup(c => c.GetDatabase(_mockOptions.Object.Value.DatabaseName, null)).Returns(_mockDB.Object);

            //Act 
            var context = new DeskBookerDataContext(_mockOptions.Object);
        }

        [Fact]
        public void MongoDB_GetCollectionWithEmptyName_ShouldFail()
        {
            var settings = new DeskDatabaseSettings()
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "DeskBookDB"
            };

            _mockOptions.Setup(s => s.Value).Returns(settings);
            _mockClient.Setup(c => c.GetDatabase(_mockOptions.Object.Value.DatabaseName, null)).Returns(_mockDB.Object);

            //Act 
            var context = new DeskBookerDataContext(_mockOptions.Object);
            var deskCollection = context.GetCollection<Desk>("");
            var bookingCollection = context.GetCollection<DeskBooking>("");

            //Assert 
            deskCollection.Should().BeNull();
            bookingCollection.Should().BeNull();
        }

        [Fact]
        public void MongoDB_GetCollection_ShouldReturnCollection()
        {
            var settings = new DeskDatabaseSettings()
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "DeskBookDB"
            };

            _mockOptions.Setup(s => s.Value).Returns(settings);
            _mockClient.Setup(c => c.GetDatabase(_mockOptions.Object.Value.DatabaseName, null)).Returns(_mockDB.Object);

            //Act 
            var context = new DeskBookerDataContext(_mockOptions.Object);
            var deskCollection = context.GetCollection<Desk>(Collections.Desk.ToString());
            var bookingCollection = context.GetCollection<DeskBooking>(Collections.DeskBooking.ToString());

            //Assert 
            //deskCollection.Should().BeOfType<IMongoCollection<Desk>>();
            //bookingCollection.Should().BeOfType<IMongoCollection<DeskBooking>>();
            /*
             * 
             * Returns IMongoCollectionImpl instead of IMongoCollection
             * 
             */

            deskCollection.Should().NotBeNull();
            bookingCollection.Should().NotBeNull();
        }
    }
}
