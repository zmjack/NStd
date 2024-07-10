﻿#if NET5_0_OR_GREATER
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NStandard.Text.Json.Converters;

public class JsonAsConverter<T, TSerialize> : JsonConverter<T> where T : TSerialize, new()
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = JsonSerializer.Deserialize<JsonElement>(ref reader, options);

        var instance = new T();
        var props = typeof(TSerialize).GetProperties();

        foreach (var prop in props)
        {
            var name = options.PropertyNamingPolicy?.ConvertName(prop.Name) ?? prop.Name;
            if (value.TryGetProperty(name, out var _prop))
            {
                var _value = _prop.Deserialize(prop.PropertyType, options);
                prop.SetValue(instance, _value);
            }
        }
        return instance;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            JsonSerializer.Serialize<string?>(writer, null, options);
        }
        else
        {
            var type = typeof(TSerialize);
            JsonSerializer.Serialize(writer, value, type, options);
        }
    }
}
#endif
