using System.Collections.Generic;
using Common.JsonConverter;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Common
{
	/// <summary>
	/// Вспомогательный класс - сериализатор/десериализатор, включает
	/// все необходимые настройки для Newtonsoft JSON.
	/// </summary>
	public static class Serializer
	{
		private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.Auto,
			Converters = new List<Newtonsoft.Json.JsonConverter>
			{
				new JsonVector2IntConverter(),
				new StringEnumConverter()
			}
		};

		public static string Serialize<T>(T value)
		{
			return JsonConvert.SerializeObject(value, _serializerSettings);
		}

		public static T Deserialize<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json, _serializerSettings);
		}
	}
}