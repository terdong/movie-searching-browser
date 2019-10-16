using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MovieButtonManager : MonoBehaviour
{
    public MovieButton prefab;
    public Transform panel;

    public int buttonRowCount;
    public int buttonColumnCount;
    public float duration = 0.4f;

    protected List<MovieButton> buttons;
    private List<MovieButton> buttonsOnField;

    private Queue<MovieButton> buttonSpareQueue;
    private Queue<MovieButton> buttonActionQueue;

    private bool isAnimating = false;

    public void InsertTexture(int targetIndex, Texture texture)
    {
        foreach (var item in buttonActionQueue)
        {
            if(item.TargetIndex == targetIndex)
            {
                item.SetTexture(texture);
            }
        }
    }

    public void TaskMovieInfoList(List<MovieInfo> movieInfoList)
    {
        InitializeButtons();

        Queue<MovieButton> tempQueue = new Queue<MovieButton>();
        for (int i = 0; i < movieInfoList.Count; i++)
        {
            int targetIndex = i;
            var movieInfo = movieInfoList[i];
            var button = checkSameMovieInfoButtons(movieInfo);
            if (button == null)
            {
                button = buttonSpareQueue.Dequeue();
                button.MovieInfo = movieInfo;
                tempQueue.Enqueue(button);
            }
            button.TargetIndex = targetIndex;
        }

        ResetButtonsOnField();

        while (buttonActionQueue.Count > 0)
        {
            buttonSpareQueue.Enqueue(buttonActionQueue.Dequeue());
        }
        while (tempQueue.Count > 0)
        {
            buttonActionQueue.Enqueue(tempQueue.Dequeue());
        }

        //Debug.LogFormat("TaskMovieInfoList buttonActionQueue = {0}", buttonActionQueue.Count);
    }

    private MovieButton checkSameMovieInfoButtons(MovieInfo movieInfo)
    {
        for (int i = 0; i < buttonsOnField.Count; ++i)
        {
            if (buttonsOnField[i].MovieInfo.imdbID == movieInfo.imdbID)
            {
                return buttonsOnField[i];
            }
        }
        return null;
    }

    public void InsertSampleData(Tuple<int, int>[] sampleDatas)
    {
    //    InitializeButtons();

    //    Queue<MovieButton> tempQueue = new Queue<MovieButton>();
    //    for (int i = 0; i < sampleDatas.Length; i++)
    //    {
    //        var tuple = sampleDatas[i];
    //        int targetIndex = tuple.Item1;
    //        int sampleData = tuple.Item2;
    //        var button = checkSameDataForTesting(tuple);
    //        if (button == null)
    //        {
    //            button = buttonSpareQueue.Dequeue();
    //            button.SampleData = sampleData;
    //            tempQueue.Enqueue(button);
    //        }
    //        button.TargetIndex = targetIndex;
    //        button.SetTextForTesting(tuple);
    //    }

    //    ResetButtonsOnField();

    //    buttonActionQueue = tempQueue;
    }

    private MovieButton checkSameDataForTesting(Tuple<int, int> sampleData)
    {
    //    for (int i = 0; i < buttonsOnField.Count; ++i)
    //    {
    //        if (buttonsOnField[i].SampleData == sampleData.Item2)
    //        {
    //            return buttonsOnField[i];
    //        }
    //    }
        return null;
    }

    private void InitializeButtons()
    {
        foreach (var item in buttonsOnField)
        {
            item.TargetIndex = -1;
        }
    }

    private void ResetButtonsOnField()
    {
        List<MovieButton> tempList = new List<MovieButton>(buttonsOnField);
        foreach (var item in tempList)
        {
            if (item.TargetIndex == -1)
            {
                item.ResetButton(duration);
                buttonsOnField.Remove(item);
                buttonSpareQueue.Enqueue(item);
            }
        }
    }

    private void UpdateButtons()
    {
        // move to left
        for (int i = 0; i < buttonsOnField.Count; i++)
        {
            var button = buttonsOnField[i];
            if (button.CurrentIndex > button.TargetIndex || (i > 0 && button.CurrentIndex - buttonsOnField[i - 1].CurrentIndex > 1))
            {
                Sequence seqInner = DOTween.Sequence();
                button.CurrentIndex -= 1;
                var newPosition = buttons[button.CurrentIndex].OriginPosition;
                if (button.CurrentIndex % buttonColumnCount == buttonColumnCount - 1)
                {
                    seqInner.AppendCallback(() =>
                    {
                        var clone = Instantiate(button.RectTransform, button.transform.parent);
                        var clonePosition = button.AnchoredPosition;
                        clonePosition.x -= button.ButtonSize;
                        newPosition.x += button.ButtonSize;
                        button.AnchoredPosition = newPosition;
                        newPosition.x -= button.ButtonSize;
                        clone.DOAnchorPosX(clonePosition.x, duration).OnComplete(() => { Destroy(clone.gameObject); });
                    });
                }
                seqInner.Join(button.RectTransform.DOAnchorPosX(newPosition.x, duration));
                return;
            }
        }

        if (!isAnimating && buttonActionQueue.Count > 0)
        {
            MovieButton newButton = buttonActionQueue.Peek();
            if (!newButton.IsTexture) { return; }
            else { buttonActionQueue.Dequeue(); }

            isAnimating = true;
            Sequence seq = DOTween.Sequence();

            int targetIndex = newButton.TargetIndex;
            int currentIndex = targetIndex;

            var foundButton = buttonsOnField.Find(button => button != newButton && (button.CurrentIndex == currentIndex || button.TargetIndex > targetIndex));
            if (foundButton != null)
            {
                // push all buttons to right(button.CurrentIndex == currentIndex)
                int foundButtonCurrentIndex = foundButton.CurrentIndex;
                for (int i = foundButton.CurrentIndex; i < buttonsOnField.Count; ++i)
                {
                    var button = buttonsOnField[i];
                    button.CurrentIndex += 1;
                    var toMovePosition = buttons[button.CurrentIndex].OriginPosition;
                    if (button.CurrentIndex % buttonColumnCount == 0)
                    {
                        var clone = Instantiate(button.RectTransform, panel);
                        var cloneToPosition = button.AnchoredPosition.x + button.ButtonSize;
                        seq.Join(clone.DOAnchorPosX(cloneToPosition, duration).OnComplete(() => { Destroy(clone.gameObject); }));

                        toMovePosition.x -= button.ButtonSize;
                        button.AnchoredPosition = toMovePosition;
                        toMovePosition.x += button.ButtonSize;
                    }
                    seq.Join(button.RectTransform.DOAnchorPosX(toMovePosition.x, duration));
                }

                if (foundButton.TargetIndex > targetIndex)
                {
                    currentIndex = foundButtonCurrentIndex;
                }
            }
            else
            {
                currentIndex = buttonsOnField.Count;
            }

            newButton.CurrentIndex = currentIndex;

            if (!buttonsOnField.Contains(newButton))
            {
                if (buttonsOnField.Count > currentIndex)
                {
                    buttonsOnField.Insert(currentIndex, newButton);
                }
                else
                {
                    buttonsOnField.Add(newButton);
                }
                newButton.AnchoredPosition = buttons[currentIndex].OriginPosition;
                seq.Append(newButton.RectTransform.DOScale(1, duration));
            }

            //seq.OnComplete(() => { isAnimating = false; });
            seq.OnKill(() => { isAnimating = false; });
        }

        //int count = 0;
        //foreach (var item in buttonsOnField)
        //{
        //    Debug.LogFormat("item index = {0}, currentIndex = {1}", count++, item.CurrentIndex);
        //}
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        buttons = new List<MovieButton>(panel.GetComponentsInChildren<MovieButton>());
        buttonSpareQueue = new Queue<MovieButton>(buttons);
        buttonActionQueue = new Queue<MovieButton>();
        buttonsOnField = new List<MovieButton>();

        InvokeRepeating("UpdateButtons", 0.1f, duration);
    }

    public void GenerateButtons()
    {
        while (panel.childCount > 0)
        {
            DestroyImmediate(panel.GetChild(0).gameObject);
        }

        int row = buttonRowCount;
        int column = buttonColumnCount;
        float spacing = 10;

        var panelRectTransform = panel.GetComponent<RectTransform>();
        var panelWidth = panelRectTransform.rect.width;
        var panelHeight = panelRectTransform.rect.height;

        var buttonWidth = (panelWidth - (spacing * (column - 1))) / column;
        var buttonHeight = (panelHeight - (spacing * (row - 1))) / row;

        var buttonSize = buttonWidth < buttonHeight ? buttonWidth : buttonHeight;
        var buttonHalfSize = buttonSize * 0.5f;

        var spacingWidth = (panelWidth - (buttonSize * column)) / (column - 1);
        var spacingHeight = (panelHeight - (buttonSize * row)) / (row - 1);

        int buttonCount = 0;

        for (int k = 0; k < 2; ++k)
        {
            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < column; ++j)
                {
                    MovieButton button = Instantiate(prefab, panel);
                    button.name = buttonCount++.ToString();
                    var buttonTransForm = button.GetComponent<RectTransform>();
                    buttonTransForm.sizeDelta = new Vector2(buttonSize, buttonSize);
                    buttonTransForm.anchoredPosition = new Vector2(j * (buttonSize + spacingWidth) + buttonHalfSize, -(i * (buttonSize + spacingHeight) + buttonHalfSize));
                }
            }
        }
    }
}
