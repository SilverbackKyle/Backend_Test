using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Backend
{
    public static class GraphQLDataParser
    {
        public static QueryResponse GetResponseFromJson(List<string> wantedVariableList, QueryScriptable query, string json)
        {
            QueryResponse response = new QueryResponse();
            for(int i = 0; i < wantedVariableList.Count; i++)
            {
                string value = GetVariableFromJson(wantedVariableList[i], query, json);
                response.keyValueInResponse.Add(wantedVariableList[i], value);
            }
            return response;
        }

        public static string GetVariableFromJson(QueryReceivable receivable, string json)
        {
            var jsonDom = JsonConvert.DeserializeObject<JObject>(json)!;

            string path = "$.data";
            for (int i = 0; i < receivable.path.Count; i++)
            {
                path += "." + receivable.path[i];
            }
            path += "." + receivable.dataName;

            string variable = (string)jsonDom.SelectToken(path)!;
            return variable;
        }


        public static string GetVariableFromJson(string wantedVariable, QueryScriptable query, string json)
        {
            QueryReceivable receivable = query.FindReceivable(wantedVariable);
            if(receivable == null) { return ""; }
            return GetVariableFromJson(receivable, json);
        }
    }
}

