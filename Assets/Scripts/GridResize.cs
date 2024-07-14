using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridResize : MonoBehaviour
{
    public GameObject container;
    public float offset;
    // Update is called once per frame
    void Update()
    {
        float width = container.GetComponent<RectTransform>().rect.width;
        Vector2 newSize = new Vector2(width/offset, width/offset);
        container.GetComponent<GridLayoutGroup>().cellSize = newSize;
    }
}
