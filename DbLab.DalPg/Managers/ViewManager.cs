using DbLab.DalPg.Base;

namespace DbLab.DalPg.Managers
{
    public class ViewManager : PostgresManagerBase
    {
        public async Task<List<T>> ReadViewEntity<T>()
        {
            return await SelectView<T>();
        }
    }
}
