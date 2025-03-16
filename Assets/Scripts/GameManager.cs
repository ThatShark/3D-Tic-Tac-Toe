using System;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public enum Player {
        O,
        X,
        Neither,
        Checking
    }
    public Player currentTurn, winner;
    public ScriptForCursor cursorManager;

    void Start() {
        currentTurn = Player.O;
        cursorManager = FindFirstObjectByType<ScriptForCursor>();
        ResetScene();
    }

    public GameObject OCanvas, XCanvas, endScene, OWinText, XWinText, checkBox;

    private int currentNumber = 0;

    public GameObject OCube, TriangleCube, XCube;
    void Update() {
        switch (currentTurn) {
            case Player.Neither:
                cursorManager.cursorState = ScriptForCursor.CursorState.Default;
                checkBox.SetActive(false);
                OCanvas.SetActive(false);
                XCanvas.SetActive(false);
                endScene.SetActive(true);
                ((winner == Player.O) ? XWinText : OWinText).SetActive(false);
                break;
            case Player.Checking:
                cursorManager.cursorState = ScriptForCursor.CursorState.Default;
                break;
            default:
                if (currentTurn == Player.O) {
                    cursorManager.cursorState = ScriptForCursor.CursorState.O;
                } else {
                    cursorManager.cursorState = ScriptForCursor.CursorState.X;
                }
                IsNumberPress();
                CheckIfNextTurn();
                break;
        }
    }

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

    public UnityEngine.UI.Button surrenderButton;
    public void IsNumberPress() {
        foreach (KeyCode key in new KeyCode[] {
            KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
            KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9,

            KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5,
            KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9, KeyCode.Keypad0,}) {

            if (Input.GetKeyDown(key)) {
                currentNumber = 0;
                AllCubeActive();
                switch (key) {
                    case KeyCode.Alpha1:
                    case KeyCode.Keypad1:
                        currentNumber = 1;
                        SetRowNotActive(2, 3);
                        break;
                    case KeyCode.Alpha2:
                    case KeyCode.Keypad2:
                        currentNumber = 2;
                        SetRowNotActive(1, 3);
                        break;
                    case KeyCode.Alpha3:
                    case KeyCode.Keypad3:
                        currentNumber = 3;
                        SetRowNotActive(1, 2);
                        break;
                    case KeyCode.Alpha4:
                    case KeyCode.Keypad4:
                        currentNumber = 4;
                        SetColumnNotActive(2, 3);
                        break;
                    case KeyCode.Alpha5:
                    case KeyCode.Keypad5:
                        currentNumber = 5;
                        SetColumnNotActive(1, 3);
                        break;
                    case KeyCode.Alpha6:
                    case KeyCode.Keypad6:
                        currentNumber = 6;
                        SetColumnNotActive(1, 2);
                        break;
                    case KeyCode.Alpha7:
                    case KeyCode.Keypad7:
                        currentNumber = 7;
                        SetLayerNotActive(2, 3);
                        break;
                    case KeyCode.Alpha8:
                    case KeyCode.Keypad8:
                        currentNumber = 8;
                        SetLayerNotActive(1, 3);
                        break;
                    case KeyCode.Alpha9:
                    case KeyCode.Keypad9:
                        currentNumber = 9;
                        SetLayerNotActive(1, 2);
                        break;
                }
                //讓箭頭消失
                for (int x = 1; x <= 3; x++) {
                    for (int z = 1; z <= 3; z++) {
                        childUp = upArrows.transform.Find($"UpArrow_({x}, -1, {z})");
                        childUp.gameObject.SetActive(false);

                        childDown = downArrows.transform.Find($"DownArrow_({x}, 5, {z})");
                        childDown.gameObject.SetActive(false);
                    }
                }
            }
        }
    }


    public bool IsMoveComplete() {
        foreach (KeyCode key in new KeyCode[] {
            KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.Q,

            KeyCode.UpArrow, KeyCode.DownArrow,

            KeyCode.Escape,
        }) {

            if (Input.GetKeyDown(key)) {
                switch (key) {

                    case KeyCode.W:
                    case KeyCode.A:
                    case KeyCode.S:
                        return true;
                    case KeyCode.Q:
                        surrenderButton.onClick.Invoke();
                        return false;
                    case KeyCode.UpArrow:
                    case KeyCode.DownArrow:
                    case KeyCode.Escape:
                        return true;
                    default:
                        return false;
                }
            }
        }

        // 判斷滑鼠點擊EmptyCube
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                GameObject clickedCube = hit.collider.gameObject;

                if (clickedCube.name.Contains("EmptyCube")) {
                    SetArrowActive();
                }
            }
        }

        // 判斷滑鼠點擊Arrow
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                GameObject clickedArrow = hit.collider.gameObject;

                if (clickedArrow.name.Contains("Arrow")) {
                    return InputOX(clickedArrow.name);
                }
            }
        }

        return false;
    }

    private GameObject[,,] OXboard = new GameObject[5, 5, 5];
    private GameObject tempCube;

    // 輸入O或X
    private bool InputOX(string clickedArrow) {
        if(currentNumber == 0) {
            return false;
        }
        int count = 0;
        for (int i = 1; i <= 3; i++) {
            for (int k = 1; k <= 3; k++) {
                if (clickedArrow.Contains($"({i}, -1, {k})")) {
                    for (int y = 1; y <= 3; y++) {
                        if (board[i, y, k] != 0) {
                            count++;
                        }
                    }
                    if (count != 3) {
                        if (board[i, 1, k] == 0) {
                            cubeBoard[i, 1, k].SetActive(false);
                            if (currentTurn == Player.O) {
                                tempCube = Instantiate(OCube, cubeBoard[i, 1, k].transform.position, Quaternion.identity);
                                tempCube.SetActive(true);
                                OXboard[i, 1, k] = tempCube;
                                board[i, 1, k] = 1;
                            } else {
                                tempCube = Instantiate(XCube, cubeBoard[i, 1, k].transform.position, Quaternion.identity);
                                tempCube.SetActive(true);
                                OXboard[i, 1, k] = tempCube;
                                board[i, 1, k] = -1;
                            }
                            return true;
                        } else if (board[i, 1, k] != 0 && board[i, 2, k] == 0) {
                            cubeBoard[i, 2, k].SetActive(false);
                            if (currentTurn == Player.O) {
                                OXboard[i, 2, k] = Instantiate(OXboard[i, 1, k], cubeBoard[i, 2, k].transform.position, Quaternion.identity);
                                if (OXboard[i, 1, k] != null) {
                                    Destroy(OXboard[i, 1, k]);  // 刪除舊物件，避免殘留
                                }

                                tempCube = Instantiate(OCube, cubeBoard[i, 1, k].transform.position, Quaternion.identity);
                                tempCube.SetActive(true);

                                OXboard[i, 1, k] = tempCube;
                                board[i, 2, k] = board[i, 1, k];
                                board[i, 1, k] = 1;
                            } else {
                                OXboard[i, 2, k] = Instantiate(OXboard[i, 1, k], cubeBoard[i, 2, k].transform.position, Quaternion.identity);
                                if (OXboard[i, 1, k] != null) {
                                    Destroy(OXboard[i, 1, k]);  // 刪除舊物件，避免殘留
                                }

                                tempCube = Instantiate(XCube, cubeBoard[i, 1, k].transform.position, Quaternion.identity);
                                tempCube.SetActive(true);

                                OXboard[i, 1, k] = tempCube;
                                board[i, 2, k] = board[i, 1, k];
                                board[i, 1, k] = -1;
                            }
                            return true;
                        } else if (board[i, 1, k] != 0 && board[i, 2, k] != 0 && board[i, 3, k] == 0) {
                            cubeBoard[i, 3, k].SetActive(false);
                            if (currentTurn == Player.O) {
                                OXboard[i, 3, k] = Instantiate(OXboard[i, 2, k], cubeBoard[i, 3, k].transform.position, Quaternion.identity);
                                if (OXboard[i, 2, k] != null) {
                                    Destroy(OXboard[i, 2, k]);  // 刪除舊物件，避免殘留
                                }
                                OXboard[i, 2, k] = Instantiate(OXboard[i, 1, k], cubeBoard[i, 2, k].transform.position, Quaternion.identity);
                                if (OXboard[i, 1, k] != null) {
                                    Destroy(OXboard[i, 1, k]);  // 刪除舊物件，避免殘留
                                }

                                tempCube = Instantiate(OCube, cubeBoard[i, 1, k].transform.position, Quaternion.identity);
                                tempCube.SetActive(true);

                                OXboard[i, 1, k] = tempCube;
                                board[i, 3, k] = board[i, 2, k];
                                board[i, 2, k] = board[i, 1, k];
                                board[i, 1, k] = -1;
                            } else {
                                OXboard[i, 3, k] = Instantiate(OXboard[i, 2, k], cubeBoard[i, 3, k].transform.position, Quaternion.identity);
                                if (OXboard[i, 2, k] != null) {
                                    Destroy(OXboard[i, 2, k]);  // 刪除舊物件，避免殘留
                                }
                                OXboard[i, 2, k] = Instantiate(OXboard[i, 1, k], cubeBoard[i, 2, k].transform.position, Quaternion.identity);
                                if (OXboard[i, 1, k] != null) {
                                    Destroy(OXboard[i, 1, k]);  // 刪除舊物件，避免殘留
                                }

                                tempCube = Instantiate(XCube, cubeBoard[i, 1, k].transform.position, Quaternion.identity);
                                tempCube.SetActive(true);

                                OXboard[i, 1, k] = tempCube;
                                board[i, 3, k] = board[i, 2, k];
                                board[i, 2, k] = board[i, 1, k];
                                board[i, 1, k] = -1;
                            }
                            return true;
                        }
                    }
                    count = 0;
                    return false;
                } else if (clickedArrow.Contains($"({i}, 5, {k})")) {
                    for (int y = 3; y >= 1; y--) {
                        if (board[i, y, k] != 0) {
                            count++;
                        }
                    }
                    if (count != 3) {
                        if (board[i, 3, k] == 0) {
                            if (board[i, 2, k] == 0) {
                                if (board[i, 1, k] == 0) {
                                    cubeBoard[i, 1, k].SetActive(false);
                                    if (currentTurn == Player.O) {
                                        tempCube = Instantiate(OCube, cubeBoard[i, 1, k].transform.position, Quaternion.identity);
                                        tempCube.SetActive(true);
                                        OXboard[i, 1, k] = tempCube;
                                        board[i, 1, k] = 1;
                                    } else {
                                        tempCube = Instantiate(XCube, cubeBoard[i, 1, k].transform.position, Quaternion.identity);
                                        tempCube.SetActive(true);
                                        OXboard[i, 1, k] = tempCube;
                                        board[i, 1, k] = -1;
                                    }
                                    return true;
                                }

                                cubeBoard[i, 2, k].SetActive(false);
                                if (currentTurn == Player.O) {
                                    tempCube = Instantiate(OCube, cubeBoard[i, 2, k].transform.position, Quaternion.identity);
                                    tempCube.SetActive(true);
                                    OXboard[i, 2, k] = tempCube;
                                    board[i, 2, k] = 1;
                                } else {
                                    tempCube = Instantiate(XCube, cubeBoard[i, 2, k].transform.position, Quaternion.identity);
                                    tempCube.SetActive(true);
                                    OXboard[i, 2, k] = tempCube;
                                    board[i, 2, k] = -1;
                                }
                                return true;
                            }
                            cubeBoard[i, 1, k].SetActive(false);
                            if (currentTurn == Player.O) {
                                tempCube = Instantiate(OCube, cubeBoard[i, 3, k].transform.position, Quaternion.identity);
                                tempCube.SetActive(true);

                                OXboard[i, 1, k] = tempCube;
                                board[i, 1, k] = 1;
                            } else {
                                tempCube = Instantiate(XCube, cubeBoard[i, 3, k].transform.position, Quaternion.identity);
                                tempCube.SetActive(true);
                                OXboard[i, 1, k] = tempCube;
                                board[i, 1, k] = -1;
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

    private Transform childUp;
    private Transform childDown;

    //設定箭頭根據輸入數字激活
    void SetArrowActive() {
        upArrows.SetActive(true);
        downArrows.SetActive(true);


        switch (currentNumber) {
            case 1:
                for (int z = 1; z <= 3; z++) {
                    childUp = upArrows.transform.Find($"UpArrow_(1, -1, {z})");
                    childUp.gameObject.SetActive(true);

                    childDown = downArrows.transform.Find($"DownArrow_(1, 5, {z})");
                    childDown.gameObject.SetActive(true);
                }
                break;
            case 2:
                for (int z = 1; z <= 3; z++) {
                    childUp = upArrows.transform.Find($"UpArrow_(2, -1, {z})");
                    childUp.gameObject.SetActive(true);
                    childDown = downArrows.transform.Find($"DownArrow_(2, 5, {z})");
                    childDown.gameObject.SetActive(true);
                }
                break;
            case 3:
                for (int z = 1; z <= 3; z++) {
                    childUp = upArrows.transform.Find($"UpArrow_(3, -1, {z})");
                    childUp.gameObject.SetActive(true);
                    childDown = downArrows.transform.Find($"DownArrow_(3, 5, {z})");
                    childDown.gameObject.SetActive(true);
                }
                break;
            case 4:
                for (int x = 1; x <= 3; x++) {
                    childUp = upArrows.transform.Find($"UpArrow_({x}, -1, 1)");
                    childUp.gameObject.SetActive(true);
                    childDown = downArrows.transform.Find($"DownArrow_({x}, 5, 1)");
                    childDown.gameObject.SetActive(true);
                }
                break;
            case 5:
                for (int x = 1; x <= 3; x++) {
                    childUp = upArrows.transform.Find($"UpArrow_({x}, -1, 2)");
                    childUp.gameObject.SetActive(true);
                    childDown = downArrows.transform.Find($"DownArrow_({x}, 5, 2)");
                    childDown.gameObject.SetActive(true);
                }
                break;
            case 6:
                for (int x = 1; x <= 3; x++) {
                    childUp = upArrows.transform.Find($"UpArrow_({x}, -1, 3)");
                    childUp.gameObject.SetActive(true);
                    childDown = downArrows.transform.Find($"DownArrow_({x}, 5, 3)");
                    childDown.gameObject.SetActive(true);
                }
                break;
            case 7:
            case 8:
            case 9:
            case 0:
                for (int x = 1; x <= 3; x++) {
                    for (int z = 1; z <= 3; z++) {
                        childUp = upArrows.transform.Find($"UpArrow_({x}, -1, {z})");
                        childUp.gameObject.SetActive(true);

                        childDown = downArrows.transform.Find($"DownArrow_({x}, 5, {z})");
                        childDown.gameObject.SetActive(true);
                    }
                }
                break;
        }

    }

    //設定只激活一行
    void SetRowNotActive(int x1, int x2) {
        int x = 6 - x1 - x2;
        for (int y = 1; y <= 3; y++) {
            for (int z = 1; z <= 3; z++) {
                if (cubeBoard[x1, y, z].activeSelf) {
                    cubeBoard[x1, y, z].SetActive(false);
                }
                if (cubeBoard[x2, y, z].activeSelf) {
                    cubeBoard[x2, y, z].SetActive(false);
                }

            }
        }


    }

    //設定只激活一列
    void SetColumnNotActive(int z1, int z2) {
        int z = 6 - z1 - z2;
        for (int x = 1; x <= 3; x++) {
            for (int y = 1; y <= 3; y++) {
                if (cubeBoard[x, y, z1].activeSelf) {
                    cubeBoard[x, y, z1].SetActive(false);
                }
                if (cubeBoard[x, y, z2].activeSelf) {
                    cubeBoard[x, y, z2].SetActive(false);
                }
            }
        }
    }

    //設定只激活一層
    void SetLayerNotActive(int y1, int y2) {
        for (int x = 1; x <= 3; x++) {
            for (int z = 1; z <= 3; z++) {
                if (cubeBoard[x, y1, z].activeSelf) {
                    cubeBoard[x, y1, z].SetActive(false);
                }
                if (cubeBoard[x, y2, z].activeSelf) {
                    cubeBoard[x, y2, z].SetActive(false);
                }
            }
        }
    }

    //激活3*3*3所有方塊
    void AllCubeActive() {
        for (int x = 1; x <= 3; x++) {
            for (int y = 1; y <= 3; y++) {
                for (int z = 1; z <= 3; z++) {
                    cubeBoard[x, y, z].SetActive(true);
                }
            }
        }
    }
    public GameObject myPrefab;
    public GameObject upArrows, downArrows;
    private int[,,] board = new int[5, 5, 5];
    private GameObject[,,] cubeBoard = new GameObject[5, 5, 5]; // 座標[7*(i-2), 7*(j-2), 7*(k-2)]
    private GameObject EmptyCube;

    public void ResetScene() {
        endScene.SetActive(false);
        OCanvas.SetActive(true);
        XCanvas.SetActive(false);
        foreach (var arrow in upArrows.GetComponentsInChildren<Transform>()) {
            arrow.gameObject.SetActive(false);
        }
        foreach (var arrow in downArrows.GetComponentsInChildren<Transform>()) {
            arrow.gameObject.SetActive(false);
        }

        currentTurn = Player.O;
        winner = Player.Neither;

        for (int x = 0; x < 5; x++) { // 清除舊方塊
            for (int y = 0; y < 5; y++) {
                for (int z = 0; z < 5; z++) {
                    Destroy(cubeBoard[x, y, z]);
                }
            }
        }

        board = new int[5, 5, 5];
        for (int x = 0; x < 5; x++) {
            for (int y = 0; y < 5; y++) {
                for (int z = 0; z < 5; z++) {
                    Vector3 position = new Vector3(7 * (x - 2), 7 * (y - 2), 7 * (z - 2));
                    EmptyCube = Instantiate(myPrefab, position, Quaternion.identity);

                    EmptyCube.SetActive(true); // 先全部建出來
                    cubeBoard[x, y, z] = EmptyCube; // 存入 cubeBoard 陣列
                    EmptyCube.name = $"EmptyCube(Clone)_({x}, {y}, {z})";

                    //(1~3, 1~3, 1~3)的 cube 不失活
                    if (x == 0 || x == 4 || y == 0 || y == 4 || z == 0 || z == 4) {
                        EmptyCube.SetActive(false);
                    }
                }
            }
        }
    }

    public bool SomeoneHasWin() {
        int vs = 0;
        // 檢查每層、列、行以及對角線
        for (int i = 0; i < 5; i++) {
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
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 3; j++) {
                // 檢查行
                if (Mathf.Abs(board[layer, i, j] + board[layer, i, j + 1] + board[layer, i, j + 2]) == 3) {
                    vs += board[layer, i, j];
                }

                // 檢查列
                if (Mathf.Abs(board[layer, j, i] + board[layer, j + 1, i] + board[layer, j + 2, i]) == 3) {
                    vs += board[layer, j, i];
                }
            }
        }

        // 檢查對角線
        for (int i = 0; i < 3; i++) {
            if (Mathf.Abs(board[layer, i, i] + board[layer, i + 1, i + 1] + board[layer, i + 2, i + 2]) == 3) {
                vs += board[layer, i, i];
            }

            if (Mathf.Abs(board[layer, i, 4 - i] + board[layer, i + 1, 3 - i] + board[layer, i + 2, 2 - i]) == 3) {
                vs += board[layer, i, 4 - i];
            }
        }
        return vs;
    }

    // 檢查縱向的列是否有玩家獲勝
    private int CheckColumn(int col) {
        int vs = 0;
        // 檢查每一列的行和列
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 3; j++) {
                // 檢查行
                if (Mathf.Abs(board[i, j, col] + board[i, j + 1, col] + board[i, j + 2, col]) == 3) {
                    vs += board[i, j, col];
                }

                // 檢查列
                if (Mathf.Abs(board[j, i, col] + board[j + 1, i, col] + board[j + 2, i, col]) == 3) {
                    vs += board[j, i, col];
                }
            }
        }

        // 檢查對角線
        for (int i = 0; i < 3; i++) {
            if (Mathf.Abs(board[i, i, col] + board[i + 1, i + 1, col] + board[i + 2, i + 2, col]) == 3) {
                vs += board[i, i, col];
            }

            if (Mathf.Abs(board[i, 4 - i, col] + board[i + 1, 3 - i, col] + board[i + 2, 2 - i, col]) == 3) {
                vs += board[i, 4 - i, col];
            }
        }
        return vs;
    }

    // 檢查橫向的行是否有玩家獲勝
    private int CheckRow(int row) {
        int vs = 0;
        // 檢查每一行的行和列
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 3; j++) {
                // 檢查行
                if (Mathf.Abs(board[i, row, j] + board[i, row, j + 1] + board[i, row, j + 2]) == 3) {
                    vs += board[i, row, j];
                }

                // 檢查列
                if (Mathf.Abs(board[j, row, i] + board[j + 1, row, i] + board[j + 2, row, i]) == 3) {
                    vs += board[j, row, i];
                }
            }
        }

        // 檢查對角線
        for (int i = 0; i < 3; i++) {
            if (Mathf.Abs(board[i, row, i] + board[i + 1, row, i + 1] + board[i + 2, row, i + 2]) == 3) {
                vs += board[i, row, i];
            }

            if (Mathf.Abs(board[i, row, 4 - i] + board[i + 1, row, 3 - i] + board[i + 2, row, 2 - i]) == 3) {
                vs += board[i, row, 4 - i];
            }
        }
        return vs;
    }

    // 檢查 3D 對角線是否有玩家獲勝
    private int Check3DDiagonals() {
        int vs = 0;
        for (int i = 0; i < 3; i++) {
            if (Mathf.Abs(board[i, i, i] + board[i + 1, i + 1, i + 1] + board[i + 2, i + 2, i + 2]) == 3) {
                vs += board[i, i, i];
            }
            if (Mathf.Abs(board[i, i, 4 - i] + board[i + 1, i + 1, 3 - i] + board[i + 2, i + 2, 2 - i]) == 3) {
                vs += board[i, i, 4 - i];
            }
            if (Mathf.Abs(board[i, 4 - i, i] + board[i + 1, 3 - i, i + 1] + board[i + 2, 2 - i, i + 2]) == 3) {
                vs += board[i, 4 - i, i];
            }
            if (Mathf.Abs(board[4 - i, i, i] + board[3 - i, i + 1, i + 1] + board[2 - i, i + 2, i + 2]) == 3) {
                vs += board[4 - i, i, i];
            }
        }
        return vs;
    }
}