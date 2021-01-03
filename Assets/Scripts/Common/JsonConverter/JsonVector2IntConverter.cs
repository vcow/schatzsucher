using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;

namespace Common.JsonConverter
{
	public class JsonVector2IntConverter : Newtonsoft.Json.JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var v = (Vector2Int) value;
			writer.WriteStartObject();
			writer.WritePropertyName("x");
			writer.WriteValue(v.x);
			writer.WritePropertyName("y");
			writer.WriteValue(v.y);
			writer.WriteEndObject();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
			JsonSerializer serializer)
		{
			Assert.IsTrue(reader.TokenType == JsonToken.StartObject);
			long x = 0, y = 0;
			while (reader.Read() && reader.TokenType != JsonToken.EndObject)
			{
				Assert.IsTrue(reader.TokenType == JsonToken.PropertyName);
				switch (reader.Value.ToString())
				{
					case "x":
						reader.Read();
						x = (long) reader.Value;
						break;
					case "y":
						reader.Read();
						y = (long) reader.Value;
						break;
					default:
						throw new JsonSerializationException();
				}
			}

			return new Vector2Int((int) x, (int) y);
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Vector2Int);
		}
	}
}