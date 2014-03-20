
namespace com.celigo.net.ServiceManager.SuiteTalk
{
    public interface ICustomFieldDefinition
    {
        CustomizationFieldType fieldType { set; get; }

        string label { set; get; }

        string internalId { get; set; }
    }
}
