namespace API.DeskBookService.Core.DataInterfaces
{
    public interface IDeskDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
