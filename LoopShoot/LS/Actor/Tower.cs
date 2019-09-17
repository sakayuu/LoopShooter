using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace LS.Actor
{
    class Tower : Character
    {
        public int st;

        public Tower(int life, Vector2 pos)
            : base("pin")
        {
            this.life = life;
            position = pos;
            Initialize();
        }

        public override void Hit(Character other)
        {
            if (st == 1)
                life = Damage(2);
            else
                life = Damage(1);

        }

        public override void Initialize()
        {
            st = 0;
        }

        public override void Shutdown()
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (life <= 0)
            {
                isDeadFlag = true;
            }
        }

        public override int Damage(int damage)
        {
            life -= damage;
            return life;
        }

        public override int GetStatus()
        {
            throw new NotImplementedException();
        }

        public override void Move(Vector2 tPos)
        {

        }
    }
}
