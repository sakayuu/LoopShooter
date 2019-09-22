using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace LS.Actor
{
    class Way : Character
    {
        public Way(Vector2 pos)
            :base("michi")
        {
            position = pos;
        }


        public override int Damage(int damage)
        {
            throw new NotImplementedException();
        }

        public override int GetStatus()
        {
            throw new NotImplementedException();
        }

        public override void Hit(Character other)
        {
            throw new NotImplementedException();
        }

        public override void Initialize()
        {
            
        }

        public override void Move(Vector2 tPos)
        {
            throw new NotImplementedException();
        }

        public override void Shutdown()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
