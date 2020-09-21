using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework;

namespace Collection_of_Useful_Code_Bases__Some_Monogame_
{


    interface IStaticSprite
    {
        // Defines generic behaviour for Sprites.
        ContentManager Content { get; }
        Position Position { get; }
        Color Color { get; }
        Texture2D Image { get; }
        
        


        void Draw(SpriteBatch sb);

    }

    /// <summary>
    /// Interface similar to IStaticSprite, but defining Move() method.
    /// </summary>
    interface IMobileSprite : IStaticSprite
    {
        /// <summary>
        /// Defines a movement method that takes a parameter that if left empty, is null, and so uses the Speed property.
        /// </summary>
        /// <param name="movement"></param>
        void Move(int? movement = null);
        void Update(); // Calls anything needed for updating.
        /// <summary>
        /// Takes a path finding function, which takes three parameters,
        /// for the IMobileSprite's position, the target position, and 
        /// the return position.
        /// </summary>
        /// <param name="pathfinder"></param>
        void Pathfinding(Func<Position, Position, Position> pathfinder);
    }



    class StaticSprite : IStaticSprite
    {
        public ContentManager Content { get; }

        public Position Position { get; }
        public Texture2D Image { get; }
        public Color Color { get; }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Image, Position, Color);
        }

        /// <summary>
        /// Takes the image name as string and a Vector2 for the position.
        /// 
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="imageName"></param>
        /// <param name="topLeft"></param>
        public StaticSprite (ContentManager Content, string imageName, Vector2 topLeft, Color color)
            : this(Content, Content.Load<Texture2D>(imageName), topLeft, color) { }

        /// <summary>
        /// Takes the image as a Texture2D and Vector2 for position.
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="image"></param>
        /// <param name="topLeft"></param>
        /// <param name="color"></param>
        public StaticSprite (ContentManager Content, Texture2D image, Vector2 topLeft, Color color)
        {
            this.Content = Content;
            this.Image = image;
            this.Color = color;
            this.Position = new Position(topLeft, new Vector2(Image.Width, Image.Height));
        }

    }




    class Sprite // later inherit this from StaticSprite
    {
        public ContentManager Content { get; }
        public Position Position { get; }
        public Texture2D Image { get; }
        public Color Color { get; }

    }
}
