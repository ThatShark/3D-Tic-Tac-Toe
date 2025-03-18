using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour {
    public enum Player {
        O,
        X,
        Neither,
        Checking
    }
    public Player currentTurn, winner;
    // private ScriptForCursor cursorManager;
    private ScriptForButton buttonManager;
    void Start() {
        currentTurn = Player.O;
        // cursorManager = FindFirstObjectByType<ScriptForCursor>();
        buttonManager = FindFirstObjectByType<ScriptForButton>();
        ResetScene();
    }

    #region ResetScene()系列
    public GameObject emptyCube;
    private int[,,] countBoard;
    private GameObject[,,] cubeBoard = new GameObject[4, 3, 3]; // 座標[7*(i-1), 7*(j-1), 7*(k-1)]
    public void ResetScene() {
        XWinText.SetActive(true);
        OWinText.SetActive(true);
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
                    GameObject cube = Instantiate(emptyCube, new Vector3(7 * (x - 1), 7 * (y - 1), 7 * (z - 1)), Quaternion.identity);
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
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    buttonManager.SwitchSceneTo(0);
                } else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                    ResetScene();
                }
                break;
            case Player.Checking:
                // cursorManager.cursorState = ScriptForCursor.CursorState.Default;
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    buttonManager.Cancel();
                } else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                    buttonManager.SurrenderSkill();
                }
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

    void IsNumberPress() {
        foreach (KeyCode key in new KeyCode[] {
            KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
            KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9,

            KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5,
            KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9, KeyCode.Keypad0,}) {

            if (Input.GetKeyDown(key)) {
                //切換數字時重置狀態
                DestroyAllSelect();
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
                SelectCubeInstantiate(clickedCube?.name);
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
                    cubeBoard[x, y, z].SetActive(true);
                }
            }
        }
    }
    #endregion

    void DestroyAllSelect() {
        for (int x = 0; x < 3; x++) {
            for (int y = 0; y < 3; y++) {
                for (int z = 0; z < 3; z++) {
                    if (cubeSelectBoard[x, y, z] != null) {
                        Destroy(cubeSelectBoard[x, y, z]);
                        cubeSelectBoard[x, y, z] = null;
                        cubeBoard[x, y, z].SetActive(true);
                    }
                }
            }
        }
    }

    void CheckIfNextTurn() {
        if (SomeoneHasWin()) {
            currentTurn = Player.Neither;
        } else if (IsMoveComplete()) {
            clickedCube = null;
            currentNumber = 0;
            DestroyAllSelect();
            AllCubeActive();
            if (currentTurn == Player.O) {
                currentTurn = Player.X;
                OCanvas.SetActive(false);
                XCanvas.SetActive(true);
            } else {
                currentTurn = Player.O;
                OCanvas.SetActive(true);
                XCanvas.SetActive(false);
            }
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
    int CheckLayer(int layer) {
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
    int CheckColumn(int col) {
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
    int CheckRow(int row) {
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
    int Check3DDiagonals() {
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
    private GameObject[,,] cubeSelectBoard = new GameObject[5, 5, 5];
    private GameObject clickedCube = null;

    bool IsMoveComplete() {
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
                if (!clickedObject.name.Contains("Select")) {
                    DestroyAllSelect();
                    SelectCubeInstantiate(clickedCube?.name);
                }
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
                        return InputOX(true, clickedCube?.name);
                    case KeyCode.DownArrow:
                        return InputOX(false, clickedCube?.name);

                    case KeyCode.Escape:
                        clickedCube = null;
                        DestroyAllSelect();
                        return false;
                }
            }
        }
        return false;
    }
    #endregion

    #region inputOX()系列
    public GameObject ErrorCanvas;
    // 輸入O或X
    bool InputOX(bool pressUpArrow, string clickedCubeNName) { // true: up, false: down
        //非點選方塊時無效
        if (clickedCubeNName == null || !clickedCubeNName.Contains("Cube")) {
            return false;
        }
        int emptyCubeCount = 0;
        int i = clickedCubeNName[1] - '0', k = clickedCubeNName[7] - '0';
        for (int y = 0; y < 3; y++) {
            if (countBoard[i, y, k] == 0) {
                emptyCubeCount++;
            }
        }
        if (emptyCubeCount == 0) {
            ErrorCanvas.SetActive(true);
            StartCoroutine(DelayedSetNotActive(1.5f));
            return false;
        }
        if (pressUpArrow) {
            if (countBoard[i, 1, k] != 0) {
                Destroy(cubeBoard[i, 2, k]);
                if (currentTurn == Player.O) {
                    cubeBoard[i, 2, k] = Instantiate(OCube, new Vector3(7 * (i - 1), 7, 7 * (k - 1)), Quaternion.identity);
                    cubeBoard[i, 2, k].name = $"({i}, 2, {k})OCube";
                    countBoard[i, 2, k] = 1;
                } else if (currentTurn == Player.X) {
                    cubeBoard[i, 2, k] = Instantiate(XCube, new Vector3(7 * (i - 1), 7, 7 * (k - 1)), Quaternion.identity);
                    cubeBoard[i, 2, k].name = $"({i}, 2, {k})XCube";
                    countBoard[i, 2, k] = -1;
                }
            } else if (countBoard[i, 0, k] != 0) {
                Destroy(cubeBoard[i, 1, k]);
                if (currentTurn == Player.O) {
                    cubeBoard[i, 1, k] = Instantiate(OCube, new Vector3(7 * (i - 1), 0, 7 * (k - 1)), Quaternion.identity);
                    cubeBoard[i, 1, k].name = $"({i}, 1, {k})OCube";
                    countBoard[i, 1, k] = 1;
                } else if (currentTurn == Player.X) {
                    cubeBoard[i, 1, k] = Instantiate(XCube, new Vector3(7 * (i - 1), 0, 7 * (k - 1)), Quaternion.identity);
                    cubeBoard[i, 1, k].name = $"({i}, 1, {k})XCube";
                    countBoard[i, 1, k] = -1;
                }
            } else {
                Destroy(cubeBoard[i, 0, k]);
                if (currentTurn == Player.O) {
                    cubeBoard[i, 0, k] = Instantiate(OCube, new Vector3(7 * (i - 1), -7, 7 * (k - 1)), Quaternion.identity);
                    cubeBoard[i, 0, k].name = $"({i}, 0, {k})OCube";
                    countBoard[i, 0, k] = 1;
                } else if (currentTurn == Player.X) {
                    cubeBoard[i, 0, k] = Instantiate(XCube, new Vector3(7 * (i - 1), -7, 7 * (k - 1)), Quaternion.identity);
                    cubeBoard[i, 0, k].name = $"({i}, 0, {k})XCube";
                    countBoard[i, 0, k] = -1;
                }
            }
        } else {
            Destroy(cubeBoard[i, 2, k]);
            cubeBoard[i, 2, k] = Instantiate(cubeBoard[i, 1, k], new Vector3(7 * (i - 1), 7, 7 * (k - 1)), Quaternion.identity);
            cubeBoard[i, 2, k].name = nameCube(i, 2, k, countBoard[i, 1, k]);
            countBoard[i, 2, k] = countBoard[i, 1, k];

            Destroy(cubeBoard[i, 1, k]);
            cubeBoard[i, 1, k] = Instantiate(cubeBoard[i, 0, k], new Vector3(7 * (i - 1), 0, 7 * (k - 1)), Quaternion.identity);
            cubeBoard[i, 1, k].name = nameCube(i, 1, k, countBoard[i, 0, k]);
            countBoard[i, 1, k] = countBoard[i, 0, k];

            Destroy(cubeBoard[i, 0, k]);
            if (currentTurn == Player.O) {
                cubeBoard[i, 0, k] = Instantiate(OCube, new Vector3(7 * (i - 1), -7, 7 * (k - 1)), Quaternion.identity);
                cubeBoard[i, 0, k].name = $"({i}, 0, {k})OCube";
                countBoard[i, 0, k] = 1;
            } else if (currentTurn == Player.X) {
                cubeBoard[i, 0, k] = Instantiate(XCube, new Vector3(7 * (i - 1), -7, 7 * (k - 1)), Quaternion.identity);
                cubeBoard[i, 0, k].name = $"({i}, 0, {k})XCube";
                countBoard[i, 0, k] = -1;
            }
        }
        return true;
    }

    IEnumerator DelayedSetNotActive(float delayTime) {
        yield return new WaitForSeconds(delayTime);  // 延遲指定的時間
        ErrorCanvas.SetActive(false);
    }
    string nameCube(int i, int j, int k, int cubeType) {
        if (cubeType == 1) {
            return $"({i}, {j}, {k})OCube";
        } else if (cubeType == -1) {
            return $"({i}, {j}, {k})XCube";
        } else {
            return $"({i}, {j}, {k})EmptyCube";
        }

    }

    #endregion

    #region SelectCubeInstantiate()系列
    //設定cube根據點擊激活變為selectCube
    void SelectCubeInstantiate(string clickedCubeName) {
        if (clickedCubeName == null) {
            return;
        }
        int i = clickedCubeName[1] - '0', k = clickedCubeName[7] - '0';
        int tempNumber;
        switch (currentNumber) {
            case 1:
            case 2:
            case 3:
                tempNumber = currentNumber - 1;
                if (clickedCubeName.Contains($"({tempNumber}") && clickedCubeName.Contains($"{k})")) {
                    for (int j = 0; j < 3; j++) {
                        InstantiateSelectCube(tempNumber, j, k, countBoard[tempNumber, j, k]);
                    }
                }
                break;

            case 4:
            case 5:
            case 6:
                tempNumber = currentNumber - 4;
                if (clickedCubeName.Contains($"({i}") && clickedCubeName.Contains($"{tempNumber})")) {
                    for (int j = 0; j < 3; j++) {
                        InstantiateSelectCube(i, j, tempNumber, countBoard[i, j, tempNumber]);
                    }
                }
                break;

            case 7:
            case 8:
            case 9:
                tempNumber = currentNumber - 7;
                InstantiateSelectCube(i, tempNumber, k, countBoard[i, tempNumber, k]);
                break;

            case 0:
                for (int j = 0; j < 3; j++) {
                    InstantiateSelectCube(i, j, k, countBoard[i, j, k]);
                }
                break;
        }
    }

    void InstantiateSelectCube(int i, int j, int k, int cubeType) {
        GameObject tempSelectCube;
        Vector3 position = cubeBoard[i, j, k].transform.position;

        if (cubeType == 1) {
            tempSelectCube = Instantiate(SelectOCube, position, Quaternion.identity);
        } else if (cubeType == -1) {
            tempSelectCube = Instantiate(SelectXCube, position, Quaternion.identity);
        } else {
            tempSelectCube = Instantiate(SelectEmptyCube, position, Quaternion.identity);
        }

        tempSelectCube.SetActive(true);
        cubeBoard[i, j, k].SetActive(false);
        cubeSelectBoard[i, j, k] = tempSelectCube;

        if (cubeType == 1) {
            cubeSelectBoard[i, j, k].name = $"({i}, {j}, {k})SelectOCube";
        } else if (cubeType == -1) {
            cubeSelectBoard[i, j, k].name = $"({i}, {j}, {k})SelectXCube";
        } else {
            cubeSelectBoard[i, j, k].name = $"({i}, {j}, {k})SelectEmptyCube";
        }
    }
    #endregion
}