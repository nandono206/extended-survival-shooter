public interface PetObserver
{
    public void OnNotify(string petTag);

    public void OnNotifyDead();
}