using System;
using com.celigo.net.ServiceManager.SuiteTalk;
#if CLR_2_0
using Celigo.Linq;
#endif

namespace com.celigo.net.ServiceManager.Utility
{
    public partial class CustomFieldDictionary
    {
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
        /// Gets the value of the specified LongCustomFieldRef.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public long? GetLong(string fieldId)
        {
            Func<LongCustomFieldRef, long?> extractor = f => f.value;
            return ExtractFieldValue(this, fieldId, extractor);
        }

        /// <summary>
        /// Gets the value of the specified BooleanCustomFieldRef.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public bool? GetBoolean(string fieldId)
        {
            Func<BooleanCustomFieldRef, bool?> extactor = f => f.value;
            return ExtractFieldValue(this, fieldId, extactor);
        }

        /// <summary>
        /// Gets the value of the specified DoubleCustomFieldRef.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public double? GetDouble(string fieldId)
        {
            Func<DoubleCustomFieldRef, double?> extractor = f => f.value;
            return ExtractFieldValue(this, fieldId, extractor);
        }

        /// <summary>
        /// Gets the value of the specified DateCustomFieldRef.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public DateTime? GetDate(string fieldId)
        {
            Func<DateCustomFieldRef, DateTime?> extractor = f => f.value;
            return ExtractFieldValue(this, fieldId, extractor);
        }

        /// <summary>
        /// Gets the value of the specified StringCustomFieldRef.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public string GetString(string fieldId)
        {
            Func<StringCustomFieldRef, string> extractor = f => f.value;
            return ExtractFieldValue(this, fieldId, extractor);
        }

        /// <summary>
        /// Gets the value of the specified SelecctCustomFieldRef.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public ListOrRecordRef GetListValue(string fieldId)
        {
            Func<SelectCustomFieldRef, ListOrRecordRef> extractor = f => f.value;
            return ExtractFieldValue(this, fieldId, extractor);
        }

        /// <summary>
        /// Gets the value of the specified MultiSelectCustomFieldRef.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <returns>The value of the specified custom field or <c>null</c> if the field was not found.</returns>
        public ListOrRecordRef[] GetMultiSelectListValues(string fieldId)
        {
            Func<MultiSelectCustomFieldRef, ListOrRecordRef[]> extractor = f => f.value;
            return ExtractFieldValue(this, fieldId, extractor);
        }

        private static RETTYPE ExtractFieldValue<FIELDTYPE, RETTYPE>(CustomFieldDictionary fields, 
                                                                    string fieldId, 
                                                                    Func<FIELDTYPE, RETTYPE> extractor)
                                                                    where FIELDTYPE: CustomFieldRef
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
