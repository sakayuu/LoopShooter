using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace LS.Actor
{
    class TurnPoint : Character
    {
        public enum MyDirection
        {
            Up,
            Down,
            Left,
            Right,
        }

        public MyDirection myDirection;
        public TurnPoint(Vector2 pos, MyDirection myDirection)
            : base("Way")
        {
            position = pos;
            this.myDirection = myDirection;
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
            //if(other is Enemy)

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

        public Vector2 NextPoint(Vector2 tPos)
        {
            if (myDirection == MyDirection.Up)
                tPos += new Vector2(0, -64);
            else if (myDirection == MyDirection.Down)
                tPos += new Vector2(0, +64);
            else if (myDirection == MyDirection.Left)
                tPos += new Vector2(-64, 0);
            else if (myDirection == MyDirection.Right)
                tPos += new Vector2(64, 0);
            return tPos;
        }
    }
}
