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

        public ICommand GOINTO { get; } = new GoIntoCommand();
        public ICommand GOBACK { get; } = new GoBackCommand();
        public ICommand GETBYCRITERIA { get; } = new GetByCriteriaCommand();

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
                case ProjectViewModel:
                    foreach (var unit in (CurrentNode as ProjectViewModel).Units)
                    {
                        foreach (var document in unit.Documents)
                        {
                            Documents.Add(new DocumentViewModel(document));
                        }
                    }

                    foreach (var document in (CurrentNode as ProjectViewModel).Documents)
                    {
                        Documents.Add(new DocumentViewModel(document));
                    }

                    break;
                case UnitViewModel:
                    foreach (var section in (CurrentNode as UnitViewModel).Sections)
                    {
                        foreach (var document in section.Documents)
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
                    foreach (var tag in (CurrentNode as SectionViewModel).Tags)
                    {
                        foreach (var document in tag.Documents)
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
                    foreach (var item in (CurrentNode as TagViewModel).Items)
                    {
                        foreach (var document in item.Documents)
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
                if ( !(currentProjectNumber == criteria.ProjectCriteria) )
                {
                    _itemRepo.ReadFromDatabase(Criteria.ProjectCriteria);
                    _tagRepo.ReadFromDatabase(Criteria.ProjectCriteria, _itemRepo);
                    _sectionRepo.ReadFromDatabase(Criteria.ProjectCriteria, _tagRepo);
                    _unitRepo.ReadFromDatabase(Criteria.ProjectCriteria, _sectionRepo);
                    _projectRepo.ReadFromDatabase(Criteria.ProjectCriteria, _unitRepo);

                    currentProjectNumber = criteria.ProjectCriteria;
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
                        CurrentNode = new TagViewModel(_tagRepo.GetTag(criteria.TagCriteria));
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
                        CurrentNode = new ItemViewModel(_itemRepo.GetItem(criteria.ItemCriteria));
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
                        CurrentNode = new SectionViewModel(_sectionRepo.getSection(criteria.SectionCriteria, criteria.UnitCriteria, _unitRepo));
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
                        CurrentNode = new UnitViewModel(_unitRepo.GetUnit(criteria.UnitCriteria));
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
                        foreach (var section in _sectionRepo.GetSections(criteria.SectionCriteria))
                        {
                            sections.Add(new SectionViewModel(section));
                        }
                        if (sections.Count == 0)
                        {
                            currentNode = new SectionViewModel(_sectionRepo.getSection(criteria.SectionCriteria, criteria.UnitCriteria, _unitRepo));
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
        }
    }
}