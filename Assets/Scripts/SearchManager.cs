using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SearchManager : MonoBehaviour
{
    private const string GET_MOVIE_INFO_URL_FORM = "http://www.omdbapi.com/?apikey=cacf1230&s={0}&page={1}";
    private const int MAX_MOVIE_INFO_COUNT = 12;

    public MovieButtonManager manager;
    public MovieDetailManager detailManager;

    private List<MovieInfo> movieInfoList;
    private Dictionary<string, Texture> textureCachingDic;

    private int page = 1;

    internal Texture GetTexture(string imdbID)
    {
        return textureCachingDic[imdbID];
    }

    public void GetMovieInfoAndTexture(MovieInfo excludingOne)
    {
        List<MovieInfo> tempList = new List<MovieInfo>(movieInfoList);

        var foundMovieInfo = tempList.Find(info => info.imdbID == excludingOne.imdbID);
        tempList.Remove(foundMovieInfo);

        detailManager.TaskMovieInfoList(tempList);
        for(int i=0; i< tempList.Count; ++i)
        {
            detailManager.InsertTexture(i, textureCachingDic[tempList[i].imdbID]);
        }
    }

    public void OnValueChanged(InputField input)
    {
        page = 1;
        StopAllCoroutines();
        movieInfoList.Clear();
        StartCoroutine(GetMovieInfo(input.text, page));
    }

    private IEnumerator GetMovieInfo(string keyword, int page)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(string.Format(GET_MOVIE_INFO_URL_FORM, keyword, page));

        yield return webRequest.SendWebRequest();

        if(webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            var result = JsonToOject(webRequest.downloadHandler.text);
            if (result.Response)
            {
                var searchedList = result.Search;

                foreach (var item in searchedList)
                {
                    if (item.Poster.Contains("http"))
                    {
                        movieInfoList.Add(item);
                    }
                }

                movieInfoList.Sort();

                if (movieInfoList.Count > MAX_MOVIE_INFO_COUNT)
                {
                    movieInfoList.RemoveRange(MAX_MOVIE_INFO_COUNT, movieInfoList.Count - MAX_MOVIE_INFO_COUNT);
                }

                manager.TaskMovieInfoList(movieInfoList);

                for (int i = 0; i < movieInfoList.Count; i++)
                {
                    var movieInfo = movieInfoList[i];
                    var imdbID = movieInfoList[i].imdbID;

                    if (textureCachingDic.ContainsKey(imdbID))
                    {
                        manager.InsertTexture(i, textureCachingDic[imdbID]);
                    }
                    else
                    {
                        yield return StartCoroutine(GetTexture(i, movieInfo));
                    }
                }

                if (page == 1)
                {
                    for (int i = 0; i < (result.totalResults / 10); ++i)
                    {
                        yield return StartCoroutine(GetMovieInfo(keyword, ++page));
                    }

                    //movieInfoList.Sort();

                    //Debug.LogFormat("movieInfoList count = {0}, reulst.totalResults = {1}", movieInfoList.Count, result.totalResults);


                    // seraching completes
                }
            }
        }
    }

    private IEnumerator GetTexture(int targetIndex, MovieInfo movieInfo)
    {
        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(movieInfo.Poster);
        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            Texture texture = (webRequest.downloadHandler as DownloadHandlerTexture).texture;
            textureCachingDic.Add(movieInfo.imdbID, texture);
            manager.InsertTexture(targetIndex, texture);
        }
    }

    private SearchResult JsonToOject(string jsonData)
    {
        return JsonUtility.FromJson<SearchResult>(jsonData);
    }

    private void Awake()
    {
        movieInfoList = new List<MovieInfo>();
        textureCachingDic = new Dictionary<string, Texture>();
    }
}
