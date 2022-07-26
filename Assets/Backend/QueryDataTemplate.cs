using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QDataTemplate", menuName = "GraphQL/QueryDataTemplate")]
public class QueryDataTemplate : ScriptableObject
{
    [SerializeField]
    public List<string> dataNameArray;
    [SerializeField]
    public QueryScriptable queryNeeded;
}