namespace API.DeskBookService.Core.Contracts.Responses
{
    /// <summary>
    /// Generic response
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Response code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Response message
        /// </summary>
        public string Message { get; set; }
    }
}
