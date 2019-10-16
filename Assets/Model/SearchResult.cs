using UnityEngine;
using UnityEditor;

[System.Serializable]
public class SearchResult
{
    public MovieInfo[] Search;
    public int totalResults;
    public bool Response;

    public SearchResult(MovieInfo[] search, int totalResults, bool response)
    {
        Search = search;
        this.totalResults = totalResults;
        Response = response;
    }

    //Search[10]
    //totalResults	:	1906
    //Response	:	True
}