using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovieDetailManager : MovieButtonManager
{
    public GameObject movieDetailPanel;
    public RawImage posterRawImage;
    public Text moveDetailText;
    public SearchManager searchManager;

    private Stack<MovieInfo> buttonClickingHistory;
    private MovieInfo beforeMovieInfo;

    public void OnBackClick()
    {
        if(buttonClickingHistory.Count > 0)
        {
            var movieInfo = buttonClickingHistory.Pop();
            Texture texture = searchManager.GetTexture(movieInfo.imdbID);
            ChangeMovieDetailInfo(movieInfo, texture);
        }
        else
        {
            OnCloseClick();
        }
    }

    public void OnCloseClick()
    {
        movieDetailPanel.SetActive(false);
    }

    public void openPopup(MovieButton button)
    {
        var movieInfo = button.MovieInfo;
        movieDetailPanel.SetActive(true);
        ChangeMovieDetailInfo(movieInfo, button.GetTexture);

        beforeMovieInfo = movieInfo;
    }

    private void OnButtonClick(MovieButton button)
    {
        var movieInfo = button.MovieInfo;
        buttonClickingHistory.Push(beforeMovieInfo);
        ChangeMovieDetailInfo(movieInfo, button.GetTexture);
        beforeMovieInfo = movieInfo;
    }

    private void ChangeMovieDetailInfo(MovieInfo movieInfo, Texture texture)
    {
        posterRawImage.texture = texture;
        moveDetailText.text = movieInfo.ToString();
        searchManager.GetMovieInfoAndTexture(movieInfo);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < buttons.Count; i++)
        {
            var button = buttons[i];
            button.onClick.AddListener(delegate { OnButtonClick(button); });
            button.transform.localScale = Vector3.zero;
        }

        movieDetailPanel.SetActive(false);

        buttonClickingHistory = new Stack<MovieInfo>();
    }

    private void OnDisable()
    {
        buttonClickingHistory.Clear();
    }


}
