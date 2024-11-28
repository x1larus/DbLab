using DbLab.DalPg.Entities.View;
using DbLab.DalPg.Managers;
using System.Transactions;
using DbLab.DalPg.Entities;

namespace Tests
{
    [TestClass]
    public class ManagerTest
    {
        [TestMethod]
        public void ViewManagerTest()
        {
            var mng = new ViewManager();
            var vAccrualEntities = mng.ReadViewEntity<VAccrualEntity>().Result;
        }

        [TestMethod]
        public void AccrualTypeManagerTest()
        {
            var mng = new AccrualTypeManager();
            var readAll = mng.ReadAll().Result;
            var readById = mng.ReadById(1).Result;
        }

        [TestMethod]
        public void AccrualCategoryManagerTest()
        {
            var mng = new AccrualCategoryManager();
            var readByType = mng.ReadByType(1).Result;
            var readAll = mng.ReadAll().Result;
        }

        [TestMethod]
        public void AccrualManagerTest()
        {
            var mng = new AccrualManager();
            using (var scope = new TransactionScope())
            {
                var write = mng.Write(new AccrualEntity
                {
                    Id = null,
                    IdAccrualCategory = 1,
                    Amount = 150,
                    Comment = "testmethod",
                    AccrualDate = DateOnly.FromDateTime(DateTime.Now)
                }).Result;

                // scope.Complete(); не комплитим скоуп чтобы не засирать бд
            }
        }
    }
}