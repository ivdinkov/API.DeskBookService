using API.DeskBookService.Core.DataInterfaces;

namespace API.DeskBookService.Data.DataSettings
{
    public class DeskDatabaseSettings : IDeskDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
