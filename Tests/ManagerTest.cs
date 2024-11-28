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
            var a = mng.SelectVAccrual().Result;
            Task.Delay(1000);
        }
    }
}