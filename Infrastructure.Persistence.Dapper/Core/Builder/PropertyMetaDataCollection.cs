using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence.Dapper.Core.Builder;

internal class PropertyMetaDataCollection : IDictionary<string, PropertyMetaData>
{
    private readonly Dictionary<string, PropertyMetaData> _properties = [];

    public int Count => _properties.Count;

    public IEnumerable<string> Keys => _properties.Keys;

    ICollection<string> IDictionary<string, PropertyMetaData>.Keys => _properties.Keys;

    public ICollection<PropertyMetaData> Values => _properties.Values;

    public bool IsReadOnly => ((ICollection<KeyValuePair<string, PropertyMetaData>>)_properties).IsReadOnly;

    public PropertyMetaData this[string key] { get => ((IDictionary<string, PropertyMetaData>)_properties)[key]; set => ((IDictionary<string, PropertyMetaData>)_properties)[key] = value; }

    public IEnumerator<KeyValuePair<string, PropertyMetaData>> GetEnumerator() => _properties.GetEnumerator();

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out PropertyMetaData value) => _properties.TryGetValue(key, out value);

    public void Add(string propertyType, PropertyMetaData metaData) => _properties.Add(propertyType, metaData);

    public void Remove(string propertyType) => _properties.Remove(propertyType);

    public bool ContainsKey(string key) => _properties.ContainsKey(key);

    IEnumerator IEnumerable.GetEnumerator() => _properties.GetEnumerator();

    bool IDictionary<string, PropertyMetaData>.Remove(string key) => ((IDictionary<string, PropertyMetaData>)_properties).Remove(key);

    public void Add(KeyValuePair<string, PropertyMetaData> item) => ((ICollection<KeyValuePair<string, PropertyMetaData>>)_properties).Add(item);

    public void Clear() => ((ICollection<KeyValuePair<string, PropertyMetaData>>)_properties).Clear();

    public bool Contains(KeyValuePair<string, PropertyMetaData> item) => ((ICollection<KeyValuePair<string, PropertyMetaData>>)_properties).Contains(item);

    public void CopyTo(KeyValuePair<string, PropertyMetaData>[] array, int arrayIndex) => ((ICollection<KeyValuePair<string, PropertyMetaData>>)_properties).CopyTo(array, arrayIndex);

    public bool Remove(KeyValuePair<string, PropertyMetaData> item) => ((ICollection<KeyValuePair<string, PropertyMetaData>>)_properties).Remove(item);

    public override string ToString() => base.ToString() ?? throw new ArgumentException("Can not convert to string");
}