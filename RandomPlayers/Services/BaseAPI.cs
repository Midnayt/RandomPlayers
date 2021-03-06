﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RandomPlayers.DBO;
using RandomPlayers.Extentions;
using RandomPlayers.Fragments.DialogFragments;

namespace RandomPlayers.Services {

    public enum RequestMethodType { GET, POST, PATCH };

    public class BaseAPI {

        
        private static AsyncLock m_lock = new AsyncLock();

        const string AuthUrl = "https://firestore.googleapis.com/v1beta1/projects/random-players/databases/(default)";

        private async Task<HttpClient> CreateHttpClient() {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var s = await FirebaseAuth.Instance.CurrentUser.GetTokenAsync(false);
            var token = s.Token;
            if (!string.IsNullOrWhiteSpace(token)) {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return httpClient;
        }

        protected string GetParameterStr(object s) {
            try {
                var type = s.GetType();
                var properties = type.GetRuntimeProperties();
                var param = new List<KeyValuePair<string, string>>();
                foreach (var property in properties) {
                    if (property.CanRead) {
                        var st = property.GetValue(s)?.ToString() ?? "";
                        param.Add(new KeyValuePair<string, string>(property.Name, st));
                    }
                }
                var content = new FormUrlEncodedContent(param);
                return "?" + content.ReadAsStringAsync().Result;
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"Exception Get parameter string from object {s}: {ex.Message}");
                return "";
            }
        }

        protected async Task<bool> CheckUrl(string url) {
            HttpWebResponse responseCheck = null;
            var request = WebRequest.Create(url);
            request.Method = "HEAD";


            try {
                responseCheck = (HttpWebResponse)await request.GetResponseAsync();
                if (responseCheck.StatusCode != HttpStatusCode.OK) {
                    return false;
                }
            } catch {
                return false;
            } finally {
                if (responseCheck != null) {
                    responseCheck.Dispose();
                }
            }
            return true;
        }               

        protected async Task<bool> RefreshToken() {
            var token = await FirebaseAuth.Instance.CurrentUser.GetIdTokenAsync(true);
            //var refreshToken = ""; //TODO REFRESHTOKEN
            //if (string.IsNullOrWhiteSpace(refreshToken)) {
            //    System.Diagnostics.Debug.WriteLine($"BASESERVICE: {this?.GetType()?.Name}.RefreshToken({refreshToken}): BAD REFRESH TOKEN");
            //    return false;
            //}
            //System.Diagnostics.Debug.WriteLine($"BASESERVICE: {this?.GetType()?.Name}.RefreshToken({refreshToken})");
            //var keys = new List<KeyValuePair<string, string>>() {
            //    new KeyValuePair<string, string>("grant_type", "refresh_token"),
            //    new KeyValuePair<string, string>("token", refreshToken)
            //};

            //var response = await ExecutePostAsync(Configuration.Config.AuthUrl + "/auth/refresh-token", keys);
            //if (response.StatusCode == HttpStatusCode.OK) {
            //    try {
            //        var responseString = await response.Content.ReadAsStringAsync();
            //        var token = JsonConvert.DeserializeObject<AccessTokenResponse>(responseString);
            //        await AccountStorage.SetAsync(new Account("app", token.ToAccessToken().ToDictionary()));
            //        return true;
            //    } catch { }
            //}
            //System.Diagnostics.Debug.WriteLine($"BASESERVICE: {this?.GetType()?.Name}.RefreshToken({refreshToken}) - StatusCode not OK");
            return !string.IsNullOrEmpty(token?.Token);
        }

