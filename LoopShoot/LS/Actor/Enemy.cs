using LS.Device;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Actor
{
    class Enemy : Character
    {
        Status status;

        public Vector2 velocity;

        private bool gameStartFlag;
        private Vector2 targetPosition;

        public bool deleteFlag = false;

        float speed;

        public Enemy(string name, Vector2 pos, int life, Status status)
            : base("enemy")
        {
            this.name = name;
            position = pos;
            this.life = life;
            this.status = status;

        }


        public override void Move(Vector2 tPos)
        {
            speed = 1.5f;
            targetPosition = tPos;
            velocity = targetPosition - position;
            velocity.Normalize();
            position = position + velocity * speed;
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position);
        }

        public bool Delete()
        {
            return deleteFlag;
        }

        public override void Initialize()
        {
            gameStartFlag = false;
            damageNum = 0;
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
            if (other is Bullet)
            {
                life = Damage(damageNum);
            }
            else
                life = 0;

            if (life <= 0)
            {
                isDeadFlag = true;

            }
        }

        public override int GetStatus()
        {
            return (int)status;
        }

        public override int Damage(int damage)
        {
            life -= damage;
            return life;
        }
    }
}
