using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LS.Device;
using Microsoft.Xna.Framework;

namespace LS.Actor
{
    class RayShot : Character
    {
        float speed;
        Vector2 velocity;

        public RayShot(string name, Vector2 pos)
            :base("particle")
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
            if (other is MouseCol)
                IsDead();
        }

        public override void Initialize()
        {
            position = Vector2.Zero;
        }

        public override void Move(Vector2 tPos)
        {
            speed = 10f;
            velocity = tPos - position;
            velocity.Normalize();
            position = position + velocity * speed;
        }

        public override void Shutdown()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position);
        }
    }
}
