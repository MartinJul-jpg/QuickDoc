using QuickDoc.Command;
using QuickDoc.Repository;
using QuickDoc.Stores;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace QuickDoc.ViewModel
{
    public class MainNodeViewModel : INotifyPropertyChanged
    {
        private bool goingBack;
        private bool gettingNodeTypeOrItem;
        private string currentProjectNumber;

        private ItemRepository _itemRepo;
        private TagRepository _tagRepo;
        private SectionRepository _sectionRepo;
        private UnitRepository _unitRepo;
        private ProjectRepository _projectRepo;

        public MainNodeStateContainer PriorNode;
        public NavigationStore NavigationStore { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private CriteriaViewModel criteria;
        public CriteriaViewModel Criteria
        {
            get { return criteria; }
            set 
            { 
                criteria = value;
                OnPropertyChanged("Criteria");
            }
        }

        private NodeViewModel currentNode;
        public NodeViewModel CurrentNode
        {
            get { return currentNode; }
            set 
            {
                currentNode = value;

                if (goingBack)
                {
                    goingBack = false;
                }
                else
                {
                    Documents = currentNode.GetDocuments();

                    if (gettingNodeTypeOrItem)
                    {
                        gettingNodeTypeOrItem = false;
                    }
                    else
                    {
                        Children = currentNode.GetChildren();

                        foreach (NodeViewModel child in Children)
                        {
                            Documents.Concat<DocumentViewModel>(child.GetDocuments());
                        }
                    }
                }

                OnPropertyChanged("CurrentNode");
            }
        }

        private List<NodeViewModel> children;
        public List<NodeViewModel> Children
        {
            get { return children; }
            set
            {
                children = value;
                OnPropertyChanged("Children");
            }
        }

        private NodeViewModel selectedChild;
        public NodeViewModel SelectedChild
        {
            get { return selectedChild; }
            set
            {
                selectedChild = value;
                OnPropertyChanged("SelectedChild");
            }
        }

        private List<DocumentViewModel> documents;
        public List<DocumentViewModel> Documents
        {
            get { return documents; }
            set 
            { 
                documents = value;
                OnPropertyChanged("Documents");
            }
        }

        public ICommand GOINTO { get; }
        public ICommand GOBACK { get; }
        public ICommand GETBYCRITERIA { get; }
        public ICommand GOTOSCAN { get; }
        public ICommand GETBYSCAN { get; }
        public ICommand GOINTOSERIAL { get; }
        public ICommand UPDATESERIAL { get; }

        public MainNodeViewModel()
        {
            _itemRepo = new ItemRepository();
            _tagRepo = new TagRepository();
            _sectionRepo = new SectionRepository();
            _unitRepo = new UnitRepository();
            _projectRepo = new ProjectRepository();

            criteria = new CriteriaViewModel();
            children = new List<NodeViewModel>();
            documents = new List<DocumentViewModel>();

            GOINTO = new GoIntoCommand(this);
            GOBACK = new GoBackCommand();
            GETBYCRITERIA = new GetByCriteriaCommand();
            GOTOSCAN = new GoToScanCommand();
            GETBYSCAN = new GetByScanCommand();
            GOINTOSERIAL = new GoIntoSerialCommand(this);
            UPDATESERIAL = new UpdateSerialCommand();
        }

        public void GoInto()
        {
            if (PriorNode != null)
            {
                MainNodeStateContainer priorNodeUnderConstruction = new MainNodeStateContainer(PriorNode, CurrentNode, Children, Documents);
                PriorNode = priorNodeUnderConstruction;
            }
            else
            {
                MainNodeStateContainer priorNodeUnderConstruction = new MainNodeStateContainer(CurrentNode, Children, Documents);
                PriorNode = priorNodeUnderConstruction;
            }

            Children = new List<NodeViewModel>();
            Documents = new List<DocumentViewModel>();

            if (SelectedChild is ItemViewModel)
            {
                gettingNodeTypeOrItem = true;
            }

            CurrentNode = SelectedChild;
            SelectedChild = null;
        }

        public void GoBack()
        {
            goingBack = true;

            if (PriorNode != null)
            {
                CurrentNode = PriorNode.CurrentNode;
                Children = PriorNode.Children;
                Documents = PriorNode.Documents;
                PriorNode = PriorNode.PriorNode;
            }
            else
            {
                CurrentNode = null;
                Children = new List<NodeViewModel>();
                Documents = new List<DocumentViewModel>();
                PriorNode = null;
            }
        }

        public void GetByCriteria()
        {
            PriorNode = null;

            bool projectFull = !string.IsNullOrEmpty(Criteria.ProjectCriteria);
            bool unitFull = !string.IsNullOrEmpty(Criteria.UnitCriteria);
            bool sectionFull = Criteria.SectionCriteria != 0;
            bool tagFull = !string.IsNullOrEmpty(Criteria.TagCriteria);
            bool itemFull = !string.IsNullOrEmpty(Criteria.ItemCriteria);

            if (projectFull)
            {
                if ( !(currentProjectNumber == Criteria.ProjectCriteria) )
                {
                    _itemRepo.ReadFromDatabase(Criteria.ProjectCriteria);
                    _tagRepo.ReadFromDatabase(Criteria.ProjectCriteria, _itemRepo);
                    _sectionRepo.ReadFromDatabase(Criteria.ProjectCriteria, _tagRepo);
                    _unitRepo.ReadFromDatabase(Criteria.ProjectCriteria, _sectionRepo);
                    _projectRepo.ReadFromDatabase(Criteria.ProjectCriteria, _unitRepo);

                    currentProjectNumber = Criteria.ProjectCriteria;
                }

                Children = new List<NodeViewModel>();
                Documents = new List<DocumentViewModel>();

                if ( !(unitFull || sectionFull || tagFull || itemFull) ) //Looking for a specific project
                {
                    CurrentNode = new ProjectViewModel(_projectRepo.GetProject());
                }
                else if (tagFull || itemFull)
                {
                    if (tagFull && !itemFull) //Looking for a specific tag
                    {
                        CurrentNode = new TagViewModel(_tagRepo.GetTag(Criteria.TagCriteria));
                    }
                    else if (!tagFull && itemFull) //Looking for an item type
                    {
                        gettingNodeTypeOrItem = true;

                        CurrentNode = new ItemViewModel(_itemRepo.GetItemOfType(Criteria.ItemCriteria));
                    }
                    else if (tagFull && itemFull) //Looking for a specific item
                    {
                        gettingNodeTypeOrItem = true;

                        CurrentNode = new ItemViewModel(_itemRepo.GetItem(Criteria.TagCriteria, Criteria.ItemCriteria));
                    }
                }
                else
                {
                    if (unitFull && sectionFull) //Looking for a specific section
                    {
                        CurrentNode = new SectionViewModel(_sectionRepo.getSection(Criteria.SectionCriteria, Criteria.UnitCriteria, _unitRepo));
                    }
                    else if (unitFull && !sectionFull) //Looking for a specific unit
                    {
                        CurrentNode = new UnitViewModel(_unitRepo.GetUnit(Criteria.UnitCriteria));
                    }
                    else if (!unitFull && sectionFull) //Looking for several specific sections, in other words a section type
                    {
                        gettingNodeTypeOrItem = true;

                        if (_sectionRepo.GetSections(criteria.SectionCriteria).Count != 0)
                        {
                            CurrentNode = new SectionViewModel(_sectionRepo.GetSections(Criteria.SectionCriteria).First());
                        }
                        else
                        {
                            CurrentNode = new SectionViewModel(_sectionRepo.getSection(Criteria.SectionCriteria, Criteria.UnitCriteria, _unitRepo));
                        }
                    }
                }
            }
        }

        public void GetByScan()
        {
            GetByCriteria();
        } 

        public void UpdateSerialNumber()
        {
            _itemRepo.UpdateSerialNumber((CurrentNode as ItemViewModel).ItemID, (CurrentNode as ItemViewModel).SerialNumber);
        }
    }
}