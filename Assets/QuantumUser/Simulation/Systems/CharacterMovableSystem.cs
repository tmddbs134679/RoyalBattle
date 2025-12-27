namespace Quantum
{
    using Photon.Deterministic;
    using System;

    public unsafe class CharacterMovableSystem : SystemMainThreadFilter<CharacterMovableSystem.Filter>, ISignalOnTriggerEnter2D, ISignalOnTriggerExit2D
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

    
 

        public void OnTriggerEnter2D(Frame f, TriggerInfo2D info)
        {
            if (!f.TryGet(info.Entity, out PlayerLink playerLink))
                return;


            if (!f.TryGet<Grass>(info.Other, out _))
                return;

            f.Events.OnPlayerEnteredGrass(playerLink.Player);
        }

        public void OnTriggerExit2D(Frame f, ExitInfo2D info)
        {
            if (!f.TryGet(info.Entity, out PlayerLink playerLink))
                return;


            if (!f.TryGet<Grass>(info.Other, out _))
                return;

            f.Events.OnPlayerExitGrass(playerLink.Player);

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
