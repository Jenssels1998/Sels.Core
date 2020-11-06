﻿using Sels.Core.Extensions.Serialization.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sels.Core.Components.Serialization
{
    public class BsonProvider : ISerializationProvider
    {
        public T Deserialize<T>(string value)
        {
            return value.DeserializeFromBson<T>();
        }

        public string Serialize<T>(T value)
        {
            return value.SerializeAsBson();
        }
    }
}