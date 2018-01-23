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


            JToken t = JToken.FromObject(value, tt);

            //t = RemoveEmptyChildren(t);

            if (t.Type != JTokenType.Object) {
                t.WriteTo(writer);
            } else {
                JObject o = (JObject)t;
                var toRemoveKeys = new List<string>();

                foreach (var ob in o) {
                    if (ob.Value.Type == JTokenType.Null) {
                        toRemoveKeys.Add(ob.Key);
                    } else {

                        if (ob.Value.Type == JTokenType.String) {
                            var k = new { stringValue = ob.Value };
                            o[ob.Key] = JToken.FromObject(k);
                        } else {

                            if (ob.Value.Type == JTokenType.Integer) {
                                var k = new { integerValue = ob.Value };
                                o[ob.Key] = JToken.FromObject(k);
                            } else {

                                if (ob.Value.Type == JTokenType.Boolean) {
                                    var k = new { booleanValue = ob.Value };
                                    o[ob.Key] = JToken.FromObject(k);
                                } else {

                                    if (ob.Value.Type == JTokenType.Object) {
                                        var k = new { mapValue = ob.Value };
                                        o[ob.Key] = JToken.FromObject(k);
                                    } else {
                                        if (ob.Value.Type == JTokenType.Array) {
                                            var k = new { arrayValue = ob.Value };
                                            o[ob.Key] = JToken.FromObject(k);
                                        } else {
                                            if (ob.Value.Type == JTokenType.Date) {
                                                var k = new { timestampValue = ob.Value };
                                                o[ob.Key] = JToken.FromObject(k);
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (var key in toRemoveKeys) {
                    o.Remove(key);
                }

                o.WriteTo(writer);
            }
        }
            
        


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanRead {
            get { return false; }
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