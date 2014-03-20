using System;
using System.Collections.Generic;
using com.celigo.net.ServiceManager.SuiteTalk;

namespace com.celigo.net.ServiceManager.Utility.Helpers
{
    class CustomFieldValueHelper
    {
        private readonly IDictionary<string, CustomFieldRef> _parent;

        public CustomFieldValueHelper(IDictionary<string, CustomFieldRef> parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="value">The value.</param>
        public CustomFieldRef SetValue(string fieldId, bool? value)
        {
            return SetValue(fieldId, value, () => new BooleanCustomFieldRef()
            {
                internalId = fieldId,
                value = value.Value,
            });
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="value">The value.</param>
        public CustomFieldRef SetValue(string fieldId, string value)
        {
            return SetValue(fieldId, value, () => new StringCustomFieldRef()
            {
                internalId = fieldId,
                value = value,
            });
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="value">The value.</param>
        public CustomFieldRef SetValue(string fieldId, long? value)
        {
            return SetValue(fieldId, value, () => new LongCustomFieldRef()
            {
                internalId = fieldId,
                value = value.Value,
            });
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="value">The value.</param>
        public CustomFieldRef SetValue(string fieldId, ListOrRecordRef value)
        {
            return SetValue(fieldId, value, () => new SelectCustomFieldRef()
            {
                internalId = fieldId,
                value = value,
            });
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="value">The value.</param>
        public CustomFieldRef SetValue(string fieldId, ListOrRecordRef[] value)
        {
            return SetValue(fieldId, value, () => new MultiSelectCustomFieldRef()
            {
                internalId = fieldId,
                value = value,
            });
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="value">The value.</param>
        public CustomFieldRef SetValue(string fieldId, DateTime? value)
        {
            return SetValue(fieldId, value, () => new DateCustomFieldRef()
            {
                internalId = fieldId,
                value = value.Value,
            });
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="value">The value.</param>
        public CustomFieldRef SetValue(string fieldId, double? value)
        {
            return SetValue(fieldId, value, () => new DoubleCustomFieldRef()
            {
                internalId = fieldId,
                value = value.Value,
            });
        }

        /// <summary>
        /// Sets the value of the specified custom field.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="value">The value.</param>
        /// <param name="fieldBuilder">The builder function.</param>
        private CustomFieldRef SetValue<T>(string fieldId, T value, Func<CustomFieldRef> fieldBuilder)
        {
            CustomFieldRef retVal = null;
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
        /// Gets the value of the specified LongCustomFieldRef.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public long? GetLong(string fieldId)
        {
            Func<LongCustomFieldRef, long?> extractor = f => f.value;
            return ExtractFieldValue(_parent, fieldId, extractor);
        }

        /// <summary>
        /// Gets the value of the specified BooleanCustomFieldRef.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public bool? GetBoolean(string fieldId)
        {
            Func<BooleanCustomFieldRef, bool?> extactor = f => f.value;
            return ExtractFieldValue(_parent, fieldId, extactor);
        }

        /// <summary>
        /// Gets the value of the specified DoubleCustomFieldRef.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public double? GetDouble(string fieldId)
        {
            Func<DoubleCustomFieldRef, double?> extractor = f => f.value;
            return ExtractFieldValue(_parent, fieldId, extractor);
        }

        /// <summary>
        /// Gets the value of the specified DateCustomFieldRef.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public DateTime? GetDate(string fieldId)
        {
            Func<DateCustomFieldRef, DateTime?> extractor = f => f.value;
            return ExtractFieldValue(_parent, fieldId, extractor);
        }

        /// <summary>
        /// Gets the value of the specified StringCustomFieldRef.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public string GetString(string fieldId)
        {
            Func<StringCustomFieldRef, string> extractor = f => f.value;
            return ExtractFieldValue(_parent, fieldId, extractor);
        }

        /// <summary>
        /// Gets the value of the specified SelecctCustomFieldRef.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public ListOrRecordRef GetListValue(string fieldId)
        {
            Func<SelectCustomFieldRef, ListOrRecordRef> extractor = f => f.value;
            return ExtractFieldValue(_parent, fieldId, extractor);
        }

        /// <summary>
        /// Gets the value of the specified MultiSelectCustomFieldRef.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public ListOrRecordRef[] GetMultiSelectListValues(string fieldId)
        {
            Func<MultiSelectCustomFieldRef, ListOrRecordRef[]> extractor = f => f.value;
            return ExtractFieldValue(_parent, fieldId, extractor);
        }

        private static RETTYPE ExtractFieldValue<FIELDTYPE, RETTYPE>(IDictionary<string, CustomFieldRef> fields,
                                                                    string fieldId,
                                                                    Func<FIELDTYPE, RETTYPE> extractor)
                                                                    where FIELDTYPE : CustomFieldRef
        {
            CustomFieldRef field;
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
