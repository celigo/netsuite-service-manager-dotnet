using System;
using com.celigo.net.ServiceManager.SuiteTalk;

namespace Celigo.ServiceManager.Utility.Search
{
    public static class SearchColumnCustomFieldExtensions
    {
        public static object GetValue(this SearchColumnCustomField customField)
        {
            object value = null;
            if (customField is SearchColumnBooleanCustomField)
                value = ((SearchColumnBooleanCustomField)customField).GetValue();
            else if (customField is SearchColumnDateCustomField)
                value = ((SearchColumnDateCustomField)customField).GetValue();
            else if (customField is SearchColumnDoubleCustomField)
                value = ((SearchColumnDoubleCustomField)customField).GetValue();
            else if (customField is SearchColumnEnumMultiSelectCustomField)
                value = ((SearchColumnEnumMultiSelectCustomField)customField).GetValue();
            else if (customField is SearchColumnLongCustomField)
                value = ((SearchColumnLongCustomField)customField).GetValue();
            else if (customField is SearchColumnMultiSelectCustomField)
                value = ((SearchColumnMultiSelectCustomField)customField).GetValue();
            else if (customField is SearchColumnSelectCustomField)
                value = ((SearchColumnSelectCustomField)customField).GetValue();
            else if (customField is SearchColumnStringCustomField)
                value = ((SearchColumnStringCustomField)customField).GetValue();

            return value;
        }

        public static bool GetValue(this SearchColumnBooleanCustomField customField)
        {
            if (customField != null && customField.searchValueSpecified)
                return customField.searchValue;
            else
                return default(bool);
        }

        public static DateTime GetValue(this SearchColumnDateCustomField customField)
        {
            if (customField != null && customField.searchValueSpecified)
                return customField.searchValue;
            else
                return default(DateTime);
        }

        public static double GetValue(this SearchColumnDoubleCustomField customField)
        {
            if (customField != null && customField.searchValueSpecified)
                return customField.searchValue;
            else
                return default(double);
        }

        public static string[] GetValue(this SearchColumnEnumMultiSelectCustomField customField)
        {
            if (customField != null)
                return customField.searchValue;
            else
                return null;
        }

        public static long GetValue(this SearchColumnLongCustomField customField)
        {
            if (customField != null && customField.searchValueSpecified)
                return customField.searchValue;
            else
                return default(long);
        }

        public static ListOrRecordRef[] GetValue(this SearchColumnMultiSelectCustomField customField)
        {
            if (customField != null)
                return customField.searchValue;
            else
                return null;
        }

        public static ListOrRecordRef GetValue(this SearchColumnSelectCustomField customField)
        {
            if (customField != null)
                return customField.searchValue;
            else
                return null;
        }

        public static string GetValue(this SearchColumnStringCustomField customField)
        {
            if (customField != null)
                return customField.searchValue;
            else
                return null;
        }
    }
}
