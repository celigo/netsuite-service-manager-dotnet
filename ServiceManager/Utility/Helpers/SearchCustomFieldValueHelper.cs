using System;
using System.Collections.Generic;
using com.celigo.net.ServiceManager.SuiteTalk;

namespace com.celigo.net.ServiceManager.Utility.Helpers
{
    class SearchCustomFieldValueHelper
    {
        private readonly IDictionary<string, SearchCustomField> _parent;

        public SearchCustomFieldValueHelper(IDictionary<string, SearchCustomField> parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="value">The value.</param>
        public SearchCustomField SetValue(string fieldId, bool? value)
        {
            return SetValue(fieldId, value, () => new SearchBooleanCustomField()
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
        /// <param name="searchOp">The search op.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public SearchCustomField SetValue(string fieldId, SearchStringFieldOperator searchOp, string value)
        {
            return SetValue(fieldId, value, () => new SearchStringCustomField()
                                                {
                                                    internalId = fieldId,
                                                    searchValue = value,
                                                    @operator = searchOp,
                                                    operatorSpecified = true,
                                                });
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="searchOp">The search op.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public SearchCustomField SetValue(string fieldId, SearchLongFieldOperator searchOp, long? value)
        {
            return SetValue(fieldId, value, () => new SearchLongCustomField()
                                            {
                                                internalId = fieldId,
                                                searchValue = value.GetValueOrDefault(),
                                                searchValueSpecified = value.HasValue,
                                                operatorSpecified = true,
                                                @operator = searchOp,
                                            });
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="searchOp">The search op.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public SearchCustomField SetValue(string fieldId, SearchMultiSelectFieldOperator searchOp, ListOrRecordRef[] value)
        {
            return SetValue(fieldId, value, () => new SearchMultiSelectCustomField()
                                            {
                                                internalId = fieldId,
                                                @operator = searchOp,
                                                operatorSpecified = true,
                                                searchValue = value,
                                            });
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="searchOp">The search op.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public SearchCustomField SetValue(string fieldId, SearchEnumMultiSelectFieldOperator searchOp, string[] value)
        {
            return SetValue(fieldId, value, () => new SearchEnumMultiSelectCustomField()
                                                {
                                                    internalId = fieldId,
                                                    @operator = searchOp,
                                                    operatorSpecified = true,
                                                    searchValue = value,
                                                });
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="searchOp">The search operator.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public SearchCustomField SetValue(string fieldId, SearchDateFieldOperator searchOp, DateTime? value)
        {
            return SetValue(fieldId, value, () => new SearchDateCustomField()
                                                {
                                                    internalId = fieldId,
                                                    @operator = searchOp,
                                                    operatorSpecified = true,
                                                    searchValue = value.GetValueOrDefault(),
                                                    searchValueSpecified = true,
                                                });
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="searchOp">The search op.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public SearchCustomField SetValue(string fieldId, SearchDoubleFieldOperator searchOp, double? value)
        {
            return SetValue(fieldId, value, () => new SearchDoubleCustomField()
                                            {
                                                internalId = fieldId,
                                                @operator = searchOp,
                                                operatorSpecified = true,
                                                searchValue = value.Value,
                                                searchValueSpecified = true,
                                            });
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="value">The value.</param>
        /// <param name="fieldBuilder">The builder function.</param>
        private SearchCustomField SetValue<T>(string fieldId, T value, Func<SearchCustomField> fieldBuilder)
        {
            SearchCustomField retVal = null;
            if (value != null)
            {
                retVal = fieldBuilder();
                _parent[fieldId] = retVal;
            }
            else if (_parent.ContainsKey(fieldId))
            {
                retVal = _parent[fieldId];
                _parent.Remove(fieldId);
            }
            return retVal;
        }

        /// <summary>
        /// Gets the value of the specified LongSearchCustomField.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public long? GetLong(string fieldId)
        {
            Func<SearchLongCustomField, long?> extractor = f => f.searchValue;
            return ExtractFieldValue(_parent, fieldId, extractor);
        }

        /// <summary>
        /// Gets the value of the specified BooleanSearchCustomField.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public bool? GetBoolean(string fieldId)
        {
            Func<SearchBooleanCustomField, bool?> extactor = f => f.searchValue;
            return ExtractFieldValue(_parent, fieldId, extactor);
        }

        /// <summary>
        /// Gets the value of the specified DoubleSearchCustomField.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public double? GetDouble(string fieldId)
        {
            Func<SearchDoubleCustomField, double?> extractor = f => f.searchValue;
            return ExtractFieldValue(_parent, fieldId, extractor);
        }

        /// <summary>
        /// Gets the value of the specified DateSearchCustomField.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public DateTime? GetDate(string fieldId)
        {
            Func<SearchDateCustomField, DateTime?> extractor = f => f.searchValue;
            return ExtractFieldValue(_parent, fieldId, extractor);
        }

        /// <summary>
        /// Gets the value of the specified StringSearchCustomField.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public string GetString(string fieldId)
        {
            Func<SearchStringCustomField, string> extractor = f => f.searchValue;
            return ExtractFieldValue(_parent, fieldId, extractor);
        }

        /// <summary>
        /// Gets the value of the specified SelecctSearchCustomField.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public ListOrRecordRef[] GetListValue(string fieldId)
        {
            Func<SearchMultiSelectCustomField, ListOrRecordRef[]> extractor = f => f.searchValue;
            return ExtractFieldValue(_parent, fieldId, extractor);
        }

        /// <summary>
        /// Gets the value of the specified MultiSelectSearchCustomField.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public string[] GetEnumValues(string fieldId)
        {
            Func<SearchEnumMultiSelectCustomField, string[]> extractor = f => f.searchValue;
            return ExtractFieldValue(_parent, fieldId, extractor);
        }

        private static RETTYPE ExtractFieldValue<FIELDTYPE, RETTYPE>(IDictionary<string, SearchCustomField> fields,
                                                                    string fieldId,
                                                                    Func<FIELDTYPE, RETTYPE> extractor)
                                                                    where FIELDTYPE : SearchCustomField
        {
            SearchCustomField field;
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
