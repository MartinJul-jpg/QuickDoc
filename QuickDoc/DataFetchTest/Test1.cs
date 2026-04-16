using QuickDoc.ViewModel;
namespace DataFetchTest
{
    [TestClass]
    public sealed class UnitTest1
    {
        MainNodeViewModel mainNodeViewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            mainNodeViewModel = new MainNodeViewModel();
        }


        [TestMethod]
        public void GetProjectChildren()
        {


            //Arrange
            mainNodeViewModel.Criteria.ProjectCriteria = "P - 0368486";

            //ACT
            mainNodeViewModel.GetByCriteria();

            //Assert
            Assert.AreEqual(0, 0);
        }
    }
}
