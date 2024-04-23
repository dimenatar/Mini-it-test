using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

public class DictionaryProgressManager
{
    private Dictionary<string, object> _pairs;

    public DictionaryProgressManager()
    {
        _pairs = LoadUserData();
    }

    public void WipeData()
    {
        _pairs.Clear();
    }

    public object GetValue(string tag, object defaultValue = null)
    {
        if (!_pairs.ContainsKey(tag))
        {
            _pairs.Add(tag, defaultValue);
        }
        return _pairs[tag];
    }

    public T GetTypedValue<T>(string tag, T defaultValue = default(T))
    {
        if (!_pairs.ContainsKey(tag))
        {
            _pairs.Add(tag, defaultValue);
        }
        return (T)_pairs[tag];
    }

    public T GetObject<T>(string tag, object defaultValue = null)
    {
        var obj = GetValue(tag, defaultValue);

        if (obj is T)
        {
            return (T) obj;
        }
        return default;
    }

    public void SaveValue(string tag, object value)
    {
        if (_pairs.ContainsKey(tag))
        {
            _pairs[tag] = value;
        }
        else
        {
            _pairs.Add(tag, value);
        }
    }

    public bool ContainsKey(string key)
    {
        return _pairs.ContainsKey(key);
    }

    public List<T> GetList<T>(string key)
    {
        var arr = GetValue(key, new List<T>()) as List<T>;
        return arr;
    }

    public IEnumerable<T> GetEnumerable<T>(string key)
    {
        return GetValue(key, Enumerable.Empty<T>()) as IEnumerable<T>;
    }

    public void AddToCollection<T>(string key, T item)
    {
        var arr = GetValue(key, new JArray()) as Newtonsoft.Json.Linq.JArray;
        var jItem = JToken.FromObject(item);
        arr.Add(jItem);
        SaveValue(key, arr);
    }

    public bool ContainsInCollection<T>(string key, T item)
    {
        var arr = GetValue(key, new JArray()) as Newtonsoft.Json.Linq.JArray;
        var jItem = JToken.FromObject(item);
        return arr.Contains(jItem);
    }

    public void ClearCollectionByKey(string key)
    {
        _pairs.Remove(key);
    }

    public int GetInt(string key, int defaultValue = 0)
    {
        return Convert.ToInt32(GetValue(key, defaultValue));
    }

    public long GetLong(string key, long defaultValue = 0L)
    {
        return Convert.ToInt64(GetValue(key, defaultValue));
    }

	public float GetFloat(string key, float defaultValue = 0f)
	{
		return Convert.ToSingle(GetValue(key, defaultValue));
	}

	public bool GetBool(string key, bool defaultValue = false)
    {
        return Convert.ToBoolean(GetValue(key, defaultValue));
    }

    public T GetEnum<T>(string key, T defaultValue = default)
    {
        T enumVal = (T)Enum.Parse(typeof(T), GetValue(key, defaultValue).ToString());
        return enumVal;
    }

    public int IncrementInt(string key, int defaultValue = 0)
    {
        var value = GetInt(key, defaultValue);
        SaveValue(key, ++value);
        return value;
    }

    private Dictionary<string, object> LoadUserData()
    {
        var data = UserDataSerializer<Dictionary<string, object>>.LoadUserData(new List<string>() { "missionData" });

        if (data == null)
        {
            data = new Dictionary<string, object>();
            UpdateUserData();
        }

        return data;
    }


    public void UpdateUserData()
    {
        if (_pairs != null)
        {
            UserDataSerializer<Dictionary<string, object>>.SaveUserData(_pairs);
        }
    }
}
