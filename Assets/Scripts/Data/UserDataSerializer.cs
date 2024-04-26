using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Data
{
	public static class UserDataSerializer<T>
	{

#if UNITY_EDITOR
		public static string Path { get; } = "Assets//UserData.json";
#else
    public static string Path { get; } = Application.persistentDataPath + "//UserData.json";
#endif

		public static T LoadUserData(string path)
		{
			return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
		}

		public static T LoadUserData(List<string> propertiesToRemove)
		{
			if (File.Exists(Path))
			{
				var rawJson = File.ReadAllText(Path);

				JObject respJObject = JObject.Parse(rawJson);

				RemoveFromJObjectOfName(respJObject, propertiesToRemove);

				var updatedJson = respJObject.ToString();
				var obj = respJObject.ToObject<Dictionary<string, object>>();

				return JsonConvert.DeserializeObject<T>(updatedJson, new JsonSerializerSettings()
				{
					TypeNameHandling = TypeNameHandling.Auto
				});

			}
			return default;
		}

		public static void SaveUserData(string path, T data)
		{
			File.WriteAllText(path, JsonConvert.SerializeObject(data, new JsonSerializerSettings()
			{
				TypeNameHandling = TypeNameHandling.All
			}));
		}

		public static void SaveUserData(T data)
		{
			File.WriteAllText(Path, JsonConvert.SerializeObject(data, new JsonSerializerSettings()
			{
				TypeNameHandling = TypeNameHandling.All
			}));
		}

		private static void RemoveFromJObjectOfName(JObject jObject, List<string> propertyNames)
		{
			var properties = jObject.Properties();

			for (int i = 0; i < properties.Count(); i++)
			{
				ManageProperty(properties.ElementAt(i), propertyNames);
			}
		}

		private static void ManageProperty(JProperty jProperty, List<string> propertyNames)
		{
			if (propertyNames.Contains(jProperty.Name))
			{
				jProperty.Remove();
			}
			else
			{
				var jToken = jProperty.Value;
				ManageToken(jToken, propertyNames);
			}
		}

		private static void ManageToken(JToken jToken, List<string> propertyNames)
		{
			if (jToken is JObject jObj)
			{
				RemoveFromJObjectOfName(jObj, propertyNames);
			}
			else if (jToken is JArray jArray)
			{
				for (int i = 0; i < jArray.Count; i++)
				{
					JToken arrayJToken = jArray[i];
					ManageToken(arrayJToken, propertyNames);
				}
			}
			else if (jToken is JProperty jProperty)
			{
				ManageProperty(jProperty, propertyNames);
			}
		}
	}
}