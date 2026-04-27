using QuickDoc.Model;
using QuickDoc.ViewModel;

namespace DataFetchTest
{
    [TestClass]
    public sealed class UnitTest1
    {
        MainNodeViewModel mnvm;

        [TestInitialize]
        public void TestInitialize()
        {
            mnvm = new MainNodeViewModel();
        }

        [TestMethod]
        public void LookingForSpecificProject()
        {
            //Arrange
            string expected = "P-0368486";

            //Act
            mnvm.Criteria.ProjectCriteria = expected;
            mnvm.GETBYCRITERIA.Execute(mnvm);

            string actual = (mnvm.CurrentNode as ProjectViewModel).ProjectNumber;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        public void LookingForSpecificUnit()
        {
            //Arrange
            string expectedProject = "P-0368486";
            string expectedUnit = "120";

            //Act
            mnvm.Criteria.ProjectCriteria = expectedProject;
            mnvm.Criteria.UnitCriteria = expectedUnit;
            mnvm.GETBYCRITERIA.Execute(mnvm);

            string actualUnit = (mnvm.CurrentNode as UnitViewModel).UnitNumber;

            //Assert
            Assert.AreEqual(expectedUnit, actualUnit);
        }
    }
}