using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace LS.Actor
{
    class MouseCol : Character
    {
        public MouseCol(string name,Vector2 pos)
            :base("white")
        {
            this.name = name;
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
            if (other is Pillar)
                putPossibleFlag = false;
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
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
