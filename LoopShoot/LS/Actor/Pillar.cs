using LS.Device;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Actor
{
    class Pillar : Character
    {
        public bool currentFlag;
        
        public bool hitFlag;

        public Pillar(string name, Vector2 pos)
            : base("white")
        {
            this.name = name;

            position = pos;

            hitFlag = false;
        }
        
        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position);
        }
        
        public override void Initialize()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Shutdown()
        {
            throw new NotImplementedException();
        }

        public override void Hit(Character other)
        {
            damageNum++;
        }

        public override int Damage(int damage)
        {
            throw new NotImplementedException();
        }

        public override int GetStatus()
        {
            throw new NotImplementedException();
        }

        public override void Move(Vector2 tPos)
        {
            throw new NotImplementedException();
        }
    }
}
