using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Backend;
using Sirenix.OdinInspector;

public class BackendService : MonoBehaviour
{
    public static BackendService Instance;

    [SerializeField]
    [Required]
    GraphQLAPI api;

    [SerializeField]
    List<QueryScriptable> queryList;

    private void Awake()
    {
        Instance = this;
    }

    public QueryScriptable FindQuery(string queryName)
    {
        for (int i = 0; i < queryList.Count; i++)
        {
            if (string.Equals(queryName, queryList[i].name, StringComparison.OrdinalIgnoreCase))
            {
                return queryList[i];
            }
        }
        Debug.Log("No such query: " + queryName);
        return null;
    }

    public QueryPackage BuildPackage(string queryName, List<string> argKeys, List<string> argValues)
    {
        return BuildPackage(FindQuery(queryName), argKeys, argValues);
    }

    public QueryPackage BuildPackage(QueryScriptable query, List<string> argKeys, List<string> argValues)
    {
        List<QueryArguments> queryArgList = new List<QueryArguments>();
        for (int i = 0; i < argKeys.Count; i++)
        {
            queryArgList.Add(new QueryArguments(argKeys[i], argValues[i]));
        }

        return BuildPackage(query, queryArgList);
    }

    public QueryPackage BuildPackage(QueryScriptable query, List<QueryArguments> args)
    {
        return new QueryPackage(query, args);
    }

    public void CallQueryForResponse(List<string> wantedVariables, QueryPackage queryPack, Action<QueryResponse> onResponse)
    {
        api.GetQueryResponse(wantedVariables, queryPack, onResponse);
    }

    public void CallQueryForResponse(string wantedVariable, QueryPackage queryPack, Action<QueryResponse> onResponse)
    {
        CallQueryForResponse(new List<string> { wantedVariable }, queryPack, onResponse);
    }

    public void CallQueryForResponse(List<string> argValues, QueryDataTemplate queryDataModel, Action<QueryResponse> onResponse)
    {
        QueryPackage package = BuildPackage(queryDataModel.queryNeeded, queryDataModel.queryNeeded.argKeyList, argValues);
        CallQueryForResponse(queryDataModel.dataNameArray, package, onResponse);
    }

    //[Button("Test User Data")]
    //public void SignIn(string ID)
    //{
    //    QueryPackage package = BuildPackage("GetUserData", new string[] { "userId" }, new string[] { ID });
    //    CallQueryForResponse("userData", package, DebugResults);            
    //}

    //void DebugResults(QueryResponse response)
    //{
    //    Debug.Log(response.keyValueInResponse["userData"]);
    //}
}


