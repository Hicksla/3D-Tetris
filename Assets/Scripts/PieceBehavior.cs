using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceBehavior : MonoBehaviour
{
    public struct blockPos
    {
        public void Coords(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X {
            get { return X; }
            set { X = value; }
        }
        public int Y
        {
            get { return Y; }
            set { Y = value; }
        }
        public int Z
        {
            get { return Z; }
            set { Z = value; }
        }

        public override string ToString() => $"({X}, {Y}, {Z})";
    }

    enum buttonState
    {
        nothing,
        pressed,
        held,
        released
    }

    private buttonState[] inputStates = new buttonState[6];
    
    private blockPos currBlockPos = new blockPos();
    private bool alreadyRotated;
    int yDist = -1;
    private bool isFalling;
    Transform[] blockPieceTransforms;

    void Start()
    {
        for(int i=0; i<6; i++)
        {
            inputStates[i] = buttonState.nothing;
        }
        isFalling = false;
        alreadyRotated = false;
    }

    public void setFalling(bool value)
    {
        isFalling = value;
    }

    public float[] getInput()
    {
        int correctInput = 0;
        float[] input = new float[5];
        input[0] = Input.GetAxis("xAxis");
        input[1] = Input.GetAxis("yAxis");
        input[2] = Input.GetAxis("zAxis");
        input[3] = Input.GetAxis("leftRight");
        input[4] = Input.GetAxis("upDown");
        return input;
    }

    // Update is called once per frame
    void Update()
    {
        float tempInput;
        float[] input = getInput();

        if(input[0]!=0 || input[1]!=0 || input[2] != 0)
        {
            if(!alreadyRotated){
                alreadyRotated = true;
                transform.Rotate(new Vector3(90*input[0], 90*input[1], 90*input[2]));



                /*
                blockPieceTransforms = this.GetComponentsInChildren(Transform);
                for (var i = 0, len = blockPieceTransforms.Length; i < len; i++) {
                    var child = blockPieceTransforms[i];
                    if(child.position.y<0){
                        transform.translate(new vector3(0, child.position.y, 0));
                    }
                }
                */
             // $$anonymous$$ethod 3
             //v3Pos = new Vector3(0.0f,-.5f, 0.0f);
             //v3Pos = transform.rotation * v3Pos + transform.position;
             //Debug.Log ("-->"+v3Pos);
            }
        }
        else{
            alreadyRotated = false;
        }
        if (input[3] != 0)
        {
            Debug.Log("Horizontal: " + input[3]);
        }
    }
}
