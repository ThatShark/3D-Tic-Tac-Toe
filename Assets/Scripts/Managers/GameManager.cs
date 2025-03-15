using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public enum Player {
        O,
        X,
        Neither
    }
    private Player currentTurn, winner = Player.Neither;

    void Start() {
        currentTurn = Player.O;
        ResetScene();
    }

    void Update() {
        switch (currentTurn) {
            case Player.Neither:
                AgainOrQuit();
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
            currentTurn = (currentTurn == Player.O) ? Player.X : Player.O;
        } else {
            int nowTurn = (currentTurn == Player.O) ? 1 : -1;
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

    public bool IsMoveComplete() {
        return false;   
    }

    public GameObject myPrefab;
    private int[,,] board = new int[5, 5, 5];
    private GameObject[,,] cubeBoard = new GameObject[5, 5, 5]; // 座標[7*(i-2), 7*(j-2), 7*(k-2)]
    public void ResetScene() {
        winner = Player.Neither;
        board = new int[5, 5, 5]; 
        for (int x = 0; x <= 4; x++) {
            for (int y = 0; y <= 4; y++) {
                for (int z = 0; z <= 4; z++) {
                    cubeBoard[x, y, z] = myPrefab;
                }
            }
        }
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                for (int z = -1; z <= 1; z++) {
                    Instantiate(myPrefab, new Vector3(7*x, 7*y, 7*z), Quaternion.identity);
                }
            }
        }
    }

    public void AgainOrQuit() {

    }


    // 檢查某一層是否有玩家獲勝
    private int CheckLayer(int layer) {
        int vs = 0;
        // 檢查每一層的行和列
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 3; j++) {
                // 檢查行
                if (Mathf.Abs(board[layer, i, j] + board[layer, i, j+1] + board[layer, i, j+2]) == 3) {
                    vs += board[layer, i, j];
                }
                
                // 檢查列
                if (Mathf.Abs(board[layer, j, i] + board[layer, j+1, i] + board[layer, j+2, i]) == 3) {
                    vs += board[layer, j, i];
                }
            }
        }

        // 檢查對角線
        for (int i = 0; i < 3; i++) {
            if (Mathf.Abs(board[layer, i, i] + board[layer, i+1, i+1] + board[layer, i+2, i+2]) == 3) {
                vs += board[layer, i, i];
            }
            
            if (Mathf.Abs(board[layer, i, 4-i] + board[layer, i+1, 3-i] + board[layer, i+2, 2-i]) == 3) {
                vs += board[layer, i, 4-i];
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
                if (Mathf.Abs(board[i, j, col] + board[i, j+1, col] + board[i, j+2, col]) == 3) {
                    vs += board[i, j, col];
                }
                
                // 檢查列
                if (Mathf.Abs(board[j, i, col] + board[j+1, i, col] + board[j+2, i, col]) == 3) {
                    vs += board[j, i, col];
                }
            }
        }

        // 檢查對角線
        for (int i = 0; i < 3; i++) {
            if (Mathf.Abs(board[i, i, col] + board[i+1, i+1, col] + board[i+2, i+2, col]) == 3) {
                vs += board[i, i, col];
            }
            
            if (Mathf.Abs(board[i, 4-i, col] + board[i+1, 3-i, col] + board[i+2, 2-i, col]) == 3) {
                vs += board[i, 4-i, col];
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
                if (Mathf.Abs(board[i, row, j] + board[i, row, j+1] + board[i, row, j+2]) == 3) {
                    vs += board[i, row, j];
                }
                
                // 檢查列
                if (Mathf.Abs(board[j, row, i] + board[j+1, row, i] + board[j+2, row, i]) == 3) {
                    vs += board[j, row, i];
                }
            }
        }

        // 檢查對角線
        for (int i = 0; i < 3; i++) {
            if (Mathf.Abs(board[i, row, i] + board[i+1, row, i+1] + board[i+2, row, i+2]) == 3) {
                vs += board[i, row, i];
            }
            
            if (Mathf.Abs(board[i, row, 4-i] + board[i+1, row, 3-i] + board[i+2, row, 2-i]) == 3) {
                vs += board[i, row, 4-i];
            }
        }
        return vs;
    }

    // 檢查 3D 對角線是否有玩家獲勝
    private int Check3DDiagonals() {
        int vs = 0;
        for (int i = 0; i < 3; i++) {
            if (Mathf.Abs(board[i, i, i] + board[i+1, i+1, i+1] + board[i+2, i+2, i+2]) == 3) {
                vs += board[i, i, i];
            }
            if (Mathf.Abs(board[i, i, 4-i] + board[i+1, i+1, 3-i] + board[i+2, i+2, 2-i]) == 3) {
                vs += board[i, i, 4-i];
            }
            if (Mathf.Abs(board[i, 4-i, i] + board[i+1, 3-i, i+1] + board[i+2, 2-i, i+2]) == 3) {
                vs += board[i, 4-i, i];
            }
            if (Mathf.Abs(board[4-i, i, i] + board[3-i, i+1, i+1] + board[2-i, i+2, i+2]) == 3) {
                vs += board[4-i, i, i];
            }
        }
        return vs;
    }
}