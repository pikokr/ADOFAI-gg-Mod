using System;
using System.Collections;
using System.Collections.Generic;
using TinyJSON;
using TinyJSON.Types;
using ADOFAI_GG.Data.Entity.Remote.Types;
using ADOFAI_GG.Data.Entity.Remote.Filters;
using MelonLoader;
using UnityEngine;
using UnityEngine.Networking;

namespace ADOFAI_GG.Data {
    public static class RequestCo {
        public const string RequestURL = "https://api.adofai.gg:9200/";

        private static IEnumerator _none() {
            yield break;
        }

        public static IEnumerator None => _none();
        
        public static IEnumerator RequestLevels(LevelFilter filter, Func<List<Level>, int, IEnumerator> callback, Func<IEnumerator> fail = null) {
            IEnumerator Callback(Variant json) {
                if (json == null) {
                    yield return fail?.Invoke();
                    yield break;
                }

                var result = new List<Level>();
                var results = json["results"];

                foreach (var level in (ProxyArray) results) {
                    result.Add(Level.FromJson(level));
                }

                yield return callback(result, json["count"]);
            }

            yield return _request($"api/v1/levels?{filter}", Callback, fail = null);
            
        }

        public static IEnumerator RequestLevel(int id, Func<Level, IEnumerator> callback, Func<IEnumerator> fail = null) {
            IEnumerator Callback(Variant json) {
                if (json == null) {
                    yield return fail?.Invoke();
                    yield break;
                }
                yield return callback(Level.FromJson(json));
            }
            yield return _request($"api/v1/levels/{id}", Callback, fail = null);
        }

        public static IEnumerator RequestPeople(PersonFilter filter, Func<List<Person>, int, IEnumerator> callback, Func<IEnumerator> fail = null) {
            IEnumerator Callback(Variant json) {
                if (json == null) {
                    yield return fail?.Invoke();
                    yield break;
                }
                var result = new List<Person>();
                var results = json["results"];

                foreach (var person in (ProxyArray) results) {
                    result.Add(Person.FromJson(person));
                }

                yield return callback(result, json["count"]);
            }

            yield return _request($"api/v1/person?{filter}", Callback, fail = null);
        }

        public static IEnumerator RequestPerson(int id, Func<Person, IEnumerator> callback, Func<IEnumerator> fail = null) {
            IEnumerator Callback(Variant json) {
                if (json == null) {
                    yield return fail?.Invoke();
                    yield break;
                }

                yield return callback(Person.FromJson(json));
            }
            
            yield return _request($"api/v1/person/{id}", Callback, fail = null);
        }


        public static IEnumerator RequestRanking(RankingFilter filter, Func<List<Ranking>, int, IEnumerator> callback, Func<IEnumerator> fail = null) {
            IEnumerator Callback(Variant json) {
                if (json == null) {
                    yield return fail?.Invoke();
                    yield break;
                }
                
                var result = new List<Ranking>();
                var results = (ProxyArray) json["results"];

                int count = result.Count;
                foreach (var rank in results) {
                    yield return Ranking.FromJson(rank, ranking => {
                        result.Add(ranking);
                        count -= 1;
                        return None;
                    });
                }

                yield return new WaitUntil(() => count == 0);
                yield return callback(result, json["count"]);
            }
            
            yield return _request($"api/v1/ranking?{filter}", Callback, fail = null);

        }

        public static IEnumerator RequestPlayLogs(PlayLogsFilter filter, Func<List<PlayLog>, int, IEnumerator> callback, Func<IEnumerator> fail = null) {
            IEnumerator Callback(Variant json) {
                if (json == null) {
                    yield return fail?.Invoke();
                    yield break;
                }
                
                var result = new List<PlayLog>();
                var results = (ProxyArray) json["results"];

                int count = result.Count;
            
                foreach (var playlog in results) {
                    yield return PlayLog.FromJson(playlog, log => {
                        result.Add(log);
                        count -= 1;
                        return None;
                    });
                }

                yield return new WaitUntil(() => count == 0);
                yield return callback(result, json["count"]);
            }

            yield return _request($"api/v1/playLogs?{filter}", Callback, fail = null);
        }

        public static IEnumerator RequestPlayLog(int id, Func<PlayLog, IEnumerator> callback, Func<IEnumerator> fail = null) {
            IEnumerator Callback(Variant json) {
                if (json == null) {
                    yield return fail?.Invoke();
                    yield break;
                }

                IEnumerator Callback2(PlayLog playLog) {
                    yield return callback(playLog);
                }

                yield return PlayLog.FromJson(json, Callback2, fail = null);
            }

            yield return _request($"api/v1/playLogs/{id}", Callback, fail = null);
        }

        private static IEnumerator _request(string url, Func<Variant, IEnumerator> callback, Func<IEnumerator> fail = null) {
            MelonLogger.Msg(url);
            var client = new UnityWebRequest(RequestURL + url);
            client.downloadHandler = new DownloadHandlerBuffer();
            yield return client.SendWebRequest();
            
            if (client.isHttpError || client.isNetworkError) {
                MelonLogger.Msg("Failed fetching");
                yield return fail?.Invoke();
            }

            var content = client.downloadHandler.text;
            var json = JSON.Load(content);
            yield return callback(json);
        }
    }
}