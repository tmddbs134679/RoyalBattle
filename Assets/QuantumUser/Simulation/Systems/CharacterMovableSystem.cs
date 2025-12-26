namespace Quantum
{
    using Photon.Deterministic;

    public unsafe class CharacterMovableSystem : SystemMainThreadFilter<CharacterMovableSystem.Filter>, ISignalOnPlayerAdded
    {
   
        public override void Update(Frame frame, ref Filter filter)
        {
            
            var input = frame.GetPlayerInput(filter.PlayerLink->Player);

            var dir = input->Dir;

            if(dir.Magnitude > 1)
            {
                dir = dir.Normalized;
            }
       
            var kccSettings = frame.FindAsset(filter.KCC->Settings);
            kccSettings.Move(frame, filter.Entity, dir);


        }

        public struct Filter
        {
            public EntityRef Entity;
            public KCC* KCC;
            public PlayerLink* PlayerLink;
        }

        public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)     
        {
            Log.Info("Player Connected at frame ");

            var playerData = f.GetPlayerData(player);
            var playerEntity = f.Create(playerData.PlayerAvatar);
            var playerLink = new PlayerLink()
            {
                Player = player
            };
            f.Add(playerEntity, playerLink);
            var kcc = f.Unsafe.GetPointer<KCC>(playerEntity);
            var kccSettings = f.FindAsset(kcc->Settings);

            kcc->Acceleration = kccSettings.Acceleration;
            kcc->MaxSpeed = kccSettings.BaseSpeed;

        }


    }
}
