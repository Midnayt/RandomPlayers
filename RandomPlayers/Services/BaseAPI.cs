using System;
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
using RandomPlayers.Extentions;
using RandomPlayers.Fragments.DialogFragments;

namespace RandomPlayers.Services {
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

        protected async Task<ApiResponse> GetAsync(string url) {
            string str;

            System.Diagnostics.Debug.WriteLine($"BASESERVICE: {this?.GetType()?.Name}.GetAsync({url})");

            using (var realaser = await m_lock.LockAsync()) {
                try {
                    var response = await ExecuteGetAsync(url);
                    if (response.StatusCode == HttpStatusCode.Unauthorized) {
                        if (await RefreshToken())
                            response = await ExecuteGetAsync(url);
                    }
                    if (response.StatusCode == HttpStatusCode.Unauthorized) {
                        if (await ReLogin())
                            response = await ExecuteGetAsync(url);
                    }
                    if (response.StatusCode == HttpStatusCode.Unauthorized) {
                        var newFragment = new MessageAlert("Паролі не співпадають");

                        //Add fragment
                        //newFragment.Show(this.FragmentManager.BeginTransaction(), "dialog");
                    }

                    if (response.StatusCode == HttpStatusCode.BadRequest) {
                        var responseString = await response.Content.ReadAsStringAsync();
                        return new ApiResponse {
                            Succeed = false,
                            Errors = "ERROR",
                            //StatusCode = response.StatusCode
                        };
                    }
                    str = await response.Content.ReadAsStringAsync();
                } catch (Exception ex) {
                    var t = await CheckUrl(AuthUrl);
                    System.Diagnostics.Debug.WriteLine($"{this.GetType().Name}: GatAsync: Exception: {ex.Message}");
                    return new ApiResponse {
                        Succeed = false,
                        Errors = t ? "unknow" : "check connection",
                    };
                }
            }

            return new ApiResponse {
                Succeed = true,
            };
        }

        protected async Task<ApiResponse<T>> GetAsync<T>(string url) where T : new() {
            string str;
            T resultObject;

            System.Diagnostics.Debug.WriteLine($"BASESERVICE: {this?.GetType()?.Name}.GetAsync({url})");

            using (var realaser = await m_lock.LockAsync()) {
                try {
                    var response = await ExecuteGetAsync(url);
                    if (response.StatusCode == HttpStatusCode.Unauthorized) {
                        if (await RefreshToken())
                            response = await ExecuteGetAsync(url);
                    }
                    if (response.StatusCode == HttpStatusCode.Unauthorized) {
                        if (await ReLogin())
                            response = await ExecuteGetAsync(url);
                    }
                    if (response.StatusCode == HttpStatusCode.Unauthorized) {
                        var newFragment = new MessageAlert("Паролі не співпадають");

                        //Add fragment
                        //newFragment.Show(FragmentManager.BeginTransaction(), "dialog");
                    }

                    if (response.StatusCode == HttpStatusCode.BadRequest) {
                        var responseString = await response.Content.ReadAsStringAsync();
                        return new ApiResponse<T> {
                            Succeed = false,
                            Errors = "",
                            //StatucCode = response.StatusCode
                        };
                    }
                    str = await response.Content.ReadAsStringAsync();
                    resultObject = JsonConvert.DeserializeObject<T>(str);
                } catch (JsonReaderException ex) {
                    System.Diagnostics.Debug.WriteLine($"{this.GetType().Name}: GatAsync<{typeof(T).Name}>: Exception: {ex.Message}");
                    return new ApiResponse<T> {
                        Succeed = false,
                        Errors = "Cannot parse result"
                    };
                } catch (Exception ex) {
                    var t = await CheckUrl(AuthUrl);
                    System.Diagnostics.Debug.WriteLine($"{this.GetType().Name}: GatAsync<{typeof(T).Name}>: Exception: {ex.Message}");
                    return new ApiResponse<T> {
                        Succeed = false,
                        Errors = t ? $"{ex.Message}" : "Check internet connection"
                    };
                }
            }

            return new ApiResponse<T>() {
                Succeed = true,
                ResponseObject = resultObject,
            };
        }

        protected async Task<ApiResponse<T>> PostAsync<T>(string url, object keys) where T : new() {
            ApiResponse<T> result;
            var t = await CheckUrl(AuthUrl);
            System.Diagnostics.Debug.WriteLine($"BASESERVICE: {GetType().Name}.PostAsync<{typeof(T).Name}>({url}, {keys.GetType().Name})");
            using (var realaser = await m_lock.LockAsync()) {
                try {
                    var response = await ExecutePostAsync(url, keys);
                    if (response.StatusCode == HttpStatusCode.Unauthorized) {
                        if (await RefreshToken())
                            response = await ExecutePostAsync(url, keys);
                    }
                    if (response.StatusCode == HttpStatusCode.Unauthorized) {
                        if (await ReLogin())
                            response = await ExecutePostAsync(url, keys);
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

                    try {
                        if (typeof(T) == typeof(Guid)) {
                            if (Guid.TryParse(responseString, out Guid guid))
                                responseString = JsonConvert.SerializeObject(guid);
                        }
                    } catch { }
                    result = new ApiResponse<T>() {
                        Succeed = true,
                        ResponseObject = JsonConvert.DeserializeObject<T>(responseString)
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

        protected async Task<ApiResponse> PostAsync(string url, object keys) {
            using (var realaser = await m_lock.LockAsync()) {
                try {
                    var response = await ExecutePostAsync(url, keys);
                    if (response.StatusCode == HttpStatusCode.Unauthorized) {
                        if (await RefreshToken())
                            response = await ExecutePostAsync(url, keys);
                    }
                    if (response.StatusCode == HttpStatusCode.Unauthorized) {
                        if (await ReLogin())
                            response = await ExecutePostAsync(url, keys);
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

        private async Task<HttpResponseMessage> ExecutePostAsync(string url, object keys) {
            HttpClient httpClient = await CreateHttpClient();
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

        private async Task<HttpResponseMessage> ExecuteGetAsync(string url) {
            var client = await CreateHttpClient();
            return await client.GetAsync(url);
        }

        private async Task<HttpResponseMessage> ExecutePostAsync(string url, ICollection<byte[]> bytes) {

            var client = await CreateHttpClient();
            var content = new MultipartFormDataContent();
            foreach (var array in bytes) {
                var byteContent = new ByteArrayContent(array);
                byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                var name = Guid.NewGuid().ToString();
                byteContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = name + ".jpg", Name = "file" };
                content.Add(byteContent);
            }
            var response = await client.PostAsync(url, content);
            return response;
        }
    }
       
}
