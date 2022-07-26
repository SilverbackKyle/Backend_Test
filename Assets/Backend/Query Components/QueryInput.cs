using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueryInput : MonoBehaviour
{
    [SerializeField] int indexForInput;

    [SerializeField] TestQuery query;

    public void SetInput(string input)
    {
        query.SetInputValue(indexForInput, input);
    }
}
