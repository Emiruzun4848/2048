using UnityEngine;
using TMPro;

public class Cube
{
    public Transform cubeTransform;
    public int cubeNumber;
    public Cube(Transform cubeObj) => cubeTransform = cubeObj;

    public void Update(Color color, int number)
    {
        cubeTransform.GetComponent<SpriteRenderer>().color = color;
        cubeNumber = number;
        cubeTransform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = Mathf.Pow(2, cubeNumber + 1).ToString();
    }
}
