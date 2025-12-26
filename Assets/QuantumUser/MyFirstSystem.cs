namespace Quantum
{
    using Photon.Client;
    using Photon.Deterministic;

    //Preserve를 붙여야 빌드 때 실행 
    [Preserve] 
    public unsafe class MyFirstSystem : SystemMainThread
    {
        public override void Update(Frame frame)
        {
            Log.Info("Hellow");
        }
    }
}
