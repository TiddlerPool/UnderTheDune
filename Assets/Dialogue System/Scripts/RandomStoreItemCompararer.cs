using System.Collections.Generic;

public class IntValueComparer : IComparer<StoreRandomItemData>
{
    public int Compare(StoreRandomItemData x, StoreRandomItemData y)
    {
        return y.Chance.CompareTo(x.Chance);
    }
}

public class RecordIDComparer : IComparer<RecordObject>
{
    public int Compare(RecordObject x,RecordObject y)
    {
        return x.RecordID.CompareTo(y.RecordID);
    }
}
