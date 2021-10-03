namespace API.DeskBookService.DataAccess
{
    public interface IDeskDatabaseSettings
    {
        string DeskCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
