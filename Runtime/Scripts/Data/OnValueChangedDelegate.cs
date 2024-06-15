namespace LCHFramework.Data
{
    public delegate void OnValueChangedDelegate<in T>(T prevOrNull, T current);
}