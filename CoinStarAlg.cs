using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HW6
{
    class CoinStarAlg
    {
        enum Side {
           Black, White
        }
        public Point Size{ get; set; }
        /*const Side[] b_goal =  new Side[] { 
            Side.Black, Side.Black, Side.Black, Side.Black, 
            Side.Black, Side.Black, Side.Black, Side.Black, 
            Side.Black, Side.Black, Side.Black, Side.Black,
            Side.Black, Side.Black, Side.Black, Side.Black 
        };
        const Side[] w_goal = new Side[] {
            Side.White, Side.White, Side.White, Side.White,
            Side.White, Side.White, Side.White, Side.White,
            Side.White, Side.White, Side.White, Side.White,
            Side.White, Side.White, Side.White, Side.White
        };*/
        const int MAX_STATES = 65535;

        int startPoint;
        int[] tested;
        int answer;

        public CoinStarAlg(Point size, Coin[,] coins)
        {
            Size = size;
            tested = new int[MAX_STATES];
            startPoint = calculate(coins);
            for (int i = 0; i < MAX_STATES; i++)
                tested[i] = -1;
        }

        public int calculate(Coin[,] coins)
        {
            bool [] temp = new bool[16];
            int count = 0;
            for(int y = 0; y < Grid.gridSize; y++ ) {
                for(int x = 0; x < Grid.gridSize; x++) {
                    temp[count] = coins[y, x].CoinColor == Color.Black ? false : true;
                    count++;
                }
            }
            System.Console.WriteLine(temp);
            return binaryToInt(temp);
        }

        public bool solve() {
            Side[] tester = new Side[16];
            int currentTestInt = 0;
            int child = 0;
            bool[] currTestArr;

            Queue<int> toBeTested = new Queue<int>();
            toBeTested.Enqueue(startPoint);

            while( toBeTested.Count > 0 ) {
                currentTestInt = toBeTested.Dequeue();
                currTestArr = intToArr(currentTestInt);
                setState(tester, currTestArr);
                for(int i = 0; i < 16; i++) {
                    ChangeState(i, tester);
                    child = arrToInt(tester);
                    if(child == 0 || child == 65535) {
                        tested[child] = currentTestInt;
                        answer = child;
                        return true;
                    }
                    else {
                        if(tested[child] == -1) {
                            tested[child] = currentTestInt;
                            toBeTested.Enqueue(child);
                        }
                    }
                    ChangeState(i, tester);
                }
            }
            return false;
        }

        public void Answer() {
            int temp = 0;
            Queue<int> answers = new Queue<int>();
            while(temp != startPoint) {
                answers.Enqueue(temp);
                temp = tested[temp];
            }
        }

        private bool[] intToArr(int num) {
            string temp = Convert.ToString(num, 2);
            char[] binary = temp.ToCharArray();
            bool[] ret = new bool[16];
            for(int i = 0; i < 16; i++) {
                ret[i] = false;
            }
            for(int i = binary.Length-1; i > 0; i--) {
                if (binary[i] == '1')
                    ret[i] = true;
            }
            return ret;
        }

        private int arrToInt(Side[] side)
        {
            int num = 0;
            for(int i = 0; i < 16; i++) {
                if (side[i] == Side.White)
                    num += (int)Math.Pow(2, i);
            }
            return num;
        }

        private void setState(Side[] currState, bool[] coins)
        {
            for(int i = 0; i < 15; i++) {
                if (coins[i])
                    currState[i] = Side.White;
                else
                    currState[i] = Side.Black;
            }
        }

        private int binaryToInt(bool[] bin)
        {
            int num = 0;
            for(int i = 0; i < 16; i++) {
                if (bin[i])
                    num += (int)Math.Pow(2, i);
            }
            return num;
        }

        private void ChangeState(int x, Side[] currState)
        {
            if( x < 4 ) {
                /*if(1 < x && x < 4) {
                    currState[x - 1] = flipSide(currState[x - 1]);
                    currState[x + 1] = flipSide(currState[x + 1]);
                }*/
                currState[x + 4] = flipSide(currState[x + 4]);
                switch(x) {
                    case 0:
                        currState[x + 1] = flipSide(currState[x + 1]);
                        break;
                    case 3:
                        currState[x - 1] = flipSide(currState[x + 1]);
                        break;
                    default:
                        currState[x - 1] = flipSide(currState[x - 1]);
                        currState[x + 1] = flipSide(currState[x + 1]);
                        break;
                }
            }
            else if( x < 12 && x > 3 ) {
                currState[x - 4] = flipSide(currState[x - 4]);
                currState[x + 4] = flipSide(currState[x + 4]);
                if (x == 4 || x == 8)
                    currState[x + 1] = flipSide(currState[x + 1]);
                else if (x == 7 || x == 11)
                    currState[x - 1] = flipSide(currState[x - 1]);
                else {
                    currState[x + 1] = flipSide(currState[x + 1]);
                    currState[x - 1] = flipSide(currState[x - 1]);
                }
            }
            else if( x < 16 && x > 11 ) {
                currState[x - 4] = flipSide(currState[x - 4]);
                if (x == 12)
                    currState[x + 1] = flipSide(currState[x + 1]);
                else if (x == 15)
                    currState[x - 1] = flipSide(currState[x - 1]);
                else
                {
                    currState[x + 1] = flipSide(currState[x + 1]);
                    currState[x - 1] = flipSide(currState[x - 1]);
                }
                   
            }
        }

        private  Side flipSide(Side side) {
            return side = (side == Side.Black) ? Side.White : Side.Black;
        }
    }
}
