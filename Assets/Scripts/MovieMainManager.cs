using UnityEngine;
using UnityEditor;

public class MovieMainManager : MovieButtonManager
{
    public MovieDetailManager movieDetailManager;


    public void OnButtonClick(MovieButton button)
    {
        Debug.LogFormat("button.name = {0}", button.name);

        movieDetailManager.openPopup(button);
    }


    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < buttons.Count; i++)
        {
            var button = buttons[i];
            button.onClick.AddListener(delegate { OnButtonClick(button); });
            button.transform.localScale = Vector3.zero;
        }
    }
}