using UnityEngine;
using System.Collections;

public class ItemInfo : MonoBehaviour
{
    private int _value;

    public int GetValue()
    {
        return _value;
    }
    
    public void SetValue(int value)
    {
        if(value < 0)
        {
            Debug.Log("Value cannot be a negative number! Setting to default value: 0.");
            _value = 0;
            return;
        }

        _value = value;
    }
}
