using System;
using System.Linq.Expressions;
using com.celigo.net.ServiceManager.SuiteTalk;

namespace Celigo.ServiceManager.Utility.Search
{
    public static partial class SearchRowExtensions
    {
        public static string GetInternalId<T>(this T basic, Expression<Func<T, SearchColumnSelectField[]>> getter) where T : ISearchRowBasic
        {
            if (basic == null)
                return null;

            var recRef = basic.Get(getter);
            if (recRef == null)
                return null;
            else
                return recRef.internalId;
        }

        //public static RecordRef Get<T>(this T basic, Expression<Func<T, SearchColumnSelectField[]>> getter) where T : ISearchRowBasic
        //{
        //    if (basic == null)
        //        return null;

        //    var value = getter.Compile().Invoke(basic);
        //    if (value != null && value.Length > 0)
        //        return value[0].searchValue;
        //    else
        //        return null;
        //}

        //public static string Get<T>(this T basic, Expression<Func<T, SearchColumnEnumSelectField[]>> getter) where T : ISearchRowBasic
        //{
        //    if (basic == null)
        //        return null;

        //    var value = getter.Compile().Invoke(basic);
        //    if (value != null && value.Length > 0)
        //        return value[0].searchValue;
        //    else
        //        return null;
        //}


        //// TODO: Code Gen the rest
        //public static string Get<T>(this T basic, Expression<Func<T, SearchColumnStringField[]>> getter) where T : ISearchRowBasic
        //{
        //    if (basic == null)
        //        return null;

        //    var value = getter.Compile().Invoke(basic);
        //    if (value != null && value.Length > 0)
        //        return value[0].searchValue;
        //    else
        //        return null;
        //}
    }
}
