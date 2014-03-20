using com.celigo.net.ServiceManager.SuiteTalk;

namespace Celigo.ServiceManager.Utility.Customizations
{
    public static partial class CustomFieldRefExtensions
    {
        public static void SetValue(this StringCustomFieldRef field, object value)
        {
            if (field != null && value == null)
            {
                field.value = null;
            }
            else if (field != null)
            {
                field.value = value.ToString();
            }
        }

        public static void SetValue(this MultiSelectCustomFieldRef field, object value)
        {
            if (field == null)
                return;

            if (value == null)
                field.value = null;
            else if (value is string)
                field.value = new[] { new ListOrRecordRef() { internalId = value.ToString() } };
            else if (value is RecordRef)
                field.value = new[] { new ListOrRecordRef() { internalId = ((RecordRef)value).internalId } };
            else if (value is ListOrRecordRef)
                field.value = new[] { (ListOrRecordRef)value };
            else if (value is ListOrRecordRef[])
                field.value = (ListOrRecordRef[])value;
            else if (value is RecordRef[])
            {
                var refs = (RecordRef[])value;
                field.value = new ListOrRecordRef[refs.Length];
                for (int i = 0; i < refs.Length; i++)
                {
                    field.value[i] = new ListOrRecordRef() { internalId = refs[i].internalId };
                }
            }
        }
    }
}
