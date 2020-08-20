namespace R.BooBus
{
    public interface IPersistentConnection<TModel>
    {
        TModel GetModel();
    }
}
