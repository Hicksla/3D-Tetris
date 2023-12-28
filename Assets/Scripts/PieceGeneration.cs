using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class PieceGeneration : MonoBehaviour
{
    GameObject[,,] board = new GameObject[10, 5, 5];
    bool[,,] boardBool = new bool[10, 5, 5];
    private float[] input = new float[7];

    enum buttonState
    {
        nothing,
        pressed,
        held,
        released
    }

    private buttonState[] inputStates = new buttonState[5];
    private bool keyHeld;

    public GameObject cube;
    public List<GameObject> block;
    private float width;
    private float height;
    private float length;
    private float fallingTimeDelta;
    private float fallingRate;
    private bool fastFalling;
    private bool isFalling;
    private bool dropped;
    private int layer;

    private bool isPaused;

    void Start()
    {
        fallingTimeDelta = 0.0f;
        fallingRate = 1.5f;
        fastFalling = false;
        isFalling = true;
        dropped = false;
        layer = 10;

        keyHeld = false;
        
        isPaused = false;

        createPiece();
    }

    public void rotateY(bool direction)
    {
        float tempValue;
        for(int i=0; i<4; i++){
            tempValue = block[i].transform.position.z;
            block[i].transform.position = new Vector3(block[i].transform.position.x, block[i].transform.position.y, block[i].transform.position.x);
            block[i].transform.position = new Vector3(tempValue, block[i].transform.position.y, -1*block[i].transform.position.z);
        }
    }

    public void updateInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }

        float minX = 3;
        float maxX = -3;
        float minZ = 3;
        float maxZ = -3;
        bool ableToMove = true;
        input[0] = Input.GetAxis("xAxis");
        input[1] = Input.GetAxis("yAxis");
        input[2] = Input.GetAxis("zAxis");
        input[3] = Input.GetAxis("leftRight");
        input[4] = Input.GetAxis("upDown");
        input[5] = Input.GetAxis("Drop");
        input[6] = Input.GetAxis("Pause");

        if(!isPaused){
            if(input[1]>0){
                if (!keyHeld){
                    rotateY(true);
                    keyHeld=true;
                }
            }
            else if(input[1]<0){
                if (!keyHeld){
                    rotateY(false);
                    keyHeld=true;
                }
            }

            else if(input[3] != 0){
                if (!keyHeld){
                    for(int i=0; i<4; i++){
                        if(input[3]>0 && block[i].transform.position.x==-2) ableToMove = false;
                        if(input[3]<0 && block[i].transform.position.x==2) ableToMove = false;
                    }

                    if(ableToMove){
                        for(int i=0; i<4; i++){
                            block[i].transform.position = block[i].transform.position - new Vector3(input[3], 0, 0);
                        }
                    }
                    keyHeld = true;
                }
            }
            else if(input[4] != 0){
                if (!keyHeld){
                    for(int i=0; i<4; i++){
                        if(input[4]<0 && block[i].transform.position.z==-2) ableToMove = false;
                        if(input[4]>0 && block[i].transform.position.z==2) ableToMove = false;
                    }

                    if(ableToMove){
                        for(int i=0; i<4; i++){
                            block[i].transform.position = block[i].transform.position + new Vector3(0, 0, input[4]);
                        }
                    }
                    keyHeld = true;
                }
            }
            else if(input[5] > 0){
                if (!keyHeld){
                    bool stopped = false;
                    while(!stopped){
                        for(int i=0; i<4; i++){
                            Debug.Log("X: " + layer + "Y: " + layer + "Z: " + (int)block[block.Count-1].transform.position.z);
                            if(boardBool[layer-1, (int)block[block.Count-1].transform.position.x+2, (int)block[block.Count-1].transform.position.z+2]){
                                stopped = true;
                            } 
                            else if(layer>0){
                                for(int j=0; j<4; j++){
                                    block[j].transform.position -= new Vector3(0, 1, 0);
                                }
                                layer -= 1;
                            }
                            else{
                                stopped = true;
                            }
//                            block[i].transform.position -= new Vector3(0, layer, 0);
                        }
                    }
                    if(stopped){
                        Debug.Log("Droppppppped");
                        dropped = true;
                        while(block.Count>0){
                            board[(int)block[block.Count-1].transform.position.y, (int)block[block.Count-1].transform.position.x+2, (int)block[block.Count-1].transform.position.z+2] = Instantiate(cube, block[block.Count - 1].transform.position, Quaternion.identity);
                            boardBool[(int)block[block.Count-1].transform.position.y, (int)block[block.Count-1].transform.position.x+2, (int)block[block.Count-1].transform.position.z+2] = true;
                            block.RemoveAt(block.Count-1);
                        }
                        createPiece();
                    }

                    layer = 0;
                    keyHeld = true;
                }
            }
            else if(input[5]<0){
                fastFalling = true;
                fallingRate *= 0.5f;
            }
            else{
                if(fastFalling){
                    fastFalling = false;
                }
                keyHeld = false;
            }
            if(input[6]!=0){
                isPaused = true;
                keyHeld = true;
            }
        }
        else if(input[6]==0){
            isPaused = false;
            keyHeld = false;
        }
    }

    void createPiece()
    {
        int rInt = UnityEngine.Random.Range(0, 7);

        for(int i=0; i<4; i++){
            block.Add(cube);
            block[i] = Instantiate(cube, cube.transform.position, Quaternion.identity);
        }
        switch(rInt){
            case 0:         //I Piece
                block[0].transform.position = new Vector3(-1, 10, 0);
                block[1].transform.position = new Vector3(0, 10, 0);
                block[2].transform.position = new Vector3(1, 10, 0);
                block[3].transform.position = new Vector3(2, 10, 0);
                break;
            case 1:         //L Piece
                block[0].transform.position = new Vector3(-1, 10, 0);
                block[1].transform.position = new Vector3(0, 10, 0);
                block[2].transform.position = new Vector3(1, 10, 0);
                block[3].transform.position = new Vector3(1, 10, 1);
                break;
            case 2:         //Square Piece
                block[0].transform.position = new Vector3(1, 10, 0);
                block[1].transform.position = new Vector3(0, 10, 0);
                block[2].transform.position = new Vector3(0, 10, 1);
                block[3].transform.position = new Vector3(1, 10, 1);
                break;
            case 3:         //T Piece
                block[0].transform.position = new Vector3(-1, 10, 0);
                block[1].transform.position = new Vector3(0, 10, 0);
                block[2].transform.position = new Vector3(1, 10, 0);
                block[3].transform.position = new Vector3(0, 10, 1);
                break;
            case 4:         //S Piece
                block[0].transform.position = new Vector3(-1, 10, 0);
                block[1].transform.position = new Vector3(0, 10, 0);
                block[2].transform.position = new Vector3(0, 10, 1);
                block[3].transform.position = new Vector3(1, 10, 1);
                break;
            case 5:         //Curve Piece
                block[0].transform.position = new Vector3(-1, 10, 0);
                block[1].transform.position = new Vector3(0, 10, 0);
                block[2].transform.position = new Vector3(0, 10, 1);
                block[3].transform.position = new Vector3(0, 11, 1);
                break;
            case 6:         //Corner Piece
                block[0].transform.position = new Vector3(-1, 10, 0);
                block[1].transform.position = new Vector3(0, 10, 0);
                block[2].transform.position = new Vector3(0, 10, 1);
                block[3].transform.position = new Vector3(0, 11, 0);
                break;
            default:        //Corner Piece
                block[0].transform.position = new Vector3(-1, 10, 0);
                block[1].transform.position = new Vector3(0, 10, 0);
                block[2].transform.position = new Vector3(0, 10, 1);
                block[3].transform.position = new Vector3(0, 11, 0);
                break;
        }
    }

    void addToBoard()
    {
        Vector3 tempVector = new Vector3();
        bool layerFull = true;
        while(block.Count>0){
            tempVector = block[block.Count - 1].transform.position;
            block.RemoveAt(block.Count - 1);
            board[(int)tempVector.y, (int)tempVector.x+2, (int)tempVector.z+2] = Instantiate(cube, tempVector, Quaternion.identity);
            boardBool[(int)tempVector.y, (int)tempVector.x +2, (int)tempVector.z+2] = true;
        }

        for(int i=0; i<4; i++){
            for(int j=0; j<5; j++){
                for(int k=0; k<5; k++){
                    if(!boardBool[layer, j, k]) layerFull = false;
                }
            }
            if(layerFull){
                for(int l=0; l<9-layer; l++){
                    for(int j=0; j<5; j++){
                        for(int k=0; k<5; k++){
                            board[l, j, k] = board[l+1, j, k];
                        }
                    }
                }
            }
        }
        createPiece();
    }

    void stoppedFallingStatus()
    {
        float minY = 0.0f;
        bool[] bottomPiece = new bool[4];
        for(int i=0; i<4; i++){
            if(layer>1 && block[i].transform.position.y==layer){
                if(boardBool[layer - 1, (int)block[i].transform.position.x+2, (int)block[i].transform.position.z+2]){
                    isFalling = false;
                }
            }
            if(minY==0.0f) minY = block[i].transform.position.y;
            else if(block[i].transform.position.y==minY) bottomPiece[i] = true;
            else if(block[i].transform.position.y<minY){ minY = block[i].transform.position.y; bottomPiece[i] = true; }
        }
        
        for(int i=0; i<4; i++){
                Debug.Log("X: " + block[i].transform.position.x + " Y: " + block[i].transform.position.y + " Z: " + (int)block[i].transform.position.z);
                if(boardBool[layer-1, (int)block[i].transform.position.x+2, (int)block[i].transform.position.z+2]){
                    isFalling = false;
                }
        }
    }

    void Update()
    {
        updateInput();
        if(!isPaused){
//            stoppedFallingStatus();
            if(fallingTimeDelta>=fallingRate && layer!=0 && isFalling && !dropped){
                for(int i=0; i<4; i++){
                    block[i].transform.position = block[i].transform.position - new Vector3(0, 1, 0);
                }
                fallingTimeDelta = 0;
                layer--;
            }
            else if(layer==0){
                addToBoard();
                fallingTimeDelta = 0.0f;
                fallingRate = 1.5f;
                isFalling = true;
                layer = 10;
            }
            else if(fallingTimeDelta<fallingRate){
                fallingTimeDelta += Time.deltaTime;
            }
            else if(dropped){
                Debug.Log("Dropped");
                createPiece();
                fallingTimeDelta = 0.0f;
                fallingRate = 1.5f;
                dropped = false;
                layer = 10;
            }
            if (!isFalling){
                addToBoard();
                fallingTimeDelta = 0.0f;
                fallingRate = 1.5f;
                isFalling = true;
                layer = 10;
            }
        }
        else{
            if(Input.GetAxis("Pause")!=0){
                if(!keyHeld){
                    Debug.Log("Exiting Pause");
                    isPaused = false;
                }
            }
            else{
                keyHeld = false;
            }
        }
    }
}