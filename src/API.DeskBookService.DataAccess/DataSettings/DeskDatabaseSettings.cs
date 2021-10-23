namespace API.DeskBookService.DataAccess
{
    public class DeskDatabaseSettings : IDeskDatabaseSettings
    {
        public string DeskCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
