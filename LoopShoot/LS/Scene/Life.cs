using LS.Device;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Scene
{
    class Life
    {
        public int life;
        private int currentLife;
        Vector2 position;
        public Life(int lifeNum, Vector2 position)
        {
            Initialize();
            life = lifeNum;
            this.position = position;
        }

        public void Initialize()
        {
            life = 0;
            currentLife = 0;
        }

        public void Update(GameTime gameTime)
        {
            life = LifeUpdate();

        }

        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture("HeartLife", position, new Rectangle(320 - 64 * life, 0, 320, 64));
        }

        public int LifeUpdate()
        {
            life = currentLife;
            return life;
        }

        public void GetLife(int lifeNum)
        {
            currentLife = lifeNum;
        }
    }
}
