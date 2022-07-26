using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Backend
{
    public class GraphQLAPI : MonoBehaviour
    {
        [FoldoutGroup("Settings")]
        [SerializeField]
        string url;
        [FoldoutGroup("Settings")]
        [SerializeField]
        string apiKeyType = "x-api-key";
        [FoldoutGroup("Settings")]
        [SerializeField]
        string authToken;

        GraphQLClient client;

        private void Awake()
        {
            client = new GraphQLClient(url);
            client.SetAuth(apiKeyType, authToken);
        }

        /// <summary>
        /// Query backend for a QueryResponse based on the given variables list and QueryPackage.
        /// Calls onResponseCalled action after json has been parsed for variables.
        /// </summary>
        /// <param name="wantedVariables"></param>
        /// <param name="package"></param>
        /// <param name="onResponseCalled"></param>
        public void GetQueryResponse(List<string> wantedVariables, QueryPackage package, Action<QueryResponse> onResponseCalled)
        {
            SendQueryPackage(package, (value) => { OnResponseMade(value, wantedVariables, package, onResponseCalled); });
        }

        void OnResponseMade(string json, List<string> wantedVariables, QueryPackage package, Action<QueryResponse> onResponseCalled)
        {
            QueryResponse response = GraphQLDataParser.GetResponseFromJson(wantedVariables, package.query, json);
            onResponseCalled.Invoke(response);
        }

        IEnumerator QueryCall(string query, string arguments, Action<string> callback)
        {
            using (UnityWebRequest www = client.Query(query, arguments))
            {
                yield return www.Send();

                if (www.isNetworkError)
                {
                    Debug.Log(www.error);

                    callback(www.error);
                    yield break;
                }
                string responseString = www.downloadHandler.text;
                Debug.Log(responseString);

                callback(responseString);
            }
        }

        /// <summary>
        /// Use assembled package and give a action expecting to receive a json format string.
        /// Not recommended to use if unaware of how to use json.
        /// </summary>
        /// <param name="package"></param>
        /// <param name="onResponse"></param>
        public void SendQueryPackage(QueryPackage package, Action<string> onResponse)
        {
            string arg = BuildArg(package.argList);
            StartCoroutine(QueryCall(package.query.query, arg, onResponse));
        }

        /// <summary>
        /// Build arg for json Query IE { "key" : "value", "anotherKey" : "anotherValue" }
        /// </summary>
        /// <param name="argList"></param>
        /// <returns></returns>
        string BuildArg(List<QueryArguments> argList)
        {
            string arg = "{";
            for (int i = 0; i < argList.Count; i++)
            {
                arg += argList[i].ReturnArgument();
                if (i != argList.Count - 1)
                {
                    arg += ",";
                }
            }
            arg += "}";
            return arg;
        }
    }

    /// <summary>
    /// Class to help keep both the query and the needed args together.
    /// </summary>
    [Serializable]    
    public class QueryPackage
    {
        public QueryScriptable query;
        public List<QueryArguments> argList;

        public QueryPackage() { }
        public QueryPackage(QueryScriptable query)
        {
            this.query = query;
            argList = new List<QueryArguments>();
        }
        public QueryPackage(QueryScriptable query, List<QueryArguments> args)
        {
            this.query = query;
            this.argList = args;
        }
    }

    [Serializable]
    public class QueryArguments
    {
        public string key;
        public string value;

        public QueryArguments() { }
        public QueryArguments(string key, string value) 
        {
            this.key = key;
            this.value = value;
        }

        public string ReturnArgument()
        {
            return $"\"{key}\" : \"{value}\"";
        }
    }

    /// <summary>
    /// Class that holds both queried variables and the values.
    /// </summary>
    public class QueryResponse
    {
        public Dictionary<string, string> keyValueInResponse;

        public QueryResponse()
        {
            keyValueInResponse = new Dictionary<string, string>();
        }
    }
}

// //Anonymous
//var p = new
//{
//    getUserData = new
//    {
//        UserData = string.Empty
//    }
//};

//var obj = JsonConvert.DeserializeAnonymousType(responseString, new
//{
//    data = p
//}); ;