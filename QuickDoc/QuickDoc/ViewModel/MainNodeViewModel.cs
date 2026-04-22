using QuickDoc.Command;
using QuickDoc.Repository;
using QuickDoc.Stores;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace QuickDoc.ViewModel
{
    public class MainNodeViewModel : INotifyPropertyChanged
    {
        public MainNodeStateContainer priorNode;
        private string currentProjectNumber;

        private ItemRepository _itemRepo;
        private TagRepository _tagRepo;
        private SectionRepository _sectionRepo;
        private UnitRepository _unitRepo;
        private ProjectRepository _projectRepo;

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
        public ICommand GETBYSCAN { get; }
        public ICommand GOTOSCAN { get; }

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
            GETBYSCAN = new GetByScanCommand();
            GOTOSCAN = new GoToScanCommand();
        }

        public void GoInto()
        {
            if (priorNode != null)
            {
                MainNodeStateContainer priorNodeUnderConstruction = new MainNodeStateContainer(priorNode, CurrentNode, Children, Documents);
                priorNode = priorNodeUnderConstruction;
            }
            else
            {
                MainNodeStateContainer priorNodeUnderConstruction = new MainNodeStateContainer(CurrentNode, Children, Documents);
                priorNode = priorNodeUnderConstruction;
            }

            CurrentNode = SelectedChild;
            Children = new List<NodeViewModel>();
            Documents = new List<DocumentViewModel>();

            switch (CurrentNode)
            {
                case UnitViewModel:
                    foreach (var section in (currentNode as UnitViewModel).Sections)
                    {
                        Children.Add(new SectionViewModel(section));
                    }

                    foreach (var section in Children)
                    {
                        foreach (var document in (section as SectionViewModel).Documents)
                        {
                            Documents.Add(new DocumentViewModel(document));
                        }
                    }

                    foreach (var document in (CurrentNode as UnitViewModel).Documents)
                    {
                        Documents.Add(new DocumentViewModel(document));
                    }

                    break;
                case SectionViewModel:
                    foreach (var tag in (currentNode as SectionViewModel).Tags)
                    {
                        Children.Add(new TagViewModel(tag));
                    }

                    foreach (var tag in Children)
                    {
                        foreach (var document in (tag as TagViewModel).Documents)
                        {
                            Documents.Add(new DocumentViewModel(document));
                        }
                    }

                    foreach (var document in (CurrentNode as SectionViewModel).Documents)
                    {
                        Documents.Add(new DocumentViewModel(document));
                    }

                    break;
                case TagViewModel:
                    foreach (var item in (currentNode as TagViewModel).Items)
                    {
                        Children.Add(new ItemViewModel(item));
                    }

                    foreach (var item in Children)
                    {
                        foreach (var document in (item as ItemViewModel).Documents)
                        {
                            Documents.Add(new DocumentViewModel(document));
                        }
                    }

                    foreach (var document in (CurrentNode as TagViewModel).Documents)
                    {
                        Documents.Add(new DocumentViewModel(document));
                    }

                    break;
                case ItemViewModel:
                    foreach (var document in (CurrentNode as ItemViewModel).Documents)
                    {
                        Documents.Add(new DocumentViewModel(document));
                    }

                    break;
            }
        }

        public void GoBack()
        {
            if (priorNode != null)
            {
                CurrentNode = priorNode.CurrentNode;
                Children = priorNode.Children;
                Documents = priorNode.Documents;
                priorNode = priorNode.PriorNode;
            }
            else
            {
                CurrentNode = null;
                Children = new List<NodeViewModel>();
                Documents = new List<DocumentViewModel>();
                priorNode = null;
            }
        }

        public void GetByCriteria()
        {
            priorNode = null;

            bool projectFull = !string.IsNullOrEmpty(Criteria.ProjectCriteria);
            bool unitFull = !string.IsNullOrEmpty(Criteria.UnitCriteria);
            bool tagFull = !string.IsNullOrEmpty(Criteria.TagCriteria);
            bool itemFull = !string.IsNullOrEmpty(Criteria.ItemCriteria);
            bool sectionFull = Criteria.SectionCriteria != 0;

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

                if ( !(unitFull || sectionFull || tagFull || itemFull) ) //Looking for a specific project
                {
                    CurrentNode = new ProjectViewModel(_projectRepo.GetProject());
                    Children = new List<NodeViewModel>();
                    Documents = new List<DocumentViewModel>();

                    foreach (var unit in (CurrentNode as ProjectViewModel).Units)
                    {
                        Children.Add(new UnitViewModel(unit));

                        foreach (var document in unit.Documents)
                        {
                            Documents.Add(new DocumentViewModel(document));
                        }
                    }

                    foreach (var document in (CurrentNode as ProjectViewModel).Documents)
                    {
                        Documents.Add(new DocumentViewModel(document));
                    }
                }
                else if (tagFull || itemFull)
                {
                    if (tagFull && !itemFull) //Looking for a specific tag
                    {
                        CurrentNode = new TagViewModel(_tagRepo.GetTag(Criteria.TagCriteria));
                        Children = new List<NodeViewModel>();
                        Documents = new List<DocumentViewModel>();

                        foreach (var item in (CurrentNode as TagViewModel).Items)
                        {
                            Children.Add(new ItemViewModel(item));

                            foreach (var document in (item.Documents))
                            {
                                Documents.Add(new DocumentViewModel(document));
                            }
                        }

                        foreach (var document in (CurrentNode as TagViewModel).Documents)
                        {
                            Documents.Add(new DocumentViewModel(document));
                        }
                    }
                    else if ( (!tagFull && itemFull) || (tagFull && itemFull) ) //Looking for an item type (also catches the silly event where someone fills out tag at the same time)
                    {
                        CurrentNode = new ItemViewModel(_itemRepo.GetItem(Criteria.ItemCriteria));
                        Children.Clear();
                        Documents = new List<DocumentViewModel>();

                        foreach (var document in (CurrentNode as ItemViewModel).Documents)
                        {
                            Documents.Add(new DocumentViewModel(document));
                        }
                    }
                }
                else
                {
                    if (unitFull && sectionFull) //Looking for a specific section
                    {
                        CurrentNode = new SectionViewModel(_sectionRepo.getSection(Criteria.SectionCriteria, Criteria.UnitCriteria, _unitRepo));
                        Children = new List<NodeViewModel>();
                        Documents = new List<DocumentViewModel>();

                        foreach (var tag in (CurrentNode as SectionViewModel).Tags)
                        {
                            Children.Add(new TagViewModel(tag));

                            foreach (var document in tag.Documents)
                            {
                                Documents.Add(new DocumentViewModel(document));
                            }
                        }

                        foreach (var document in (CurrentNode as SectionViewModel).Documents)
                        {
                            Documents.Add(new DocumentViewModel(document));
                        }
                    }
                    else if (unitFull && !sectionFull) //Looking for a specific unit
                    {
                        CurrentNode = new UnitViewModel(_unitRepo.GetUnit(Criteria.UnitCriteria));
                        Children = new List<NodeViewModel>();
                        Documents = new List<DocumentViewModel>();

                        foreach (var section in (CurrentNode as UnitViewModel).Sections)
                        {
                            Children.Add(new SectionViewModel(section));

                            foreach (var document in section.Documents)
                            {
                                Documents.Add(new DocumentViewModel(document));
                            }
                        }

                        foreach (var document in (CurrentNode as UnitViewModel).Documents)
                        {
                            Documents.Add(new DocumentViewModel(document));
                        }
                    }
                    else if (!unitFull && sectionFull) //Looking for several specific sections
                    {
                        List<SectionViewModel> sections = new List<SectionViewModel>();
                        foreach (var section in _sectionRepo.GetSections(Criteria.SectionCriteria))
                        {
                            sections.Add(new SectionViewModel(section));
                        }
                        if (sections.Count == 0)
                        {
                            currentNode = new SectionViewModel(_sectionRepo.getSection(Criteria.SectionCriteria, Criteria.UnitCriteria, _unitRepo));
                        } 

                        else
                        {
                            CurrentNode = sections.First();
                            Children.Clear();
                            Documents = new List<DocumentViewModel>();

                            foreach (SectionViewModel section in sections)
                            {
                                foreach (var document in section.Documents)
                                {
                                    Documents.Add(new DocumentViewModel(document));
                                }
                            }
                        }
                    }
                }
            }
        }
        
        public void GetByScan()
        {
            string[] scanCriteria = Criteria.ScanCriteria.Split(';');

            string sectionString = scanCriteria[2];

            Criteria.ProjectCriteria = scanCriteria[0];
            Criteria.UnitCriteria = scanCriteria[1];

            if (int.TryParse(sectionString, out int sectionNumber))
            {
                Criteria.SectionCriteria = sectionNumber;
            }

            Criteria.TagCriteria = scanCriteria[3];
            Criteria.ItemCriteria = scanCriteria[4];

            GetByCriteria();
        }
    }
}