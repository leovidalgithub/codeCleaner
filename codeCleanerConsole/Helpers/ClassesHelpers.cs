using System.Data;
using System.Reflection;

namespace codeCleanerConsole.Helpers
{
    public static class ClassesHelpers
    {
        public static void AddContentWithSeparator(object target, string propertyName, DataTable DT)
        {
            PropertyInfo prop = target.GetType().GetProperty(propertyName);
            string result = "";
            foreach (DataRow DR in DT.Rows)
            {
                if (!string.IsNullOrEmpty(result))
                    result += "?";
                result += DR[propertyName].ToString().Trim();
            }
            prop.SetValue(target, result);
        }
    }
}