        protected async Task<bool> ReLogin() {
            //var url = Configuration.Config.AuthUrl + "/auth/";

            //HttpResponseMessage response = null;
            //var fToken = await AccountStorage.GetLoginValueAsync(AccountStorage.FacebookToken);
            //if (!string.IsNullOrEmpty(fToken)) {
            //    System.Diagnostics.Debug.WriteLine($"BASESERVICE: {this?.GetType()?.Name}.Relogin(fToken:{fToken})");
            //    response = await ExecuteGetAsync($"{url}account/facebook-token/{fToken}");
            //}

            //var vkToken = await AccountStorage.GetLoginValueAsync(AccountStorage.VKToken);
            //var vkUserId = await AccountStorage.GetLoginValueAsync(AccountStorage.VKUserId);
            //if (!string.IsNullOrEmpty(vkToken) && !string.IsNullOrEmpty(vkUserId)) {
            //    System.Diagnostics.Debug.WriteLine($"BASESERVICE: {this?.GetType()?.Name}.Relogin(vkToken:{vkToken}; vkUserId:{vkUserId})");
            //    response = await ExecuteGetAsync($"{url}account/vk-token/{vkToken}/{vkUserId}");
            //}

            //var email = await AccountStorage.GetLoginValueAsync(AccountStorage.EmailKey);
            //var pass = await AccountStorage.GetLoginValueAsync(AccountStorage.PasswordKey);
            //if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(pass)) {
            //    System.Diagnostics.Debug.WriteLine($"BASESERVICE: {this?.GetType()?.Name}.Relogin(email:{email}; password:{(string.IsNullOrEmpty(pass) ? "null" : "***")})");

            //    var keys = new List<KeyValuePair<string, string>>
            //    {
            //        new KeyValuePair<string, string>("grant_type", "password"),
            //        new KeyValuePair<string, string>("username", email),
            //        new KeyValuePair<string, string>("password", pass),
            //        new KeyValuePair<string, string>("isPersistent", "true"),
            //        new KeyValuePair<string, string>("client_id", "xamarin/app"),
            //        new KeyValuePair<string, string>("client_secret", "abc123")
            //    };

            //    response = await ExecutePostAsync($"{url}token", keys);

            //}
            //System.Diagnostics.Debug.WriteLine($"{this?.GetType()?.Name}.Relogin(StatucCode == {response?.StatusCode.ToString()}");
            //if (response?.StatusCode == HttpStatusCode.OK) {
            //    try {
            //        var responseString = await response.Content.ReadAsStringAsync();
            //        var token = JsonConvert.DeserializeObject<AccessTokenResponse>(responseString);
            //        await AccountStorage.SetAsync(new Account(string.IsNullOrWhiteSpace(email) ? "" : email, token.ToAccessToken().ToDictionary()));
            //        await Task.Delay(1011);
            //        return true;
            //    } catch { }
            //}

            //System.Diagnostics.Debug.WriteLine($"BASESERVICE: {this?.GetType()?.Name}.Relogin() - StatusCode not OK");
            return false;
            //_messenger.Publish(new SignOutMessage(this));
        }

        protected async Task<ApiResponse> SendAsync (string url, RequestMethodType type = RequestMethodType.GET, object keys=null) {
            using (var realaser = await m_lock.LockAsync()) {
                try {
                    var response = await ExexuteSendAsync(url, type, keys );
                    if (response.StatusCode == HttpStatusCode.Unauthorized) {
                        if (await RefreshToken())
                            response = await ExexuteSendAsync(url, type, keys);
                    }
                    if (response.StatusCode == HttpStatusCode.Unauthorized) {
                        if (await ReLogin())
                            response = await ExexuteSendAsync(url, type, keys);
                    }
                    if (response.StatusCode == HttpStatusCode.Unauthorized) {
                        var newFragment = new MessageAlert("Паролі не співпадають");
                        // TODO: make somethins
                        //Add fragment
                        //newFragment.Show(FragmentManager.BeginTransaction(), "dialog");
                    }

                    var responseString = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK) {
                        if (!string.IsNullOrEmpty(responseString)) {
                            try {
                                var settings = new JsonSerializerSettings {
                                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                    NullValueHandling = NullValueHandling.Ignore
                                };
                                var token = JToken.Parse(responseString);
                                var error = token["error"]["message"].ToString();
                                //var error = JsonConvert.DeserializeObject<ErrorResponse>(responseString, settings);
                                return new ApiResponse {
                                    Succeed = false,
                                    Errors = error,
                                    StatusCode = response.StatusCode
                                    //ErrorType = error.Error,
                                };
                            } catch { }
                        }
                        return new ApiResponse {
                            Succeed = false,
                            Errors = "Unknow http error",
                            StatusCode = response.StatusCode
                        };
                    }
                } catch (Exception ex) {
                    var t = await CheckUrl(AuthUrl);
                    System.Diagnostics.Debug.WriteLine($"{this.GetType().Name}: PostAsync({url}): Exception: {ex.Message}");
                    return new ApiResponse() {
                        Succeed = false,
                        Errors = t ? "Unknow http error" : "Please check internet connection",
                    };
                }
            }
            return new ApiResponse() {
                Succeed = true,
            };
        }

