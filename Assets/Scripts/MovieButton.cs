using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovieButton : Button
{
    public RectTransform RectTransform { get => rectTransform; set => rectTransform = value; }
    public Vector2 AnchoredPosition { get => rectTransform.anchoredPosition; set => rectTransform.anchoredPosition = value; }
    public Vector2 OriginPosition { get => originPosition; }
    public int TargetIndex { get => targetIndex; set => targetIndex = value; }
    public int CurrentIndex { get => currentIndex; set => currentIndex = value; }
    public int SampleData { get => sampleData; set => sampleData = value; }
    public float ButtonSize { get => buttonSize;}
    public MovieInfo MovieInfo { get => movieInfo; set => movieInfo = value; }
    public bool IsTexture { get => isTexture; }
    public Texture GetTexture { get => rawImage.texture; }

    private MovieInfo movieInfo;

    private int targetIndex;
    private int currentIndex;
    private float buttonSize;
    private bool isTexture;

    private RectTransform rectTransform;
    private Vector2 originPosition;
    private RawImage rawImage;
    private Text text;

    private int sampleData;

    public void ResetButton(float duration)
    {
        isTexture = false;
        RectTransform.DOScale(0, duration);
    }

    public void SetTexture(Texture texture)
    {
        isTexture = true;
        if (texture == null)
        {
            Debug.LogErrorFormat("texture is null, targetIndex = {0}, imdbID = {1}, title = {2}", targetIndex, movieInfo.imdbID, movieInfo.Title);
            return;
        }
        rawImage.texture = texture;
    }

    public void SetTextForTesting(Tuple<int, int> tuple)
    {
        text.text = string.Format("index = {0}\nvalue = {1}", tuple.Item1, tuple.Item2);
    }

    // Start is called before the first frame update
    void Awake()
    {
        rawImage = GetComponent<RawImage>();
        RectTransform = GetComponent<RectTransform>();
        text = GetComponentInChildren<Text>();
    }
    void Start()
    {
        originPosition = rectTransform.anchoredPosition;
        buttonSize = RectTransform.rect.width;
    }
}
