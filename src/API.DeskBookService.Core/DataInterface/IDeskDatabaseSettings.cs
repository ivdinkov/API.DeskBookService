namespace API.DeskBookService.Core.DataInterfaces
{
    /// <summary>
    /// Mongodb settings
    /// </summary>
    public interface IDeskDatabaseSettings
    {
        /// <summary>
        /// Connection string
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// DB name
        /// </summary>
        string DatabaseName { get; set; }
    }
}