        protected async Task<ApiResponse<T>> SendAsync<T>(string url, RequestMethodType type = RequestMethodType.GET, object keys = null ) where T : BaseDBO, new() {
            ApiResponse<T> result;
            System.Diagnostics.Debug.WriteLine($"BASESERVICE: {GetType().Name}.SendAsync<{typeof(T).Name}>({url}, methodType = {type.ToString()})");
            using (var realaser = await m_lock.LockAsync()) {
                try {
                    var response = await ExexuteSendAsync(url, type, keys);
                    if (response.StatusCode == HttpStatusCode.Unauthorized) {
                        if (await RefreshToken())
                            response = await ExexuteSendAsync(url, type, keys);
                    }
                    if (response.StatusCode == HttpStatusCode.Unauthorized) {
                        if (await ReLogin())
                            response = await ExexuteSendAsync(url, type, keys);
                    }
                    if (response.StatusCode == HttpStatusCode.Unauthorized) {
                        var newFragment = new MessageAlert("Паролі не співпадають");

                        //Add fragment
                        //newFragment.Show(FragmentManager.BeginTransaction(), "dialog");
                    }

                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode != HttpStatusCode.OK) {
                        if (!string.IsNullOrEmpty(responseString)) {
                            try {
                                var settings = new JsonSerializerSettings {
                                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                    NullValueHandling = NullValueHandling.Ignore
                                };
                                var token = JToken.Parse(responseString);
                                var error = token["error"]["message"].ToString();

                                return new ApiResponse<T> {
                                    Succeed = false,
                                    Errors = error,
                                    StatusCode = response.StatusCode

                                };
                            } catch { }
                        }
                        return new ApiResponse<T> {
                            Succeed = false,
                            Errors = "Unknow http error",
                            StatusCode = response.StatusCode
                        };
                    }

                    result = new ApiResponse<T>() {
                        Succeed = true,
                        ResponseObject = ConvertToBaseObject<T>(responseString)
                    };
                } catch (JsonReaderException ex) {
                    System.Diagnostics.Debug.WriteLine($"{this.GetType().Name}: GatAsync<{typeof(T).Name}>: Exception: {ex.Message}");
                    return new ApiResponse<T>() {
                        Succeed = false,
                        Errors = "Cannot parse result"
                    };
                } catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine($"EXCEPTION: {GetType().Name}.PostAsync({url}): {ex.Message}");
                    return new ApiResponse<T>() {
                        Succeed = false,
                        Errors = "Unknow http error"
                    };
                }
            }
            return result;
        }

        private async Task<HttpResponseMessage> ExexuteSendAsync(string url, RequestMethodType type = RequestMethodType.GET, object keys=null) {
            var httpClient = await CreateHttpClient();
            switch (type) {
                case RequestMethodType.GET: return await httpClient.GetAsync(url);
                case RequestMethodType.POST: {
                        if (keys.GetType() == typeof(List<KeyValuePair<string, string>>)) {
                            var response = await httpClient.PostAsync(url, new FormUrlEncodedContent((List<KeyValuePair<string, string>>)keys));
                            return response;
                        } else {
                            httpClient.DefaultRequestHeaders.Accept.Clear();
                            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            string body = "";
                            if (keys.GetType() == typeof(string))
                                body = (string)keys;
                            else {
                                var settings = new JsonSerializerSettings {
                                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                    NullValueHandling = NullValueHandling.Ignore
                                };

                                body = JsonConvert.SerializeObject(keys, Formatting.Indented, settings);
                            }
                            var response = await httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
                            return response;
                        }
                    }
                case RequestMethodType.PATCH: {
                        string body = "";
                        if (keys.GetType() == typeof(string))
                            body = (string)keys;
                        else {
                            var settings = new JsonSerializerSettings {
                                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                NullValueHandling = NullValueHandling.Ignore
                            };

                            body = JsonConvert.SerializeObject(keys, Formatting.Indented, settings);
                        }
                        var method = new HttpMethod("PATCH");
                        var request = new HttpRequestMessage(method, url){
                            Content = new StringContent(body, Encoding.UTF8, "application/json")
                        };
                        return await httpClient.SendAsync(request);
                    }
                default: {
                        System.Diagnostics.Debug.WriteLine($"EXCEPTION: {GetType().Name}.PostAsync({url}): ");
                        break;
                    }
            }
            return null;
            
        }

       
        public static T ConvertToBaseObject<T>(string jsonString) where T : BaseDBO, new() {

            var jobj = JObject.Parse(jsonString);
            var t = jobj["fields"].ToString();
            var ret = JsonConvert.DeserializeObject<T>(t, new KeysJsonConverter(typeof(T)));
            ret.Name = jobj["name"].ToObject<string>();
            var ind = Math.Min(Math.Max(ret.Name.LastIndexOf('/'), 0) + 1, ret.Name.Length);
            ret.Id = ret.Name.Substring(ind, ret.Name.Length - ind);
            ret.CreatedAt = jobj["createTime"].ToObject<DateTime>();
            ret.UpdatedAt = jobj["updateTime"].ToObject<DateTime>();
            return ret;
        }

                
    }
}