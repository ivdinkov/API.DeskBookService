using System.ComponentModel.DataAnnotations;

namespace API.DeskBookService.Core.Domain
{
    /// <summary>
    /// DeskBase class
    /// </summary>
    public class DeskBase
    {
        /// <summary>
        /// The Name of the Desk
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The description of the Desk
        /// </summary>
        [Required]
        public string Description { get; set; }
    }
}
