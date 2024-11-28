using DbLab.DalPg.Base;
using DbLab.DalPg.Entities.View;

namespace DbLab.DalPg.Managers
{
    public class ViewManager : PostgresManagerBase
    {
        public async Task<List<VAccrualEntity>> SelectVAccrual()
        {
            return await SelectView<VAccrualEntity>();
        }
    }
}
