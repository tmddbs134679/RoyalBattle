namespace Quantum
{
    using Photon.Deterministic;
    using Quantum.Collections;
    using System;
    using UnityEngine.Scripting;

    // SystemSignalsOnly : 퀀텀의 시스템 중 시그널만 처리하는 특수한 시스템 타입
    // ex)  플레이어 스폰 시에만 실행, 점수 업데이트시에만 실행 -> 성능 최적화

    [Preserve]
    public unsafe class SpawnSystem : SystemSignalsOnly, ISignalOnPlayerAdded
    {



        public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
        {
            if (!firstTime)
                return;
     
            var playerEntityRef =  CreatePlayer(f, player);
            PlacePlayerOnSpawnPosition(f, playerEntityRef);


        }

        private void PlacePlayerOnSpawnPosition(Frame frame, EntityRef playerEntityRef)
        {
            var spawnPointMnager = frame.Unsafe.GetPointerSingleton<SpawnPointManager>();
            var availableSpawnPoints = frame.ResolveList(spawnPointMnager->AvaliableSpawnPoints);
            var usedSpawnPoints = frame.ResolveList(spawnPointMnager->UsedSpawnPoints);

            if (availableSpawnPoints.Count == 0 && usedSpawnPoints.Count == 0)
            {
                foreach(var componentPair in frame.GetComponentIterator<SpawnPoint>())
                {
                    availableSpawnPoints.Add(componentPair.Entity);
                }
            }
            else if(availableSpawnPoints.Count == 0 && availableSpawnPoints.Count != 0)
            {
                spawnPointMnager->AvaliableSpawnPoints = usedSpawnPoints;
                spawnPointMnager->UsedSpawnPoints = new QListPtr<EntityRef>();
                availableSpawnPoints = usedSpawnPoints;
                usedSpawnPoints = frame.ResolveList(spawnPointMnager->UsedSpawnPoints);
            }
            
            var randomIdx = frame.RNG->Next(0, availableSpawnPoints.Count);
            var spawnPoint = availableSpawnPoints[randomIdx];
            var spawnPointTransfrom = frame.Get<Transform2D>(spawnPoint);
            var playerTransform = frame.Unsafe.GetPointer<Transform2D>(playerEntityRef);
            playerTransform->Position = spawnPointTransfrom.Position;

            availableSpawnPoints.RemoveAt(randomIdx);
            usedSpawnPoints.Add(spawnPoint);

            if(availableSpawnPoints.Count == 0)
            {
                spawnPointMnager->AvaliableSpawnPoints = usedSpawnPoints;
                spawnPointMnager->UsedSpawnPoints = new QListPtr<EntityRef>();
            }
        }

        private static EntityRef CreatePlayer(Frame f, PlayerRef player)
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


            return playerEntity;
        }
    }
}
