using System;
using com.celigo.net.ServiceManager.SuiteTalk;
#if CLR_2_0
using Celigo.Linq;
#endif

namespace Celigo.ServiceManager.Utility.Search
{
    public partial class SearchColumnCustomFieldDictionary
    {
        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="value">The value.</param>
        public SearchColumnCustomField SetValue(string fieldId, bool? value)
        {
            return SetValue(fieldId, value, () => new SearchColumnBooleanCustomField()
                                            {
                                                internalId = fieldId,
                                                searchValue = value.GetValueOrDefault(),
                                                searchValueSpecified = value.HasValue,
                                            });
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="value">The value.</param>
        public SearchColumnCustomField SetValue(string fieldId, string value)
        {
            return SetValue(fieldId, value, () => new SearchColumnStringCustomField()
                                            {
                                                internalId = fieldId,
                                                searchValue = value,
                                            });
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="value">The value.</param>
        public SearchColumnCustomField SetValue(string fieldId, long? value)
        {
            return SetValue(fieldId, value, () => new SearchColumnLongCustomField()
                                            {
                                                internalId = fieldId,
                                                searchValue = value.GetValueOrDefault(),
                                                searchValueSpecified = value.HasValue,
                                            });
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="value">The value.</param>
        public SearchColumnCustomField SetValue(string fieldId, ListOrRecordRef value)
        {
            return SetValue(fieldId, value, () => new SearchColumnSelectCustomField()
                                            {
                                                internalId = fieldId,
                                                searchValue = value,
                                            });
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="value">The value.</param>
        public SearchColumnCustomField SetValue(string fieldId, ListOrRecordRef[] value)
        {
            return SetValue(fieldId, value, () => new SearchColumnMultiSelectCustomField()
                                            {
                                                internalId = fieldId,
                                                searchValue = value,
                                            });
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="value">The value.</param>
        public SearchColumnCustomField SetValue(string fieldId, DateTime? value)
        {
            return SetValue(fieldId, value, () => new SearchColumnDateCustomField()
                                            {
                                                internalId = fieldId,
                                                searchValue = value.GetValueOrDefault(),
                                                searchValueSpecified = value.HasValue,
                                            });
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="value">The value.</param>
        public SearchColumnCustomField SetValue(string fieldId, double? value)
        {
            return SetValue(fieldId, value, () => new SearchColumnDoubleCustomField()
                                            {
                                                internalId = fieldId,
                                                searchValue = value.GetValueOrDefault(),
                                                searchValueSpecified = value.HasValue,
                                            });
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="value">The value.</param>
        /// <param name="fieldBuilder">The builder function.</param>
        private SearchColumnCustomField SetValue<T>(string fieldId, T value, Func<SearchColumnCustomField> fieldBuilder)
        {
            SearchColumnCustomField retVal = null;
            if (value != null)
            {
                retVal = fieldBuilder();
                this[fieldId] = retVal;
            }
            else if (ContainsKey(fieldId))
            {
                retVal = this[fieldId];
                Remove(fieldId);
            }
            return retVal;
        }

        /// <summary>
        /// Gets the value of the specified LongCustomField.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public long? GetLong(string fieldId)
        {
            Func<SearchColumnLongCustomField, long?> extractor = f => f.searchValueSpecified ? (long?)f.searchValue : null;
            return ExtractFieldValue(this, fieldId, extractor);
        }

        /// <summary>
        /// Gets the value of the specified BooleanCustomField.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public bool? GetBoolean(string fieldId)
        {
            Func<SearchColumnBooleanCustomField, bool?> extactor = f => f.searchValueSpecified ? (bool?)f.searchValue : null;
            return ExtractFieldValue(this, fieldId, extactor);
        }

        /// <summary>
        /// Gets the value of the specified DoubleCustomField.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public double? GetDouble(string fieldId)
        {
            Func<SearchColumnDoubleCustomField, double?> extractor = f => f.searchValueSpecified ? (double?)f.searchValue : null;
            return ExtractFieldValue(this, fieldId, extractor);
        }

        /// <summary>
        /// Gets the value of the specified DateCustomField.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public DateTime? GetDate(string fieldId)
        {
            Func<SearchColumnDateCustomField, DateTime?> extractor = f => f.searchValueSpecified ? (DateTime?)f.searchValue : null;
            return ExtractFieldValue(this, fieldId, extractor);
        }

        /// <summary>
        /// Gets the value of the specified StringCustomField.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public string GetString(string fieldId)
        {
            Func<SearchColumnStringCustomField, string> extractor = f => f.searchValue;
            return ExtractFieldValue(this, fieldId, extractor);
        }

        /// <summary>
        /// Gets the value of the specified MultiSelectCustomField.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public ListOrRecordRef[] GetMultiSelectListValues(string fieldId)
        {
            Func<SearchColumnMultiSelectCustomField, ListOrRecordRef[]> extractor = f => f.searchValue;
            return ExtractFieldValue(this, fieldId, extractor);
        }

        private static RETTYPE ExtractFieldValue<FIELDTYPE, RETTYPE>(SearchColumnCustomFieldDictionary fields, 
                                                                    string fieldId, 
                                                                    Func<FIELDTYPE, RETTYPE> extractor)
                                                                    where FIELDTYPE: SearchColumnCustomField
        {
            SearchColumnCustomField field;
            if (!fields.TryGetValue(fieldId, out field))
            {
                return default(RETTYPE);
            }
            else if (field is FIELDTYPE)
            {
                return extractor((FIELDTYPE)field);
            }
            else if (field == null)
            {
                throw new InvalidOperationException(fieldId + " is set to null");
            }
            else
            {
                throw new InvalidOperationException(field.GetType().Name + " is not supported by this method");
            }
        }
    }
}
