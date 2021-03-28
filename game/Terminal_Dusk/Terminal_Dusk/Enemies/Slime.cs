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
            this.image = sprite;
            this.position = location;
        }


        //the slime will just stand still and jump for its attack and movement, very basic stuff
        //if theres more time I could implement the bone attack idea, but most likely not

        public override void Draw(SpriteBatch sb)
        {
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
            return base.ToString();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
