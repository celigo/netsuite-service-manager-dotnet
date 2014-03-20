using System.Collections.Generic;
using com.celigo.net.ServiceManager.SuiteTalk;
#if CLR_2_0
using Celigo.Linq;
#else
using System.Linq;
#endif

namespace Celigo.ServiceManager.Utility.Search
{
    /// <summary>
    /// A dictionary of Custom Fields indexed by the Internal IDs of the fields.
    /// </summary>
    public partial class SearchColumnCustomFieldDictionary : IDictionary<string, SearchColumnCustomField>
    {
        private readonly SearchColumnCustomField[] _customFields;
        private readonly Dictionary<string, SearchColumnCustomField> _dictionary;
        private bool _requiresInit;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchColumnCustomFieldDictionary"/> class.
        /// </summary>
        public SearchColumnCustomFieldDictionary() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchColumnCustomFieldDictionary"/> class.
        /// </summary>
        /// <param name="customFields">The custom fields.</param>
        public SearchColumnCustomFieldDictionary(SearchColumnCustomField[] customFields)
        {
            _customFields = customFields;
            if (null != customFields)
            {
                _dictionary = new Dictionary<string, SearchColumnCustomField>(customFields.Length);
                _requiresInit = true;
            }
            else
            {
                _dictionary = new Dictionary<string, SearchColumnCustomField>();
                _requiresInit = false;
            }
        }

        private void Initialize()
        {
            if (_requiresInit)
                lock (_dictionary)
                    if (_requiresInit)
                    {
                        for (int i = _customFields.Length - 1; i >= 0; i--)
                            if (_customFields[i] != null)
                            {
                                _dictionary[_customFields[i].GetInternalId()] = _customFields[i];
                            }
                        _requiresInit = false;
                    }
        }

        /// <summary>
        /// Returns an array of Custom Fields contained in the dictionary.
        /// </summary>
        /// <returns>An array of Custom Fields</returns>
        public SearchColumnCustomField[] ToArray()
        {
            if (_requiresInit)
                return _customFields;
            else
            {
#if CLR_2_0
                return Extensions.ToArray(_dictionary);
#else
                return _dictionary.Values.ToArray();
#endif
            }
        }

        /// <summary>
        /// Gets the field with the given Id.
        /// </summary>
        /// <typeparam name="T">The Type of the custom field.</typeparam>
        /// <param name="fieldId">The field id.</param>
        /// <returns></returns>
        public T GetCustomField<T>(string fieldId) where T: SearchColumnCustomField
        {
            return this[fieldId] as T;
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, SearchColumnCustomField value)
        {
            _dictionary[key] = value;
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	<c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(string key)
        {
            if (_requiresInit)
                Initialize();
            return _dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public ICollection<string> Keys
        {
            get 
            {
                if (_requiresInit)
                    Initialize();
                return _dictionary.Keys;
            }
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            if (_requiresInit)
                Initialize();
            return _dictionary.Remove(key);
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool TryGetValue(string key, out SearchColumnCustomField value)
        {
            if (_requiresInit)
                Initialize();
            return _dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public ICollection<SearchColumnCustomField> Values
        {
            get 
            {
                if (_requiresInit)
                    Initialize();
                return _dictionary.Values;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="com.celigo.net.ServiceManager.SuiteTalk.SearchColumnCustomField"/> with the specified key.
        /// </summary>
        /// <value></value>
        public SearchColumnCustomField this[string key]
        {
            get
            {
                if (_requiresInit)
                    Initialize();
                return _dictionary[key];
            }
            set
            {
                if (_requiresInit)
                    Initialize();
                _dictionary[key] = value;
            }
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(KeyValuePair<string, SearchColumnCustomField> item)
        {
            if (_requiresInit)
                Initialize();
            _dictionary.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            _dictionary.Clear();
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(KeyValuePair<string, SearchColumnCustomField> item)
        {
            if (_requiresInit)
                Initialize();
            return _dictionary.ContainsKey(item.Key);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<string, SearchColumnCustomField>[] array, int arrayIndex)
        {
            if (_requiresInit)
                Initialize();

            var itr = _dictionary.GetEnumerator();
            for (int i = arrayIndex; i < array.Length; i++)
            {
                itr.MoveNext();
                array[i] = itr.Current;
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get
            {
                if (_requiresInit)
                    return _customFields.Length;
                else
                    return _dictionary.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<string, SearchColumnCustomField> item)
        {
            if (_requiresInit)
                Initialize();
            return _dictionary.Remove(item.Key);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, SearchColumnCustomField>> GetEnumerator()
        {
            if (_requiresInit)
                Initialize();
            return _dictionary.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
