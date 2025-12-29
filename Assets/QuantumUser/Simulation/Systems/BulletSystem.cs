namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class BulletSystem : SystemMainThreadFilter<BulletSystem.Filter>, ISignalCreateBullet
    {
       
        public override void Update(Frame frame, ref Filter filter)
        {
            filter.Transform->Position += filter.Bullet->Direction * filter.Bullet->Speed * frame.DeltaTime;
        }

        public struct Filter
        {
            public EntityRef Entity;
            public Bullet* Bullet;
            public Transform2D* Transform;
        }


        public void CreateBullet(Frame f, EntityRef owner, WeaponData weaponData)
        {
            var bulletData = weaponData.bulletData;
            var bulletEntity = f.Create(bulletData.Bullet);
            var bulletTransform = f.Unsafe.GetPointer<Transform2D>(bulletEntity);
            var ownersTransform = f.Get<Transform2D>(owner);

            bulletTransform->Position = ownersTransform.Position + weaponData.offset.XZ.Rotate(ownersTransform.Rotation);
            bulletTransform->Rotation = ownersTransform.Rotation;
            var bullet = f.Unsafe.GetPointer<Bullet>(bulletEntity);
            bullet->Speed = bulletData.Speed;
            bullet->Damage = bulletData.Damage;
            bullet->Owner = owner;
            bullet->HeightOffset = weaponData.offset.Y;
            bullet->Time = bulletData.Duration;
            bullet->Direction = ownersTransform.Up;

        }


    }
}
