namespace API.DeskBookService.Core.Contracts
{
    /// <summary>
    /// New desk request
    /// </summary>
    public class DeskSaveRequest
    {
        /// <summary>
        /// The Name of the Desk
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the Desk
        /// </summary>
        public string Description { get; set; }
    }
}