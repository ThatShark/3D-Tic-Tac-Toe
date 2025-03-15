using System;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour {
    public enum Player {
        O,
        X,
        Neither
    }
    public Player currentTurn, winner;

    void Start() {
        currentTurn = Player.O;
        ResetScene();
    }

    public GameObject OCanvas, XCanvas, endScene, OWinText, XWinText;
    void Update() {
        switch (currentTurn) {
            case Player.Neither:
                OCanvas.SetActive(false);
                XCanvas.SetActive(false);
                endScene.SetActive(true);
                ((winner == Player.O) ? XWinText : OWinText).SetActive(false);
                break;
            default:
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

    

    public bool IsMoveComplete() {
        foreach (KeyCode key in new KeyCode[] {
            KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
            KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9,
            
            KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5,
            KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9, KeyCode.Keypad0,
            
            KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.Q, 
            
            KeyCode.UpArrow, KeyCode.DownArrow, 
            
            KeyCode.Escape
        })  {
            
            if (Input.GetKeyDown(key)) {
                switch (key) {
                    case KeyCode.Alpha1:
                    case KeyCode.Alpha2:
                    case KeyCode.Alpha3:
                    case KeyCode.Alpha4:
                    case KeyCode.Alpha5:
                    case KeyCode.Alpha6:
                    case KeyCode.Alpha7:
                    case KeyCode.Alpha8:
                    case KeyCode.Alpha9:
                    case KeyCode.Alpha0:
                    case KeyCode.Keypad1:
                    case KeyCode.Keypad2:
                    case KeyCode.Keypad3:
                    case KeyCode.Keypad4:
                    case KeyCode.Keypad5:
                    case KeyCode.Keypad6:
                    case KeyCode.Keypad7:
                    case KeyCode.Keypad8:
                    case KeyCode.Keypad9:
                    case KeyCode.Keypad0:
                    case KeyCode.W:
                    case KeyCode.A:
                    case KeyCode.S:
                    case KeyCode.Q:
                    case KeyCode.UpArrow:
                    case KeyCode.DownArrow:
                    case KeyCode.Escape:
                        return true;
                    default:
                        return false;
                }
            }
        }
            return false;
    }
    
    public GameObject myPrefab;
    private int[,,] board = new int[5, 5, 5];
    private GameObject[,,] cubeBoard = new GameObject[5, 5, 5]; // 座標[7*(i-2), 7*(j-2), 7*(k-2)]
    public void ResetScene() {
        endScene.SetActive(false);
        OCanvas.SetActive(true);
        XCanvas.SetActive(false);
        currentTurn = Player.O;
        winner = Player.Neither;

        board = new int[5, 5, 5]; 
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 5; j++) {
                for (int k = 0; k < 5; k++) {
                    cubeBoard[i,j,k] = myPrefab;
                }
            }
        }
        for (int x = -7; x <= 7; x += 7) {
            for (int y = -7; y <= 7; y += 7) {
                for (int z = -7; z <= 7; z += 7) {
                    Instantiate(myPrefab, new Vector3(x, y, z), Quaternion.identity);
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