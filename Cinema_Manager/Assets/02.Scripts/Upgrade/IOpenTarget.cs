public interface IOpenTarget{
    public bool IsOpen { get; set; }
    public void ActiveObj(bool active, bool on = false);
}
