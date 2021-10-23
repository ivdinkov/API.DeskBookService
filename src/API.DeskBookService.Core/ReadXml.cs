using System;
using System.IO;
using System.Reflection;

namespace API.DeskBookService.Core
{
    /// <summary>
    /// Class wih a static methd to read generated Xml file
    /// </summary>
    public class ReadXml
    {
        /// <summary>
        /// Return the path to the XML file
        /// </summary>
        /// <returns></returns>
        public static string GetXml()
        {
            var filePath = $"{Assembly.GetExecutingAssembly().GetName().Name}.Xml";
            filePath = Path.Combine(AppContext.BaseDirectory, filePath);
            return filePath;
        }
    }
}
