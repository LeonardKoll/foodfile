using Elasticsearch.Net;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static Nest.ConnectionSettings;

namespace Helpers
{
    public class CustomJsonNetSerializerFactory : IElasticsearchSerializer
    {
        public IElasticsearchSerializer Create(IConnectionSettingsValues settings)
        {
            return new CustomJsonNetSerializer(settings);
        }
        public IElasticsearchSerializer CreateStateful(IConnectionSettingsValues settings, JsonConverter converter)
        {
            return new CustomJsonNetSerializer(settings, converter);
        }

        public object Deserialize(Type type, Stream stream)
        {
            throw new NotImplementedException();
        }

        public T Deserialize<T>(Stream stream)
        {
            throw new NotImplementedException();
        }

        public Task<object> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Serialize<T>(T data, Stream stream, SerializationFormatting formatting = SerializationFormatting.None)
        {
            throw new NotImplementedException();
        }

        public Task SerializeAsync<T>(T data, Stream stream, SerializationFormatting formatting = SerializationFormatting.None, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }

    public class CustomJsonNetSerializer : JsonNetSerializer
    {
        public CustomJsonNetSerializer(IConnectionSettingsValues settings) : base(settings)
        {
            base.OverwriteDefaultSerializers(ModifyJsonSerializerSettings);
        }
        public CustomJsonNetSerializer(IConnectionSettingsValues settings, JsonConverter statefulConverter) :
            base(settings, statefulConverter)
        {
            base.OverwriteDefaultSerializers(ModifyJsonSerializerSettings);
        }

        private void ModifyJsonSerializerSettings(JsonSerializerSettings settings, IConnectionSettingsValues connectionSettings)
        {
            settings.NullValueHandling = NullValueHandling.Include;
            settings.TypeNameHandling = TypeNameHandling.Objects;
        }
    }
}
