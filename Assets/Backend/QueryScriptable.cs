using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QLQuery", menuName = "GraphQL/QueryScriptable")]
public class QueryScriptable : ScriptableObject
{
    [SerializeField]
    [TextArea(15, 30)]
    public string query;

    [SerializeField]
    public List<string> argKeyList;

    [SerializeField]
    public List<QueryReceivable> receivablesList;

    public QueryReceivable FindReceivable(string name)
    {
        for(int i = 0; i < receivablesList.Count; i++)
        {
            if (string.Equals(name, receivablesList[i].dataName, StringComparison.OrdinalIgnoreCase))
            {
                return receivablesList[i];
            }
        }
        Debug.LogError("No such dataName: " + name);
        return null;
    }
}

[Serializable]
public class QueryReceivable
{
    public string dataName;
    public List<string> path;
}
