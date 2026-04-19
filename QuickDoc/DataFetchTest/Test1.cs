using QuickDoc.Model;
using QuickDoc.ViewModel;

namespace DataFetchTest
{
    [TestClass]
    public sealed class UnitTest1
    {
        MainNodeViewModel mnvm;
        NodeViewModel expectedCurrentNode;
        List<NodeViewModel> expectedChildren;
        
        [TestInitialize]
        public void TestInitialize()
        {
            mnvm = new MainNodeViewModel();
        }

        [TestMethod]
        public void LookingForSpecificProject()
        {
            //Arrange
            mnvm.Criteria.ProjectCriteria = "P-0368486";

            expectedChildren = new List<NodeViewModel>
            {
                new UnitViewModel(new Unit("120", "Knock out drum Unit No.")),
                new UnitViewModel(new Unit("122", "Booster blower Unit No.")),
                new UnitViewModel(new Unit("123", "Water scrubber Unit No.")),
                new UnitViewModel(new Unit("123-1", "Water scrubber pump Unit No.")),
                new UnitViewModel(new Unit("1700", "General Plant DK")),
                new UnitViewModel(new Unit("201-2", "CO2 Compressor Cooling Water Shunt Unit No.")),
                new UnitViewModel(new Unit("201A", "CO2 Compressor A Unit No.")),
                new UnitViewModel(new Unit("201B", "CO2 Compressor B Unit No.")),
                new UnitViewModel(new Unit("342", "HP Carbon filter unit, lead-lag Unit No.")),
                new UnitViewModel(new Unit("361", "Dehydrator Unit No.")),
                new UnitViewModel(new Unit("371", "Distillation column Unit No.")),
                new UnitViewModel(new Unit("381", "Liquefaction Unit No.")),
                new UnitViewModel(new Unit("401A", "Ref Compressor A Unit No.")),
                new UnitViewModel(new Unit("401B", "Ref Compressor B Unit No.")),
                new UnitViewModel(new Unit("451", "Open flash Unit No.")),
                new UnitViewModel(new Unit("503", "Cooling Water Pump Unit No.")),
                new UnitViewModel(new Unit("511A", "Air Cooler A Unit No.")),
                new UnitViewModel(new Unit("511B", "Air Cooler B Unit No.")),
                new UnitViewModel(new Unit("511C", "Air Cooler C Unit No.")),
                new UnitViewModel(new Unit("611A", "Storage tank A Unit No.")),
                new UnitViewModel(new Unit("611B", "Storage tank B Unit No.")),
                new UnitViewModel(new Unit("611C", "Storage tank C Unit No.")),
                new UnitViewModel(new Unit("641A", "Truck filling Unit No.")),
                new UnitViewModel(new Unit("999", "Plant Auxiliary Unit No."))
            };

            //Act
            mnvm.GetByCriteria();

            //Assert
            string expected = "";
            string actual = "";

            foreach (Unit unit in expectedUnits)
            {
                expected += unit.UnitNumber;
            }

            foreach (UnitViewModel unitVM in mnvm.Children)
            {
                actual += unitVM.UnitNumber;
            }

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LookingForSpecificUnit()
        {
            //Arrange
            mnvm.Criteria.ProjectCriteria = "P-0368486";
            mnvm.Criteria.UnitCriteria = "";

            Unit expectedUnit = new Unit();

            //ACT
            mnvm.GetByCriteria();

            //Assert
            string expected = expectedUnit.UnitNumber;
            string actual = (mnvm.CurrentNode as UnitViewModel).UnitNumber;

            Assert.AreEqual(expected, actual);
        }
    }
}
