using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    
    public GameObject cubePrefab;
    public int height, width;
    public Cube[,] cells;
    public Color[] colorArray;
    public GameObject gameOverText;
    bool isGameOver = false;
    private void Awake()
    {
        cells = new Cube[height, width];
    }
    private void Update()
    {
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
                ResetGame();
            else
                return;

        }

        if (Input.GetKeyDown(KeyCode.W)) DoSomething(Vector2.up);
        else if (Input.GetKeyDown(KeyCode.S)) DoSomething(Vector2.down);
        else if (Input.GetKeyDown(KeyCode.D)) DoSomething(Vector2.right);
        else if (Input.GetKeyDown(KeyCode.A)) DoSomething(Vector2.left);
    }
    void DoSomething(Vector2 direction)
    {
        bool to = direction.x == 0;

        int startH = direction.y <= 0 ? 0 : height - 2;
        bool wayH = direction.y <= 0;

        int startW = direction.x <= 0 ? 0 : width - 2;
        bool wayW = direction.x <= 0;

        for (int h = startH; wayH ? h < height : h >= 0; h += (wayH ? +1 : -1))
        {
            for (int w = startW; wayW ? w < width : w >= 0; w += (wayW ? +1 : -1))
            {
                if (cells[h, w] != null)
                {
                    Cube cube = cells[h, w];
                    int horizontal, vertical, nh, nw;
                    nh = h;
                    nw = w;
                    while (true)
                    {
                        horizontal = nw + (to ? 0 : (wayW ? -1 : +1));
                        vertical = nh + (to ? (wayH ? -1 : +1) : 0);
                        if (vertical >= 0 && vertical < height && horizontal >= 0 && horizontal < width)
                        {
                            if (cells[vertical, horizontal] != null)
                            {
                                if (cells[vertical, horizontal].cubeNumber == cube.cubeNumber)
                                {
                                    Destroy(cells[vertical, horizontal].cubeTransform.gameObject);
                                    cube.Update(colorArray[cube.cubeNumber + 1], cube.cubeNumber + 1);
                                    cube.cubeTransform.position = new Vector3(horizontal, vertical, 0);
                                    cells[vertical, horizontal] = cube;
                                    cells[nh, nw] = null;
                                    nh = vertical;
                                    nw = horizontal;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                cube.cubeTransform.position = new Vector3(horizontal, vertical, 0);
                                cells[vertical, horizontal] = cube;
                                cells[nh, nw] = null;
                                nw = horizontal;
                                nh = vertical;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
        CreateCube();
    }
    void CreateCube()
    {
        Vector2 newpos = GetRandomEmptyLocation();
        if (newpos == Vector2.one * -2)
        {
            EndGame();
            return;
        }
        Cube newCube = new Cube(Instantiate(cubePrefab, newpos, Quaternion.identity).transform);
        newCube.Update(colorArray[0], 0);
        cells[(int)newpos.y, (int)newpos.x] = newCube;
    }
    Vector2 GetRandomEmptyLocation()
    {
        List<Vector2> emptypos = new List<Vector2>();
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                if (cells[h, w] == null)
                    emptypos.Add(new Vector2((float)w, (float)h));
            }
        }
        if (emptypos.Count <= 0)
        {
            return new Vector2(-2, -2);
        }
        return emptypos[Random.Range(0, emptypos.Count)];
    }
    private void EndGame()
    {
        gameOverText.SetActive(true);
        isGameOver = true;
    }
    public void ResetGame()
    {
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                if (cells[h,w]!=null)
                {
                    Destroy(cells[h, w].cubeTransform.gameObject);
                    cells[h, w] = null;
                }
            }
        }
        isGameOver = false;
        gameOverText.SetActive(false);
    }
}
