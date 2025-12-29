namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]  //코드 스트리핑 방지 (이 코드도 같이 빌드해라)
    public unsafe class WeaponSystem : SystemMainThreadFilter<WeaponSystem.Filter>
    {
        public override void Update(Frame frame, ref Filter filter)
        {
            if(filter.Weapon->CooldownTime >= FP._0)
            {
                filter.Weapon->CooldownTime -= frame.DeltaTime;
                return;
            }

            var input = frame.GetPlayerInput(filter.Player->Player);
            if(input->Fire.WasPressed)
            {
                var weaponData = frame.FindAsset(filter.Weapon->WeaponData);
                filter.Weapon->CooldownTime = weaponData.Cooldown;
                frame.Signals.CreateBullet(filter.Entity, weaponData);
            }

        }

        public struct Filter
        {
            public EntityRef Entity;
            public PlayerLink* Player;
            public Weapon* Weapon;
        }
    }
}
