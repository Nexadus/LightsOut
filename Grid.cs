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
    class Grid
    {
        public const int gridSize = 4;
        public Point Size { get; private set; }
        public Coin[,] coins;
        public CoinStarAlg A;
        private ContentManager content;
        

        public Grid(ContentManager Content) {
            Size = new Point(gridSize, gridSize);
            content = Content;
            coins = new Coin[Size.X, Size.Y];
            for (int i = 0; i < Size.X; i++)
                for (int j = 0; j < Size.Y; j++)
                    coins[i, j] = 
                        new Coin(new Point(i, j), 
                        content.Load<Texture2D>(@"Coin"));
            A = new CoinStarAlg(Size, coins);
            bool answer = A.solve();
            A.Answer();
        }

        public void CheckAround(int x, int y) {
            /*Starts from right to clockwise. Changes black to white and vice versa*/            
            if (x != Size.X - 1)
                coins[x + 1, y].CoinColor = flipCoin(coins[x + 1, y].CoinColor);
            if (y != Size.Y - 1)
                coins[x, y + 1].CoinColor = flipCoin(coins[x, y + 1].CoinColor);
            if (x != 0)
                coins[x - 1, y].CoinColor = flipCoin(coins[x - 1, y].CoinColor);
            if (y != 0)
                coins[x, y - 1].CoinColor = flipCoin(coins[x, y - 1].CoinColor);            
        }

        public Color flipCoin(Color col) {
            return col = (col == Color.Black) ? Color.White : Color.Black;
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (Coin c in coins)
                c.Draw(spriteBatch);
        }

        public void Update(MouseState mouseState, MouseState lastMouseState) {
            for( int i = 0; i < Size.X; i++ ) 
                for( int j = 0; j < Size.Y; j++ ) {
                    if (coins[i,j].Bounds.Contains(new Point(mouseState.X, mouseState.Y))) 
                        if (mouseState.LeftButton == ButtonState.Pressed &&
                            lastMouseState.LeftButton == ButtonState.Released) {
                            coins[i,j].CoinColor = flipCoin(coins[i,j].CoinColor);
                            CheckAround(i, j);
                        }
                    
                }
        }
    }
}
