using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public string ItemName;
    public int Value;
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


    public void SetValue(GameObject owner, int value)
    {
        if(value < 0)
        {
            Debug.Log("Value cannot be a negative number! Setting to default value: 0.");
            Value = 0;
            return;
        }

        Owner = owner;
        Value = value;
    }

}
