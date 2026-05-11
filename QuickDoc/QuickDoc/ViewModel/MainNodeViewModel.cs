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
        // Indicates GOINGBACK is ongoing. Checked when CurrentNode is being set. 
        private bool goingBack;
        // When looking for a type, section or item, or an item in general. Checked when CurrentNode is being set. 
        private bool gettingNodeTypeOrItem;

        private ItemRepository _itemRepo;
        private TagRepository _tagRepo;
        private SectionRepository _sectionRepo;
        private UnitRepository _unitRepo;
        private ProjectRepository _projectRepo;

        // Contains an object representing the prior state of this object. 
        public MainNodeStateContainer PriorNode;
        public string CurrentProjectNumber { get; set; }
        public NavigationStore NavigationStore { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // Represents input boxes, we databind these. 
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

        // Where we put the thing we're, looking for, entering, at any given time, be it a project, unit, section, tag or item. 
        private NodeViewModel currentNode;
        // This set updates Documents and Children to the currently relevant contents determined by the ingoing inhabitant of currentNode. 
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

        // Commands, we command bind to these. Represents buttons. 
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

            // Most commands get datacontext as commandparameter, which is this object, usually.
            // GoInto and GoIntoSerial get the clicked child as commandparameter instead,
            // but they still need some references from in here to gray out buttons or update the NavigationStore. 
            GOINTO = new GoIntoCommand(this);
            GOBACK = new GoBackCommand();
            GETBYCRITERIA = new GetByCriteriaCommand();
            GOTOSCAN = new GoToScanCommand();
            GETBYSCAN = new GetByScanCommand();
            GOINTOSERIAL = new GoIntoSerialCommand(this);
            UPDATESERIAL = new UpdateSerialCommand();
        }

        // Handles this object's state when entering a node's child. 
        public void GoInto()
        {
            // If the current state has a prior state or not,
            // makes a new object representing the current state (and its potential prior state) and sets it to be the prior state. 
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

            // Cleans up. 
            Children = new List<NodeViewModel>();
            Documents = new List<DocumentViewModel>();

            // Notifies the CurrentNode setter whether we're about to make an Item the primary subject. 
            if (SelectedChild is ItemViewModel)
            {
                gettingNodeTypeOrItem = true;
            }

            CurrentNode = SelectedChild;
            SelectedChild = null;
        }

        // Handles this object's state when going back. 
        public void GoBack()
        {
            // Notifies the CurrentNode setter that we're currently in the midst of going back. 
            goingBack = true;

            // Sets the current state using the prior state's information. 
            if (PriorNode != null)
            {
                CurrentNode = PriorNode.CurrentNode;
                Children = PriorNode.Children;
                Documents = PriorNode.Documents;
                PriorNode = PriorNode.PriorNode;
            }
            // Cleans up in case we're going back and there is no prior state in prep for entering SearchView. 
            // (which does not center around CurrentNode being the primary subject)
            else
            {
                CurrentNode = null;
                Children = new List<NodeViewModel>();
                Documents = new List<DocumentViewModel>();
                PriorNode = null;
            }
        }

        // asd
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
                if ( !(CurrentProjectNumber == Criteria.ProjectCriteria) )
                {
                    _itemRepo.ReadFromDatabase(Criteria.ProjectCriteria);
                    _tagRepo.ReadFromDatabase(Criteria.ProjectCriteria, _itemRepo);
                    _sectionRepo.ReadFromDatabase(Criteria.ProjectCriteria, _tagRepo);
                    _unitRepo.ReadFromDatabase(Criteria.ProjectCriteria, _sectionRepo);
                    _projectRepo.ReadFromDatabase(Criteria.ProjectCriteria, _unitRepo);

                    CurrentProjectNumber = Criteria.ProjectCriteria;
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