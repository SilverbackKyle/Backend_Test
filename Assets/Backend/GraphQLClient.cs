using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Backend
{
    public class GraphQLClient
    {
        string url;
        string key;
        string token;

        public GraphQLClient(string url)
        {
            this.url = url;
        }

        public void SetAuth(string key, string token)
        {
            this.key = key;
            this.token = token;
        }

        [Serializable]
        private class GraphQLQuery
        {
            public string query;
            public string variables;
        }        

        public UnityWebRequest Query(string query, string variables)
        {
            var fullQuery = new GraphQLQuery()
            {
                query = query,
                variables = variables
            };

            string jsonData = JsonConvert.SerializeObject(fullQuery);
            byte[] postData = Encoding.UTF8.GetBytes(jsonData);
            UnityWebRequest request = UnityWebRequest.Post(url, UnityWebRequest.kHttpVerbPOST);
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.SetRequestHeader("Content-Type", "application/json");
            if(!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(token))
            {
                request.SetRequestHeader(key, token);
            }            

            return request;
        }
    }

    
}
