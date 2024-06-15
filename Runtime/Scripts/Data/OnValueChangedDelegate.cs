namespace LCHFramework.Data
{
    public delegate void OnValueChangedDelegate();
    
    public delegate void OnValueChangedDelegate<in T>(T prev, T current);
}