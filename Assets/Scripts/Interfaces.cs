public interface ISwitchable
{
    public bool IsActive { get; }
    void Activate();
    void Deactivate();
}

public interface IInteractable
{
    void InteractLogic();
}
