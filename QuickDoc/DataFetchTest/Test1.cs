using QuickDoc.Model;
using QuickDoc.Repository;
using QuickDoc.ViewModel;

namespace DataFetchTest
{
    [TestClass]
    public sealed class UnitTest1
    {
        private bool correctProject;
        private bool correctUnit;
        private bool correctSection;
        private bool correctTag;
        private bool correctItem;

        private MainNodeViewModel mnvmWithParent;
        private MainNodeViewModel mnvm;
        private MainNodeViewModel mnvmUpdateSerial;

        // Cleans up. 
        [TestInitialize]
        public void TestInitialize()
        {
            correctProject = false;
            correctUnit = false;
            correctSection = false;
            correctTag = false;
            correctItem = false;

            mnvmWithParent = new MainNodeViewModel();
            mnvm = new MainNodeViewModel();
            mnvmUpdateSerial = new MainNodeViewModel();
        }

        // Sets representative input fields to be used with different methods. 
        // Sets flags to reflect the correctness of the fields. 
        // This particular test takes correct input. 
        [TestMethod]
        public void CorrectInput()
        {
            correctProject = true;
            correctUnit = true;
            correctSection = true;
            correctTag = true;
            correctItem = true;

            string project = "P-0368486";
            string unit = "120";
            int section = 120;
            string tag = "B-12000-01";
            string item = "535547";

            RunGetByCriteriaTests(project, unit, section, tag, item);
        }

        // This test takes correct input except project being incorrect. 
        [TestMethod]
        public void IncorrectProjectInput()
        {
            correctUnit = true;
            correctSection = true;
            correctTag = true;
            correctItem = true;

            string project = "test";
            string unit = "120";
            int section = 120;
            string tag = "B-12000-01";
            string item = "535547";

            RunGetByCriteriaTests(project, unit, section, tag, item);
        }

        [TestMethod]
        public void IncorrectUnitInput()
        {
            correctProject = true;
            correctSection = true;
            correctTag = true;
            correctItem = true;

            string project = "P-0368486";
            string unit = "test";
            int section = 120;
            string tag = "B-12000-01";
            string item = "535547";

            RunGetByCriteriaTests(project, unit, section, tag, item);
        }

        [TestMethod]
        public void IncorrectSectionInput()
        {
            correctProject = true;
            correctUnit = true;
            correctTag = true;
            correctItem = true;

            string project = "P-0368486";
            string unit = "120";
            int section = -1;
            string tag = "B-12000-01";
            string item = "535547";

            RunGetByCriteriaTests(project, unit, section, tag, item);
        }

        [TestMethod]
        public void IncorrectTagInput()
        {
            correctProject = true;
            correctUnit = true;
            correctSection = true;
            correctItem = true;

            string project = "P-0368486";
            string unit = "120";
            int section = 120;
            string tag = "test";
            string item = "535547";

            RunGetByCriteriaTests(project, unit, section, tag, item);
        }

        [TestMethod]
        public void IncorrectItemInput()
        {
            correctProject = true;
            correctUnit = true;
            correctSection = true;
            correctTag = true;

            string project = "P-0368486";
            string unit = "120";
            int section = 120;
            string tag = "B-12000-01";
            string item = "test";

            RunGetByCriteriaTests(project, unit, section, tag, item);
        }

        // Runs various tests representing different permutations of filled input. 
        public void RunGetByCriteriaTests(string project, string unit, int section, string tag, string item)
        {
            LookingForSpecificProject(project);
            LookingForSpecificUnit(project, unit);
            LookingForSpecificSection(project, unit, section);
            LookingForSpecificTag(project, tag);
            LookingForSpecificItem(project, tag, item);
            LookingForSectionType(project, section);
            LookingForItemType(project, item);
        }

        // Checks whether the input project string matches the string ProjectNumber in the found node placed in CurrentNode after GetByCriteria is run. 
        public void LookingForSpecificProject(string project)
        {
            //Arrange
            string expectedProject;

            if (correctProject)
            {
                expectedProject = project;
            }
            else
            {
                expectedProject = null;
            }

            //Act
            mnvm.Criteria.ScanCriteria = ";;0;;";
            mnvm.Criteria.ProjectCriteria = project;
            mnvm.GetByCriteria();

            string actualProject = (mnvm.CurrentNode as ProjectViewModel).ProjectNumber;

            //Assert
            Assert.AreEqual(expectedProject, actualProject);

            GoingIntoFirst();
        }

