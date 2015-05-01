
namespace GoogleFu
{
    public class GoogleFuMimeType
    {

        private static System.Collections.Generic.IDictionary<string, string> _mappings = new System.Collections.Generic.Dictionary<string, string>(System.StringComparer.InvariantCultureIgnoreCase)
    {
        {".csv", "text/csv"},
        {".ods", "application/oleobject"},
        {".tsv", "text/tab-separated-values"},
        {".txt", "text/plain"},
        {".xls", "application/vnd.ms-excel"},
        {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
    };

        public static string GetMimeType(string filePath)
        {
            if (filePath == null)
            {
                throw new System.ArgumentNullException("Null File Path");
            }

            string extension = filePath.Substring( filePath.LastIndexOf('.') );

            string mime;
            return _mappings.TryGetValue(extension, out mime) ? mime : "application/octet-stream";
        }
    }
}
