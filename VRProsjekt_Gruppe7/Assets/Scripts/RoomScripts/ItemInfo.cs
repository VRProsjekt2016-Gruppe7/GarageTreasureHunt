using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public string ItemName;
    public int Value;
    public int SizeValue;
    public GameObject Owner;
    private float _maxDistFromBox = 0.4f;
    private bool _inBox = true;

    public int GetValue()
    {
        return Value;
    }

    void Update()
    {
        if (Owner == null)
            return;

        float dist = Vector3.Distance(transform.position, Owner.transform.position);

        if (dist > 0.4f && _inBox)
        {
            _inBox = false;
            RemoveFromBox();
            print("Too far away from the box: " + dist);
        }
    }

    private void RemoveFromBox()
    {
        Owner.GetComponent<BoxInfo>().RemoveMe(gameObject);
    }

    public void SetValues(GameObject owner, int value, int sizeValue)
    {
        if (value < 0 || sizeValue < 0)
        {
            Debug.Log("Values cannot be a negative number! Setting to default values: 0.");
            Value = 0;
            SizeValue = 0;
            return;
        }

        Owner = owner;
        Value = value;
        SizeValue = sizeValue;
    }
}
