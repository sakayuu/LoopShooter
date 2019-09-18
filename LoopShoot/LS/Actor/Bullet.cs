using LS.Device;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Actor
{
    class Bullet : Character
    {
        Status status;

        Vector2 velocity;

        float speed;

        public Bullet(string name, Vector2 position)
            : base("bullet")
        {
            velocity = Vector2.Zero;
            this.name = name;
            this.position = position;

        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position);
        }

        public override void Move(Vector2 destination)
        {
            speed = 10;

            velocity = destination - position;
            velocity.Normalize();
            position = position + velocity * speed;
        }

        public override void Initialize()
        {
            status = Status.prototype;
            speed = 0;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Shutdown()
        {

        }

        public override void Hit(Character other)
        {

        }

        public override int Damage(int damage)
        {
            throw new NotImplementedException();
        }

        public override int GetStatus()
        {
            return (int)status;
        }

    }
}
