using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RandomPlayers.DBO;

namespace RandomPlayers.Extentions {
    public class KeysJsonConverter : JsonConverter {

        enum FirestoreType { stringValue, integerValue, booleanValue, mapValue, arrayValue, nullValue, timestampValue, geoPointValue, referenceValue };

        private readonly Type[] _types;

        public KeysJsonConverter(params Type[] types) {
            _types = types;
        }



        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var tt = JsonSerializer.CreateDefault();
            tt.NullValueHandling = NullValueHandling.Ignore;
            tt.ContractResolver = new CamelCasePropertyNamesContractResolver();
            tt.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

            JToken t = JToken.FromObject(value, tt);

            if (t.Type != JTokenType.Object) {
                t.WriteTo(writer);
            } else {
                JObject o = (JObject)t;
                var toRemoveKeys = new List<string>();

                foreach (var ob in o) {
                    switch (ob.Value.Type) {

                        case JTokenType.Object: o[ob.Key] = JToken.FromObject(new { mapValue = ob.Value }); break;
                        case JTokenType.Array: o[ob.Key] = JToken.FromObject(new { arrayValue = ob.Value }); break;
                        case JTokenType.Integer: o[ob.Key] = JToken.FromObject(new { integerValue = ob.Value }); break;
                        case JTokenType.String: o[ob.Key] = JToken.FromObject(new { stringValue = ob.Value }); break;
                        case JTokenType.Boolean: o[ob.Key] = JToken.FromObject(new { booleanValue = ob.Value }); break;
                        case JTokenType.Null: toRemoveKeys.Add(ob.Key); break;
                        case JTokenType.Date: o[ob.Key] = JToken.FromObject(new { timestampValue = ob.Value }); break;
                        default: break;
                    }

                }
                foreach (var key in toRemoveKeys) {
                    o.Remove(key);
                }

                o.WriteTo(writer);
            }
        }




        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {

            var jObject = JObject.Load(reader);

            foreach (var child in jObject) {
                var valueType = JObject.FromObject(child.Value);
                var res = child.Value.First.ToObject<string>();
                jObject[child.Key] = res;
            }

            var target = Activator.CreateInstance(objectType);
            serializer.Populate(jObject.CreateReader(), target);

            return target;

        }

        public override bool CanRead {
            get { return true; }
        }

        public override bool CanConvert(Type objectType) {
            return _types.Any(t => t == objectType);
        }

        public static JToken RemoveEmptyChildren(JToken token) {
            if (token.Type == JTokenType.Object) {
                JObject copy = new JObject();
                foreach (JProperty prop in token.Children<JProperty>()) {
                    JToken child = prop.Value;
                    if (child.HasValues) {
                        child = RemoveEmptyChildren(child);
                    }
                    if (!IsEmpty(child)) {
                        copy.Add(prop.Name, child);
                    }
                }
                return copy;
            } else if (token.Type == JTokenType.Array) {
                JArray copy = new JArray();
                foreach (JToken item in token.Children()) {
                    JToken child = item;
                    if (child.HasValues) {
                        child = RemoveEmptyChildren(child);
                    }
                    if (!IsEmpty(child)) {
                        copy.Add(child);
                    }
                }
                return copy;
            }
            return token;
        }

        public static bool IsEmpty(JToken token) {
            return (token.Type == JTokenType.Null);
        }


    }
}