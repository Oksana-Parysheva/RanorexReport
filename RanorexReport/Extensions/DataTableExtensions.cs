using System.Data;
using System.Text;

namespace RanorexReport.Extensions
{
    public static class DataTableExtensions
    {
        public static string ToHtmlTable(this DataTable dt)
        {
            var sb = new StringBuilder();
            sb.Append("<table><tr>");
            foreach (DataColumn col in dt.Columns)
                sb.Append($"<th>{col.ColumnName}</th>");
            sb.Append("</tr>");
            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<tr>");
                foreach (var item in row.ItemArray)
                    sb.Append($"<td>{item}</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");

            var htmlTable = @"<!DOCTYPE html> <html> <head> <style> table {font - family: imakc mm., sans-serif;   border-collapse: collapse;   width: 100%; } td, th {border: 1px solid #dddddd;   text-align: left;   padding: 8px; } tr:nth-child(even) {background - color: #dddddd; } </style> </head> <body> " + sb.ToString() + " </body> </html>";
            return htmlTable;
        }
    }
}
