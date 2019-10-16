using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

[System.Serializable]
public class MovieInfo : IComparable<MovieInfo>
{
    public string Title;
    public int Year;
    public string imdbID;
    public string Type;
    public string Poster;

    public MovieInfo(string title, int year, string imdbID, string type, string poster)
    {
        Title = title;
        Year = year;
        this.imdbID = imdbID;
        Type = type;
        Poster = poster;
    }

    public int CompareTo(MovieInfo other)
    {
        int result = Title.CompareTo(other.Title);
        if(result == 0) { result = imdbID.CompareTo(other.imdbID); }
        return result;
    }

    public override string ToString()
    {
        return string.Format("Title: {0}\nYear: {1}\nimdbID: {2}\nType: {3}",Title, Year, imdbID, Type);
    }

    //Title	:	The Rock
    //Year	:	1996
    //imdbID	:	tt0117500
    //Type	:	movie
    //Poster : https://m.media-amazon.com/images/M/MV5BZDJjOTE0N2EtMmRlZS00NzU0LWE0ZWQtM2Q3MWMxNjcwZjBhXkEyXkFqcGdeQXVyNDk3NzU2MTQ@._V1_SX300.jpg
}
