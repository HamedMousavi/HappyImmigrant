using System;
using System.Data;

namespace DatabaseManager
{
    public static class AdoConverter
    {

        public static T Read<T>(IDataReader reader, string fieldName, T defaultValue) where T : IConvertible
        {
            try
            {
                if (reader != null)
                {
                    var value = reader[fieldName];

                    if (value != null && value != DBNull.Value)
                    {
                        var result = (T)Convert.ChangeType(value, typeof(T));
                        if (result is string)
                        {
                            var txt = result as string;
                            if (!string.IsNullOrWhiteSpace(txt))
                            {
                                result = (T)Convert.ChangeType(txt.Trim(), typeof(T));
                            }
                        }

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception:{0}", ex);
            }

            return defaultValue;
        }


        internal static T ReadResult<T>(object value, T defaultValue) where T : IConvertible
        {
            try
            {
                if (value != null && value != DBNull.Value)
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception:{0}", ex);
                throw;
            }

            return defaultValue;
        }
    }
}
