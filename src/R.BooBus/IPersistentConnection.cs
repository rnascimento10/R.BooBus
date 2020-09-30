namespace R.BooBus
{
    public interface IPersistentConnection<out TModel>
    {
        TModel GetModel();
    }
}
