using System;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour {
    public enum Player {
        O,
        X,
        Neither,
        Checking
    }
    public Player currentTurn, winner;
    // public ScriptForCursor cursorManager;
    void Start() {
        currentTurn = Player.O;
        // cursorManager = FindFirstObjectByType<ScriptForCursor>();
        ResetScene();
    }

    #region ResetScene()系列
    public GameObject emptyCube;
    private int[,,] countBoard;
    private GameObject[,,] cubeBoard = new GameObject[4, 3, 3]; // 座標[7*(i-1), 7*(j-1), 7*(k-1)]
    public void ResetScene() {
        endScene.SetActive(false);
        OCanvas.SetActive(true);
        XCanvas.SetActive(false);

        currentTurn = Player.O;
        winner = Player.Neither;
        clickedCube = null;

        for (int x = 0; x < 4; x++) { // 清除舊方塊
            for (int y = 0; y < 3; y++) {
                for (int z = 0; z < 3; z++) {
                    Destroy(cubeBoard[x, y, z]);
                    if (cubeSelectBoard[x, y, z] != null) {
                        Destroy(cubeSelectBoard[x, y, z]);
                        cubeSelectBoard[x, y, z] = null;
                    }
                }
            }
        }
        countBoard = new int[4, 3, 3];

        for (int x = 0; x < 4; x++) {
            for (int y = 0; y < 3; y++) {
                for (int z = 0; z < 3; z++) {
                    Vector3 position = new Vector3(7 * (x - 1), 7 * (y - 1), 7 * (z - 1));
                    GameObject cube = Instantiate(emptyCube, position, Quaternion.identity);
                    if (x == 3) {
                        cube.SetActive(false);
                    } else {
                        cube.SetActive(true);
                    } // 先把3x3建出來
                    cubeBoard[x, y, z] = cube; // 存入 cubeBoard 陣列
                    cube.name = $"({x}, {y}, {z})EmptyCube";
                }
            }
        }
    }
    #endregion

    public GameObject OCanvas, XCanvas, endScene, OWinText, XWinText, checkBox;
    public GameObject OCube, XCube, TriangleCube, SelectEmptyCube, SelectOCube, SelectXCube, SelectTriangleCube;
    void Update() {
        switch (currentTurn) {
            case Player.Neither:
                // cursorManager.cursorState = ScriptForCursor.CursorState.Default;
                checkBox.SetActive(false);
                OCanvas.SetActive(false);
                XCanvas.SetActive(false);
                endScene.SetActive(true);
                ((winner == Player.O) ? XWinText : OWinText).SetActive(false);
                break;
            case Player.Checking:
                // cursorManager.cursorState = ScriptForCursor.CursorState.Default;
                break;
            default:
                if (currentTurn == Player.O) {
                    // cursorManager.cursorState = ScriptForCursor.CursorState.O;
                } else {
                    // cursorManager.cursorState = ScriptForCursor.CursorState.X;
                }
                IsNumberPress();
                CheckIfNextTurn();
                break;
        }
    }

    #region IsNumberPress()系列
    public UnityEngine.UI.Button surrenderButton;
    private int currentNumber = 0;

    public void IsNumberPress() {
        foreach (KeyCode key in new KeyCode[] {
            KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
            KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9,

            KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5,
            KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9, KeyCode.Keypad0,}) {

            if (Input.GetKeyDown(key)) {
                //切換數字時重置狀態
                for (int i = 0; i < 3; i++) {
                    for (int j = 0; j < 3; j++) {
                        for (int k = 0; k < 3; k++) {
                            if (cubeSelectBoard[i, j, k] != null) {
                                Destroy(cubeSelectBoard[i, j, k]);
                                cubeSelectBoard[i, j, k] = null;
                                if (OXboard[i, j, k] == null) {
                                    cubeBoard[i, j, k].SetActive(true);
                                }
                            }
                        }
                    }
                }
                currentNumber = 0;
                AllCubeActive();
                switch (key) {
                    case KeyCode.Alpha1:
                    case KeyCode.Keypad1:
                        currentNumber = 1;
                        SetRowActive(0);
                        break;
                    case KeyCode.Alpha2:
                    case KeyCode.Keypad2:
                        currentNumber = 2;
                        SetRowActive(1);
                        break;
                    case KeyCode.Alpha3:
                    case KeyCode.Keypad3:
                        currentNumber = 3;
                        SetRowActive(2);
                        break;
                    case KeyCode.Alpha4:
                    case KeyCode.Keypad4:
                        currentNumber = 4;
                        SetColumnActive(0);
                        break;
                    case KeyCode.Alpha5:
                    case KeyCode.Keypad5:
                        currentNumber = 5;
                        SetColumnActive(1);
                        break;
                    case KeyCode.Alpha6:
                    case KeyCode.Keypad6:
                        currentNumber = 6;
                        SetColumnActive(2);
                        break;
                    case KeyCode.Alpha7:
                    case KeyCode.Keypad7:
                        currentNumber = 7;
                        SetLayerActive(0);
                        break;
                    case KeyCode.Alpha8:
                    case KeyCode.Keypad8:
                        currentNumber = 8;
                        SetLayerActive(1);
                        break;
                    case KeyCode.Alpha9:
                    case KeyCode.Keypad9:
                        currentNumber = 9;
                        SetLayerActive(2);
                        break;
                }
                if (clickedCube != null && clickedCube.name.Contains("Cube")) {
                    SelectCubeInstantiate(clickedCube.name);
                }
            }
        }
    }

    //設定只激活一行
    void SetRowActive(int x) {
        for (int y = 0; y < 3; y++) {
            for (int z = 0; z < 3; z++) {
                cubeBoard[(x + 1) % 3, y, z].SetActive(false);
                cubeBoard[(x + 2) % 3, y, z].SetActive(false);
            }
        }
    }

    //設定只激活一列
    void SetColumnActive(int z) {
        for (int x = 0; x < 3; x++) {
            for (int y = 0; y < 3; y++) {
                cubeBoard[x, y, (z + 1) % 3].SetActive(false);
                cubeBoard[x, y, (z + 2) % 3].SetActive(false);
            }
        }
    }

    //設定只激活一層
    void SetLayerActive(int y) {
        for (int x = 0; x < 3; x++) {
            for (int z = 0; z < 3; z++) {
                cubeBoard[x, (y + 1) % 3, z].SetActive(false);
                cubeBoard[x, (y + 2) % 3, z].SetActive(false);
            }
        }
    }

    //激活3*3*3所有方塊
    void AllCubeActive() {
        for (int x = 0; x < 3; x++) {
            for (int y = 0; y < 3; y++) {
                for (int z = 0; z < 3; z++) {
                    if (OXboard[x, y, z] == null) {
                        cubeBoard[x, y, z].SetActive(true);
                    }
                }
            }
        }
    }
    #endregion

    void CheckIfNextTurn() {
        if (SomeoneHasWin()) {
            currentTurn = Player.Neither;
        } else if (IsMoveComplete()) {
            if (currentTurn == Player.O) {
                currentTurn = Player.X;
                OCanvas.SetActive(false);
                XCanvas.SetActive(true);
            } else {
                currentTurn = Player.O;
                OCanvas.SetActive(true);
                XCanvas.SetActive(false);
            }
        } else {
            int nowTurn = (currentTurn == Player.O) ? 1 : -1;
        }
    }

    #region SomeonesHasWin()系列
    public bool SomeoneHasWin() {
        int vs = 0;
        // 檢查每層、列、行以及對角線
        for (int i = 0; i < 3; i++) {
            vs += CheckLayer(i); // 檢查每一層
            vs += CheckColumn(i); // 檢查每一列
            vs += CheckRow(i); // 檢查每一行
        }

        // 檢查 3D 對角線
        vs += Check3DDiagonals();

        if (vs != 0) {
            winner = (vs > 0) ? Player.O : Player.X;
            return true;
        } else {
            return false;
        }
    }

    // 檢查某一層是否有玩家獲勝
    private int CheckLayer(int layer) {
        int vs = 0;
        // 檢查每一層的行和列
        for (int i = 0; i < 3; i++) {
            // 檢查行
            if (Mathf.Abs(countBoard[layer, i, 0] + countBoard[layer, i, 1] + countBoard[layer, i, 2]) == 3) {
                vs += countBoard[layer, i, 0];
            }

            // 檢查列
            if (Mathf.Abs(countBoard[layer, 0, i] + countBoard[layer, 1, i] + countBoard[layer, 2, i]) == 3) {
                vs += countBoard[layer, 0, i];
            }
        }

        // 檢查對角線
        if (Mathf.Abs(countBoard[layer, 0, 0] + countBoard[layer, 1, 1] + countBoard[layer, 2, 2]) == 3) {
            vs += countBoard[layer, 0, 0];
        }

        if (Mathf.Abs(countBoard[layer, 0, 2] + countBoard[layer, 1, 1] + countBoard[layer, 2, 0]) == 3) {
            vs += countBoard[layer, 0, 2];
        }
        return vs;
    }

    // 檢查縱向的列是否有玩家獲勝
    private int CheckColumn(int col) {
        int vs = 0;
        // 檢查每一列的行和列
        for (int i = 0; i < 3; i++) {
            // 檢查行
            if (Mathf.Abs(countBoard[i, 0, col] + countBoard[i, 1, col] + countBoard[i, 2, col]) == 3) {
                vs += countBoard[i, 0, col];
            }

            // 檢查列
            if (Mathf.Abs(countBoard[0, i, col] + countBoard[1, i, col] + countBoard[2, i, col]) == 3) {
                vs += countBoard[0, i, col];
            }

            // 特殊第四層
            if (Mathf.Abs(countBoard[1, i, col] + countBoard[2, i, col] + countBoard[3, i, col]) == 3) {
                vs += countBoard[1, i, col];
            }
        }

        // 檢查對角線
        if (Mathf.Abs(countBoard[0, 0, col] + countBoard[1, 1, col] + countBoard[2, 2, col]) == 3) {
            vs += countBoard[0, 0, col];
        }

        if (Mathf.Abs(countBoard[1, 0, col] + countBoard[2, 1, col] + countBoard[3, 2, col]) == 3) {
            vs += countBoard[0, 0, col];
        }

        if (Mathf.Abs(countBoard[0, 2, col] + countBoard[1, 1, col] + countBoard[2, 0, col]) == 3) {
            vs += countBoard[0, 2, col];
        }


        if (Mathf.Abs(countBoard[1, 2, col] + countBoard[2, 1, col] + countBoard[2, 0, col]) == 3) {
            vs += countBoard[1, 2, col];
        }

        return vs;
    }

    // 檢查橫向的行是否有玩家獲勝
    private int CheckRow(int row) {
        int vs = 0;
        // 檢查每一行的行和列
        for (int i = 0; i < 3; i++) {
            // 檢查行
            if (Mathf.Abs(countBoard[i, row, 0] + countBoard[i, row, 1] + countBoard[i, row, 2]) == 3) {
                vs += countBoard[i, row, 0];
            }

            // 檢查列
            if (Mathf.Abs(countBoard[0, row, i] + countBoard[1, row, i] + countBoard[2, row, i]) == 3) {
                vs += countBoard[0, row, i];
            }
            if (Mathf.Abs(countBoard[1, row, i] + countBoard[2, row, i] + countBoard[3, row, i]) == 3) {
                vs += countBoard[1, row, i];
            }
        }

        // 檢查對角線
        if (Mathf.Abs(countBoard[0, row, 0] + countBoard[1, row, 1] + countBoard[2, row, 2]) == 3) {
            vs += countBoard[0, row, 0];
        }
        if (Mathf.Abs(countBoard[1, row, 0] + countBoard[2, row, 1] + countBoard[3, row, 2]) == 3) {
            vs += countBoard[0, row, 0];
        }

        if (Mathf.Abs(countBoard[0, row, 2] + countBoard[1, row, 1] + countBoard[2, row, 0]) == 3) {
            vs += countBoard[0, row, 2];
        }
        if (Mathf.Abs(countBoard[1, row, 2] + countBoard[2, row, 1] + countBoard[3, row, 0]) == 3) {
            vs += countBoard[1, row, 2];
        }

        return vs;
    }

    // 檢查 3D 對角線是否有玩家獲勝
    private int Check3DDiagonals() {
        int vs = 0;
        if (Mathf.Abs(countBoard[0, 0, 0] + countBoard[1, 1, 1] + countBoard[2, 2, 2]) == 3) {
            vs += countBoard[0, 0, 0];
        }
        if (Mathf.Abs(countBoard[0, 0, 2] + countBoard[1, 1, 1] + countBoard[2, 2, 0]) == 3) {
            vs += countBoard[0, 0, 2];
        }
        if (Mathf.Abs(countBoard[0, 2, 0] + countBoard[1, 1, 1] + countBoard[2, 0, 2]) == 3) {
            vs += countBoard[0, 2, 0];
        }
        if (Mathf.Abs(countBoard[2, 0, 0] + countBoard[1, 1, 1] + countBoard[0, 2, 2]) == 3) {
            vs += countBoard[2, 0, 0];
        }
        if (Mathf.Abs(countBoard[1, 0, 0] + countBoard[2, 1, 1] + countBoard[3, 2, 2]) == 3) {
            vs += countBoard[1, 0, 0];
        }
        if (Mathf.Abs(countBoard[1, 0, 2] + countBoard[2, 1, 1] + countBoard[3, 2, 0]) == 3) {
            vs += countBoard[1, 0, 2];
        }
        if (Mathf.Abs(countBoard[1, 2, 0] + countBoard[2, 1, 1] + countBoard[3, 0, 2]) == 3) {
            vs += countBoard[1, 2, 0];
        }
        if (Mathf.Abs(countBoard[3, 0, 0] + countBoard[2, 1, 1] + countBoard[1, 2, 2]) == 3) {
            vs += countBoard[3, 0, 0];
        }

        return vs;
    }

    #endregion

    #region IsMoveComplete()系列
    private GameObject[,,] OXboard = new GameObject[5, 5, 5];
    private GameObject[,,] cubeSelectBoard = new GameObject[5, 5, 5];
    private GameObject tempCube, clickedCube;

    public bool IsMoveComplete() {
        // 判斷滑鼠點擊Cube
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                GameObject clickedObject = hit.collider.gameObject;

                //點擊時讓原本被點過的SelectCube消失
                if (clickedObject.name.Contains("Cube")) {
                    clickedCube = clickedObject;
                }
                for (int x = 0; x < 3; x++) {
                    for (int y = 0; y < 3; y++) {
                        for (int z = 0; z < 3; z++) {
                            if (cubeSelectBoard[x, y, z] != null) {
                                Destroy(cubeSelectBoard[x, y, z]);
                                cubeSelectBoard[x, y, z] = null;
                                if (OXboard[x, y, z] == null) {
                                    cubeBoard[x, y, z].SetActive(true);
                                }
                            }
                        }
                    }
                }
                // if (SetCubeActive(clickedCube.name)) {
                //     // 判斷按下上下鍵
                //     if (Input.GetKeyDown(KeyCode.UpArrow)) {
                //         return InputOX("UpArrow");
                //     } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                //         return InputOX("DownArrow");
                //     }
                // }

                SelectCubeInstantiate(clickedCube.name);
            }
        }
        foreach (KeyCode key in new KeyCode[] {
            KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.Q,

            KeyCode.UpArrow, KeyCode.DownArrow,

            KeyCode.Escape,
        }) {

            if (Input.GetKeyDown(key)) {
                switch (key) {
                    case KeyCode.W:
                    // isHoldingTriangle = true;
                    // return false;
                    case KeyCode.A:
                        // spin();
                        return true;
                    case KeyCode.S:
                        // UpsideDown();
                        return true;
                    case KeyCode.Q:
                        surrenderButton.onClick.Invoke();
                        return false;
                    case KeyCode.UpArrow:
                        return InputOX("UpArrow", clickedCube?.name);
                    case KeyCode.DownArrow:
                        return InputOX("DownArrow", clickedCube?.name);
                    // case KeyCode.Escape:
                    default:
                        return false;
                }
            }
        }

        return false;
    }
    #endregion

    #region inputOX()系列
    // 輸入O或X
    private bool InputOX(string pressUpDownArrow, string clickedCube) {
        //顯示全部時無效
        if (currentNumber == 0) {
            return false;
        }

        int count = 0;

        for (int i = 0; i < 3; i++) {
            for (int k = 0; k < 3; k++) {
                if (pressUpDownArrow == "DownArrow" && clickedCube.Contains($"({i}") && clickedCube.Contains($"{k})")) {
                    for (int y = 0; y < 3; y++) {
                        if (countBoard[i, y, k] != 0) {
                            count++;
                        }
                    }
                    // 如果該列有空位
                    if (count != 3) {
                        //最下層有空位
                        if (countBoard[i, 0, k] == 0) {
                            cubeSelectBoard[i, 0, k].SetActive(false);
                            //直接生成
                            if (currentTurn == Player.O) {
                                tempCube = Instantiate(OCube, cubeBoard[i, 0, k].transform.position, Quaternion.identity);
                                tempCube.SetActive(true);
                                OXboard[i, 0, k] = tempCube;
                                OXboard[i, 0, k].name = $"({i}, {0}, {k})OCube";
                                countBoard[i, 0, k] = 1;
                            } else if (currentTurn == Player.X) {
                                tempCube = Instantiate(XCube, cubeBoard[i, 0, k].transform.position, Quaternion.identity);
                                tempCube.SetActive(true);
                                OXboard[i, 0, k] = tempCube;
                                OXboard[i, 0, k].name = $"({i}, {0}, {k})XCube";
                                countBoard[i, 0, k] = -1;
                            }
                            return true;

                            //將最下層移至第二層後再生成最下層
                        } else if (countBoard[i, 0, k] != 0 && countBoard[i, 1, k] == 0) {
                            cubeSelectBoard[i, 1, k].SetActive(false);
                            if (currentTurn == Player.O) {
                                OXboard[i, 1, k] = Instantiate(OXboard[i, 0, k], cubeBoard[i, 1, k].transform.position, Quaternion.identity);
                                if (OXboard[i, 0, k] != null) {
                                    Destroy(OXboard[i, 0, k]);  // 刪除舊物件
                                }
                                OXboard[i, 1, k].name = $"({i}, {1}, {k})OCube";

                                tempCube = Instantiate(OCube, cubeBoard[i, 0, k].transform.position, Quaternion.identity);
                                tempCube.SetActive(true);

                                OXboard[i, 0, k] = tempCube;
                                OXboard[i, 0, k].name = $"({i}, {0}, {k})OCube";
                                countBoard[i, 1, k] = countBoard[i, 0, k];
                                countBoard[i, 0, k] = 1;
                            } else if (currentTurn == Player.X) {
                                OXboard[i, 1, k] = Instantiate(OXboard[i, 0, k], cubeBoard[i, 1, k].transform.position, Quaternion.identity);
                                if (OXboard[i, 0, k] != null) {
                                    Destroy(OXboard[i, 0, k]);  // 刪除舊物件
                                }
                                OXboard[i, 1, k].name = $"({i}, {1}, {k})XCube";

                                tempCube = Instantiate(XCube, cubeBoard[i, 0, k].transform.position, Quaternion.identity);
                                tempCube.SetActive(true);

                                OXboard[i, 0, k] = tempCube;
                                OXboard[i, 0, k].name = $"({i}, {0}, {k})XCube";
                                countBoard[i, 1, k] = countBoard[i, 0, k];
                                countBoard[i, 0, k] = -1;
                            }
                            return true;

                            //將第二層移至第三層後將第一層移至第二層後再生成第一層
                        } else if (countBoard[i, 0, k] != 0 && countBoard[i, 1, k] != 0 && countBoard[i, 2, k] == 0) {
                            cubeSelectBoard[i, 2, k].SetActive(false);
                            if (currentTurn == Player.O) {
                                OXboard[i, 2, k] = Instantiate(OXboard[i, 1, k], cubeBoard[i, 2, k].transform.position, Quaternion.identity);
                                if (OXboard[i, 1, k] != null) {
                                    Destroy(OXboard[i, 1, k]);  // 刪除舊物件
                                }
                                OXboard[i, 2, k].name = $"({i}, {2}, {k})OCube";

                                OXboard[i, 1, k] = Instantiate(OXboard[i, 0, k], cubeBoard[i, 1, k].transform.position, Quaternion.identity);
                                if (OXboard[i, 0, k] != null) {
                                    Destroy(OXboard[i, 0, k]);  // 刪除舊物件
                                }
                                OXboard[i, 1, k].name = $"({i}, {1}, {k})OCube";

                                tempCube = Instantiate(OCube, cubeBoard[i, 0, k].transform.position, Quaternion.identity);
                                OXboard[i, 0, k].SetActive(true);

                                OXboard[i, 0, k] = tempCube;
                                OXboard[i, 0, k].name = $"({i}, {0}, {k})OCube";
                                countBoard[i, 2, k] = countBoard[i, 1, k];
                                countBoard[i, 1, k] = countBoard[i, 0, k];
                                countBoard[i, 0, k] = 1;
                            } else if (currentTurn == Player.X) {
                                OXboard[i, 2, k] = Instantiate(OXboard[i, 1, k], cubeBoard[i, 2, k].transform.position, Quaternion.identity);
                                if (OXboard[i, 1, k] != null) {
                                    Destroy(OXboard[i, 1, k]);  // 刪除舊物件
                                }
                                OXboard[i, 2, k].name = $"({i}, {2}, {k})XCube";

                                OXboard[i, 1, k] = Instantiate(OXboard[i, 0, k], cubeBoard[i, 1, k].transform.position, Quaternion.identity);
                                if (OXboard[i, 0, k] != null) {
                                    Destroy(OXboard[i, 0, k]);  // 刪除舊物件
                                }

                                OXboard[i, 1, k].name = $"({i}, {1}, {k})XCube";

                                tempCube = Instantiate(XCube, cubeBoard[i, 0, k].transform.position, Quaternion.identity);
                                tempCube.SetActive(true);

                                OXboard[i, 0, k] = tempCube;
                                OXboard[i, 0, k].name = $"({i}, {0}, {k})XCube";
                                countBoard[i, 2, k] = countBoard[i, 1, k];
                                countBoard[i, 1, k] = countBoard[i, 0, k];
                                countBoard[i, 0, k] = -1;
                            }
                            return true;
                        }
                    }
                    count = 0;
                    return false;
                } else if (pressUpDownArrow == "UpArrow" && clickedCube.Contains($"({i}") && clickedCube.Contains($"{k})")) {
                    for (int y = 2; y >= 0; y--) {
                        if (countBoard[i, y, k] != 0) {
                            count++;
                        }
                    }
                    if (count != 3) {
                        if (countBoard[i, 2, k] == 0) {//只有最上層有空位，直接生成到最上層
                            if (countBoard[i, 1, k] == 0) { //最上層有空位且中層也有，將方塊生成到中層
                                if (countBoard[i, 0, k] == 0) { //最上層有空位且中下層也有，將方塊生成到最下層
                                    cubeSelectBoard[i, 0, k].SetActive(false);
                                    if (currentTurn == Player.O) {
                                        tempCube = Instantiate(OCube, cubeBoard[i, 0, k].transform.position, Quaternion.identity);
                                        tempCube.SetActive(true);
                                        OXboard[i, 0, k] = tempCube;
                                        countBoard[i, 0, k] = 1;
                                    } else if (currentTurn == Player.X) {
                                        tempCube = Instantiate(XCube, cubeBoard[i, 0, k].transform.position, Quaternion.identity);
                                        tempCube.SetActive(true);
                                        OXboard[i, 0, k] = tempCube;
                                        countBoard[i, 0, k] = -1;
                                    }
                                    return true;
                                }

                                cubeSelectBoard[i, 1, k].SetActive(false);
                                if (currentTurn == Player.O) {
                                    tempCube = Instantiate(OCube, cubeBoard[i, 1, k].transform.position, Quaternion.identity);
                                    tempCube.SetActive(true);
                                    OXboard[i, 1, k] = tempCube;
                                    countBoard[i, 1, k] = 1;
                                } else if (currentTurn == Player.X) {
                                    tempCube = Instantiate(XCube, cubeBoard[i, 1, k].transform.position, Quaternion.identity);
                                    tempCube.SetActive(true);
                                    OXboard[i, 1, k] = tempCube;
                                    countBoard[i, 1, k] = -1;
                                }
                                return true;
                            }
                            cubeSelectBoard[i, 2, k].SetActive(false);
                            if (currentTurn == Player.O) {
                                tempCube = Instantiate(OCube, cubeBoard[i, 2, k].transform.position, Quaternion.identity);
                                tempCube.SetActive(true);

                                OXboard[i, 2, k] = tempCube;
                                countBoard[i, 2, k] = 1;
                            } else if (currentTurn == Player.X) {
                                tempCube = Instantiate(XCube, cubeBoard[i, 2, k].transform.position, Quaternion.identity);
                                tempCube.SetActive(true);
                                OXboard[i, 2, k] = tempCube;
                                countBoard[i, 2, k] = -1;
                            }
                            return true;
                        }
                        count = 0;
                        return false;
                    }
                }
            }
        }
        return false;
    }

    #endregion

    #region SelectCubeInstantiate()系列
    private GameObject tempSelectCube;

    //設定cube根據點擊激活變為selectCube
    void SelectCubeInstantiate(string clickedCubeName) {
        int i = clickedCubeName[1] - '0', k = clickedCubeName[7] - '0'; //這樣點到場景會有問題，雖然不會終止遊戲
        int tempNumber;
        switch (currentNumber) {
            case 1:
            case 2:
            case 3:
                tempNumber = currentNumber - 1;
                if (clickedCubeName.Contains($"({tempNumber}, ") && clickedCubeName.Contains($", {k})")) {
                    for (int j = 0; j < 3; j++) {
                        if (countBoard[tempNumber, j, k] == 0) {
                            tempSelectCube = Instantiate(SelectEmptyCube, cubeBoard[tempNumber, j, k].transform.position, Quaternion.identity);
                            tempSelectCube.SetActive(true);
                            cubeBoard[tempNumber, j, k].SetActive(false);
                            cubeSelectBoard[tempNumber, j, k] = tempSelectCube;
                            tempSelectCube.name = $"({tempNumber}, {j}, {k})SelectEmptyCube";
                        } else if (countBoard[tempNumber, j, k] == 1) {
                            tempSelectCube = Instantiate(SelectOCube, cubeBoard[tempNumber, j, k].transform.position, Quaternion.identity);
                            tempSelectCube.SetActive(true);
                            cubeBoard[tempNumber, j, k].SetActive(false);
                            cubeSelectBoard[tempNumber, j, k] = tempSelectCube;
                            tempSelectCube.name = $"({tempNumber}, {j}, {k})SelectOCube";
                        } else if (countBoard[tempNumber, j, k] == -1) {
                            tempSelectCube = Instantiate(SelectXCube, cubeBoard[tempNumber, j, k].transform.position, Quaternion.identity);
                            tempSelectCube.SetActive(true);
                            cubeBoard[tempNumber, j, k].SetActive(false);
                            cubeSelectBoard[tempNumber, j, k] = tempSelectCube;
                            tempSelectCube.name = $"({tempNumber}, {j}, {k})SelectXCube";
                        }
                    }
                }
                break;

            case 4:
            case 5:
            case 6:
                tempNumber = currentNumber - 4;
                if (clickedCubeName.Contains($"({i}, ") && clickedCubeName.Contains($", {tempNumber})")) {
                    for (int j = 0; j < 3; j++) {
                        if (countBoard[i, j, tempNumber] == 0) {
                            tempSelectCube = Instantiate(SelectEmptyCube, cubeBoard[i, j, tempNumber].transform.position, Quaternion.identity);
                            tempSelectCube.SetActive(true);
                            cubeBoard[i, j, tempNumber].SetActive(false);
                            cubeSelectBoard[i, j, tempNumber] = tempSelectCube;
                            tempSelectCube.name = $"({i}, {j}, {tempNumber})SelectEmptyCube";
                        } else if (countBoard[i, j, tempNumber] == 1) {
                            tempSelectCube = Instantiate(SelectOCube, cubeBoard[i, j, tempNumber].transform.position, Quaternion.identity);
                            tempSelectCube.SetActive(true);
                            cubeBoard[i, j, tempNumber].SetActive(false);
                            cubeSelectBoard[i, j, tempNumber] = tempSelectCube;
                            tempSelectCube.name = $"({i}, {j}, {tempNumber})SelectOCube";
                        } else if (countBoard[i, j, tempNumber] == -1) {
                            tempSelectCube = Instantiate(SelectXCube, cubeBoard[i, j, tempNumber].transform.position, Quaternion.identity);
                            tempSelectCube.SetActive(true);
                            cubeBoard[i, j, tempNumber].SetActive(false);
                            cubeSelectBoard[i, j, tempNumber] = tempSelectCube;
                            tempSelectCube.name = $"({i}, {j}, {tempNumber})SelectXCube";
                        }
                    }
                }
                break;

            case 7:
            case 8:
            case 9:
                tempNumber = currentNumber - 7;
                if (countBoard[i, tempNumber, k] == 0) {
                    tempSelectCube = Instantiate(SelectEmptyCube, cubeBoard[i, tempNumber, k].transform.position, Quaternion.identity);
                    tempSelectCube.SetActive(true);
                    cubeBoard[i, tempNumber, k].SetActive(false);
                    cubeSelectBoard[i, tempNumber, k] = tempSelectCube;
                    tempSelectCube.name = $"({i}, {tempNumber}, {k})SelectEmptyCube";
                } else if (countBoard[i, tempNumber, k] == 1) {
                    tempSelectCube = Instantiate(SelectOCube, cubeBoard[i, tempNumber, k].transform.position, Quaternion.identity);
                    tempSelectCube.SetActive(true);
                    cubeBoard[i, tempNumber, k].SetActive(false);
                    cubeSelectBoard[i, tempNumber, k] = tempSelectCube;
                    tempSelectCube.name = $"({i}, {tempNumber}, {k})SelectOCube";
                } else if (countBoard[i, tempNumber, k] == -1) {
                    tempSelectCube = Instantiate(SelectXCube, cubeBoard[i, tempNumber, k].transform.position, Quaternion.identity);
                    tempSelectCube.SetActive(true);
                    cubeBoard[i, tempNumber, k].SetActive(false);
                    cubeSelectBoard[i, tempNumber, k] = tempSelectCube;
                    tempSelectCube.name = $"({i}, {tempNumber}, {k})SelectXCube";
                }
                break;

            case 0:
                for (int j = 0; j < 3; j++) {
                    if (countBoard[i, j, k] == 0) {
                        tempSelectCube = Instantiate(SelectEmptyCube, cubeBoard[i, j, k].transform.position, Quaternion.identity);
                        tempSelectCube.SetActive(true);
                        cubeBoard[i, j, k].SetActive(false);
                        cubeSelectBoard[i, j, k] = tempSelectCube;
                        tempSelectCube.name = $"({i}, {j}, {k})SelectEmptyCube";
                    } else if (countBoard[i, j, k] == 1) {
                        tempSelectCube = Instantiate(SelectOCube, cubeBoard[i, j, currentNumber - 3].transform.position, Quaternion.identity);
                        tempSelectCube.SetActive(true);
                        cubeBoard[i, j, currentNumber].SetActive(false);
                        cubeSelectBoard[i, j, currentNumber] = tempSelectCube;
                        tempSelectCube.name = $"({i}, {j}, {currentNumber})SelectOCube";
                    } else if (countBoard[i, j, currentNumber] == -1) {
                        tempSelectCube = Instantiate(SelectXCube, cubeBoard[i, j, k - 3].transform.position, Quaternion.identity);
                        tempSelectCube.SetActive(true);
                        cubeBoard[i, j, currentNumber].SetActive(false);
                        cubeSelectBoard[i, j, currentNumber] = tempSelectCube;
                        tempSelectCube.name = $"({i}, {j}, {currentNumber})SelectXCube";
                    }
                }
                break;
        }
    }
    #endregion
}