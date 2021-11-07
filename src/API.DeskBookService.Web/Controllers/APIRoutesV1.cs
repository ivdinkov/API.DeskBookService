namespace API.DeskBookService.Web.Controllers
{
    /// <summary>
    /// Class containing routes for different API versions
    /// </summary>
    public static class APIRoutesV1
    {
        /// <summary>
        /// Desk routes class 
        /// </summary>
        public static class Desks
        {
            /// <summary>
            /// Get all desks async
            /// </summary>
            public const string GetDesksAsync = "api/v1/desks";

            /// <summary>
            /// Get desk by Id
            /// </summary>
            public const string GetDeskAsync = "api/v1/desks/{id}";

            /// <summary>
            /// Post new desk
            /// </summary>
            public const string SaveDeskAsync = "api/v1/desks";

            /// <summary>
            /// Update desk
            /// </summary>
            public const string UpdateDeskAsync = "api/v1/desks/{id}";

            /// <summary>
            /// Delete desk by Id
            /// </summary>
            public const string DeleteDeskAsync = "api/v1/desks/{id}";
        }

        /// <summary>
        /// Booking class routes
        /// </summary>
        public static class Bookings
        {
            /// <summary>
            /// Get all bookings async
            /// </summary>
            public const string GetBokingsAsync = "api/v1/bookings";            
            
            /// <summary>
            /// Get desk by Id
            /// </summary>
            public const string GetBokingAsync = "api/v1/bookings/{id}";

            /// <summary>
            /// Post new desk
            /// </summary>
            public const string BookAsync = "api/v1/bookings";

            /// <summary>
            /// Update desk
            /// </summary>
            public const string UpdateBokingAsync = "api/v1/bookings/{id}";

            /// <summary>
            /// Delete desk by Id
            /// </summary>
            public const string DeleteBokingAsync = "api/v1/bookings/{id}";
        }
    }
}
