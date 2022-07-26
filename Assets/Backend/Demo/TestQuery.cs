using Backend;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestQuery : MonoBehaviour
{
    public QueryDataTemplate queryModel;
    public List<string> inputValues;

    public UnityEvent<string> onResponseMade;

    [Button("Debug Query")]
    public void DebugQuery()
    {
        BackendService.Instance.CallQueryForResponse(inputValues, queryModel, DebugResponse);
    }

    void DebugResponse(QueryResponse response)
    {
        string txtResponse = "";
        for (int i = 0; i < queryModel.dataNameArray.Count; i++)
        {
            txtResponse += response.keyValueInResponse[queryModel.dataNameArray[i]];
            Debug.Log(response.keyValueInResponse[queryModel.dataNameArray[i]]);
        }
        onResponseMade.Invoke(txtResponse);
    }

    public void SetInputValue(int index, string input)
    {
        if(inputValues.Count <= index)
        {
            inputValues.Insert(index, input);
        }
        inputValues[index] = input;
    }
}
