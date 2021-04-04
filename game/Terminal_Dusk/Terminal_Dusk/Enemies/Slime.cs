using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal_Dusk
{
    class Slime : Enemy
    {
        public Slime(Texture2D sprite, Rectangle location) :base (sprite, location)
        {
            image = sprite;
            position = location;
            enemyLocation = new Vector2(location.X, location.Y);
        }


        //the slime will just stand still and jump for its attack and movement, very basic stuff
        //if theres more time I could implement the bone attack idea, but most likely not

        public override void Draw(SpriteBatch sb)
        {
            DrawJump(sb);
            base.Draw(sb);
        }

        public override void Load(string filename)
        {
            base.Load(filename);
        }

        public override void Save(string filename)
        {
            base.Save(filename);
        }

        public override string ToString()
        {
            return ($"Enemy:Slime({position}) - HP:{health} - State:{currentState}");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


        private void DrawJump(SpriteBatch sb)
        {
            sb.Draw(
                image,                    // - The texture to draw
                enemyLocation,                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    0,                          //   - This rectangle specifies
                    0,                          //	   where "inside" the texture
                    240,             //     to get pixels (We don't want to
                    240),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                0.5f,                           // - Scale (100% - no change)  //Should eventually take screenSize to keep main clean
                SpriteEffects.None,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

    }
}
