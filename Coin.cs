using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace HW6
{
    class Coin {

        Texture2D sprite;
        public Point Position { get; private set; }
        public Rectangle Bounds { get; private set; }
        public Side side { get; set; }
        public Color CoinColor { get; set; }
        static Random rand = new Random();

        public Coin(Point position, Texture2D texture) {
            Position = position;
            sprite = texture;
            Bounds = new Rectangle(position.X * sprite.Width, position.Y * sprite.Height, sprite.Width, sprite.Height);
            Randomize();
        }

        public void Randomize() {
            CoinColor = (rand.Next(100) % 2) == 0 ? Color.Black : Color.White;            
        }

        public void Draw(SpriteBatch spriteBatch) {
                spriteBatch.Draw(sprite, Bounds, CoinColor);
        }

        public void Update(MouseState mouseState, MouseState lastMousestate)
        {
            
            if (Bounds.Contains(new Point(mouseState.X, mouseState.Y)))
            {
                if (mouseState.LeftButton == ButtonState.Pressed && lastMousestate.LeftButton == ButtonState.Released )
                    CoinColor = (CoinColor == Color.Black) ? Color.White : Color.Black;
            }
        }

    }
}
