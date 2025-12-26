namespace Quantum
{
    using Photon.Deterministic;

    public unsafe class CharacterMovableSystem : SystemMainThreadFilter<CharacterMovableSystem.Filter>
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


    }
}
