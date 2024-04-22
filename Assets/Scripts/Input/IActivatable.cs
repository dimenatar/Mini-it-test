public interface IActivatable
{
    public bool IsEnabled { get; }

    public void Enable();
    public void Disable();
}