        // Checks whether the input unit string matches the string UnitNumber in the found node placed in CurrentNode after GetByCriteria is run. 
        public void LookingForSpecificUnit(string project, string unit)
        {
            //Arrange
            string expectedUnit;

            if (correctProject && correctUnit)
            {
                expectedUnit = unit;
            }
            else
            {
                expectedUnit = null;
            }

            //Act
            mnvm.Criteria.ScanCriteria = ";;0;;";
            mnvm.Criteria.ProjectCriteria = project;
            mnvm.Criteria.UnitCriteria = unit;
            mnvm.GetByCriteria();

            string actualUnit = (mnvm.CurrentNode as UnitViewModel).UnitNumber;

            //Assert
            Assert.AreEqual(expectedUnit, actualUnit);

            GoingIntoFirst();
        }

        // Checks whether the input section int matches the int SectionNumber in the found node placed in CurrentNode after GetByCriteria is run. 
        // Furthermore checks whether its expected parent contains it. 
        public void LookingForSpecificSection(string project, string unit, int section)
        {
            //Arrange
            string expectedUnit;
            int expectedSection;

            if (correctProject && correctUnit && correctSection)
            {
                expectedUnit = unit;
                expectedSection = section;
            }
            else
            {
                expectedUnit = null;
                expectedSection = 0;
            }

            //Act
            mnvm.Criteria.ScanCriteria = ";;0;;";
            mnvm.Criteria.ProjectCriteria = project;
            mnvm.Criteria.UnitCriteria = unit;

            mnvmWithParent.Criteria = mnvm.Criteria;
            mnvmWithParent.GetByCriteria();

            mnvm.Criteria.SectionCriteria = section;
            mnvm.GetByCriteria();

            string actualSectionParent = null;
            int actualSection = (mnvm.CurrentNode as SectionViewModel).SectionNumber;

            List<NodeViewModel> foundSections = mnvmWithParent.CurrentNode.GetChildren().Where<NodeViewModel>(ivm => (ivm as SectionViewModel).SectionNumber == actualSection).ToList();
            if (foundSections.Count() > 0)
            {
                actualSectionParent = (mnvmWithParent.CurrentNode as UnitViewModel).UnitNumber;
            }

            //Assert
            Assert.AreEqual(expectedUnit, actualSectionParent);
            Assert.AreEqual(expectedSection, actualSection);

            GoingIntoFirst();
        }

        // Checks whether the input tag string matches the string TagNumber in the found node placed in CurrentNode after GetByCriteria is run. 
        public void LookingForSpecificTag(string project, string tag)
        {
            //Arrange
            string expectedTag;

            if (correctProject && correctTag)
            {
                expectedTag = tag;
            }
            else
            {
                expectedTag = null;
            }

            //Act
            mnvm.Criteria.ScanCriteria = ";;0;;";
            mnvm.Criteria.ProjectCriteria = project;
            mnvm.Criteria.TagCriteria = tag;
            mnvm.GetByCriteria();

            string actualTag = (mnvm.CurrentNode as TagViewModel).TagNumber;

            //Assert
            Assert.AreEqual(expectedTag, actualTag);

            GoingIntoFirst();
        }

        // Checks whether the input item string matches the string ItemNumber in the found node placed in CurrentNode after GetByCriteria is run. 
        // Furthermore checks whether its expected parent contains it. 
        public void LookingForSpecificItem(string project, string tag, string item)
        {
            //Arrange
            string expectedTag;
            string expectedItem;

            if (correctProject && correctTag && correctItem)
            {
                expectedTag = tag;
                expectedItem = item;
            }
            else
            {
                expectedTag = null;
                expectedItem = null;
            }

            //Act
            mnvm.Criteria.ScanCriteria = ";;0;;";
            mnvm.Criteria.ProjectCriteria = project;
            mnvm.Criteria.TagCriteria = tag;

            mnvmWithParent.Criteria = mnvm.Criteria;
            mnvmWithParent.GetByCriteria();

            mnvm.Criteria.ItemCriteria = item;
            mnvm.GetByCriteria();

            string actualItemParent = null;
            string actualItem = (mnvm.CurrentNode as ItemViewModel).ItemNumber;

            List<NodeViewModel> foundItems = mnvmWithParent.CurrentNode.GetChildren().Where<NodeViewModel>(ivm => (ivm as ItemViewModel).ItemNumber == actualItem).ToList();
            if (foundItems.Count() > 0)
            {
                actualItemParent = (mnvmWithParent.CurrentNode as TagViewModel).TagNumber;
            }

            //Assert
            Assert.AreEqual(expectedTag, actualItemParent);
            Assert.AreEqual(expectedItem, actualItem);

            UpdatingSerial();
            GoingIntoFirst();
        }

