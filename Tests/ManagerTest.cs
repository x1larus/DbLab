using DbLab.DalPg.Entities.View;
using DbLab.DalPg.Managers;

namespace Tests
{

    [TestClass]
    public class ManagerTest
    {
        [TestMethod]
        public void ViewManagerTest()
        {
            var mng = new ViewManager();
            var a = mng.ReadViewEntity<VAccrualEntity>().Result;
            Task.Delay(1000);
        }
    }
}