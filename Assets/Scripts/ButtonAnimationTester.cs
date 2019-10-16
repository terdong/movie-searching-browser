using UnityEngine;
using UnityEditor;
using System;

public class ButtonAnimationTester : MonoBehaviour
{
    public MovieButtonManager manager;

    public int sampleDataLength;

    public void InsertButtons()
    {
        Tuple<int, int>[] sampleData = new Tuple<int, int>[sampleDataLength];
        for (int i = 0; i < sampleData.Length; i++)
        {
            sampleData[i] = CreateTuple(i,i);
        }

        manager.InsertSampleData(sampleData);
    }

    public void InsertMixedButtons()
    {
        Tuple<int,int>[] sampleData = new Tuple<int, int>[4];
        sampleData[0] = CreateTuple(1,1);
        sampleData[1] = CreateTuple(3,3);
        sampleData[2] = CreateTuple(0,0);
        sampleData[3] = CreateTuple(2,2);

        manager.InsertSampleData(sampleData);
    }

    public void Insert3Buttons()
    {
        manager.InsertSampleData(new Tuple<int, int>[] {
            CreateTuple(1, 1),
            CreateTuple(0, 0),
            CreateTuple(2, 3)
        });
    }

    public void Insert5Buttons()
    {
        manager.InsertSampleData(new Tuple<int, int>[] {
            CreateTuple(1, 1),
            CreateTuple(0, 0),
            CreateTuple(2, 2),
            CreateTuple(4, 4),
            CreateTuple(3, 3)
        });
    }

    public void InsertHigher5Buttons()
    {
        manager.InsertSampleData(new Tuple<int, int>[] {
            CreateTuple(1, 5),
            CreateTuple(0, 4),
            CreateTuple(2, 6),
            CreateTuple(4, 8),
            CreateTuple(3, 7)
        });
    }

    public void Insert12Buttons()
    {
        Tuple<int, int>[] sampleDatas = new Tuple<int, int>[12];
        for(int i=0; i<12; ++i)
        {
            sampleDatas[i] = CreateTuple(i, i);
        }
        manager.InsertSampleData(sampleDatas);
    }

    public void InsertOnlyOneValueButtons()
    {
        manager.InsertSampleData(new Tuple<int, int>[] { CreateTuple(0, 4) });
    }

    public void InsertSameValueButtons()
    {
        Tuple<int, int>[] sampleData = new Tuple<int, int>[2];
        sampleData[0] = CreateTuple(0, 2); ;
        sampleData[1] = CreateTuple(1, 4); ;

        manager.InsertSampleData(sampleData);
    }

    private Tuple<int,int> CreateTuple(int index, int value)
    {
        return new Tuple<int, int>(index, value);
    }


    //private void Start()
    //{

    //}
}