        // Checks whether the input section int matches the int SectionNumber in the found node placed in CurrentNode after GetByCriteria is run. 
        public void LookingForSectionType(string project, int section)
        {
            //Arrange
            int expectedSection;

            if (correctProject && correctSection)
            {
                expectedSection = section;
            }
            else
            {
                expectedSection = 0;
            }

            //Act
            mnvm.Criteria.ScanCriteria = ";;0;;";
            mnvm.Criteria.ProjectCriteria = project;
            mnvm.Criteria.SectionCriteria = section;
            mnvm.GetByCriteria();

            int actualSection = (mnvm.CurrentNode as SectionViewModel).SectionNumber;

            //Assert
            Assert.AreEqual(expectedSection, actualSection);

            GoingIntoFirst();
        }

        // Checks whether the input item string matches the string ItemNumber in the found node placed in CurrentNode after GetByCriteria is run. 
        public void LookingForItemType(string project, string item)
        {
            //Arrange
            string expectedItem;

            if (correctProject && correctItem)
            {
                expectedItem = item;
            }
            else
            {
                expectedItem = null;
            }

            //Act
            mnvm.Criteria.ScanCriteria = ";;0;;";
            mnvm.Criteria.ProjectCriteria = project;
            mnvm.Criteria.ItemCriteria = item;
            mnvm.GetByCriteria();

            string actualItem = (mnvm.CurrentNode as ItemViewModel).ItemNumber;

            //Assert
            Assert.AreEqual(expectedItem, actualItem);

            GoingIntoFirst();
        }

        // Checks whether PriorNode correctly points at the prior state when GoingInto. 
        public void GoingIntoFirst()
        {
            List<NodeViewModel> children = mnvm.CurrentNode.GetChildren();

            if (children.Count > 0)
            {
                //Arrange
                NodeViewModel expectedParent = mnvm.CurrentNode;

                //Act
                mnvm.SelectedChild = children.First();
                mnvm.GoInto();

                NodeViewModel actualParent = mnvm.PriorNode.CurrentNode;

                //Assert
                Assert.AreEqual(expectedParent, actualParent);
                
                GoingIntoFirst();
            }
            else
            {
                GoingBack();
            }
        }

        // Checks whether CurrentNode was updated correctly to the prior state contained in PriorNode after GoingBack. 
        public void GoingBack()
        {
            if (mnvm.PriorNode != null)
            {
                //Arrange
                NodeViewModel expectedParent = mnvm.PriorNode.CurrentNode;

                //Act
                mnvm.GoBack();

                NodeViewModel actualParent = mnvm.CurrentNode;

                //Assert
                Assert.AreEqual(expectedParent, actualParent);

                GoingBack();
            }
        }

        // Checks whether SerialNumber persists in the database,
        // this is achieved by running UpdateSerialNumber() and then checking a newly populated repository afterwards. 
        public void UpdatingSerial()
        {
            if (correctProject && correctTag && correctItem)
            {
                Random rnd = new Random();
                string testSerialNumber = $"test{rnd.Next(1, 10)}";

                //Arrange
                string expectedSerialNumber = testSerialNumber;
                
                CriteriaViewModel currentCriteria = mnvm.Criteria;
                (mnvm.CurrentNode as ItemViewModel).SerialNumber = expectedSerialNumber;

                //Act
                mnvm.UpdateSerialNumber();
                mnvmUpdateSerial.Criteria = currentCriteria;
                mnvmUpdateSerial.GetByCriteria();

                string actualSerialNumber = (mnvmUpdateSerial.CurrentNode as ItemViewModel).SerialNumber;

                //Assert
                Assert.AreEqual(expectedSerialNumber, actualSerialNumber);
            }
        }
    }
}