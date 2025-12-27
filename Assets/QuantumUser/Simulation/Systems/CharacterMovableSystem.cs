namespace Quantum
{
    using Photon.Deterministic;
    using System;

    public unsafe class CharacterMovableSystem : SystemMainThreadFilter<CharacterMovableSystem.Filter>
    {
   
        public override void Update(Frame frame, ref Filter filter)
        {

            var input = frame.GetPlayerInput(filter.PlayerLink->Player);

            MovePlayer(frame, filter, input);
            RotatePlayer(frame, filter, input);
        }

  
        private static void MovePlayer(Frame frame, Filter filter, Input* input)
        {
            var dir = input->Dir;

            if (dir.Magnitude > 1)
            {
                dir = dir.Normalized;
            }

            var kccSettings = frame.FindAsset(filter.KCC->Settings);
            kccSettings.Move(frame, filter.Entity, dir);
        }
        private void RotatePlayer(Frame frame, Filter filter, Input* input)
        {
            var dir = input->MousePos - filter.Transform->Position;
            filter.Transform->Rotation = FPVector2.RadiansSigned(FPVector2.Up, dir);
        }


        public struct Filter
        {
            public EntityRef Entity;
            public KCC* KCC;
            public PlayerLink* PlayerLink;
            public Transform2D* Transform;
        }


    }
}
