using QuickDoc.Command;
using QuickDoc.Repository;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace QuickDoc.ViewModel
{
    public class MainNodeViewModel : INotifyPropertyChanged
    {
        //Make Private later cause they are exposed for unit testing
        private ItemRepository _itemRepo;
        private TagRepository _tagRepo;
        private SectionRepository _sectionRepo;
        private UnitRepository _unitRepo;
        private ProjectRepository _projectRepo;

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

        public ICommand GETBYCRITERIA { get; } = new GetByCriteriaCommand();

        public void GetByCriteria()
        {
            _itemRepo.ReadFromDatabase(Criteria.ProjectCriteria);
            _tagRepo.ReadFromDatabase(Criteria.ProjectCriteria, _itemRepo);
            _sectionRepo.ReadFromDatabase(Criteria.ProjectCriteria, _tagRepo);
            _unitRepo.ReadFromDatabase(Criteria.ProjectCriteria, _sectionRepo);
            _projectRepo.readFromDatabase(Criteria.ProjectCriteria, _unitRepo);

            bool projectFull = Criteria.ProjectCriteria != string.Empty; 
            bool unitFull = Criteria.UnitCriteria != string.Empty;
            bool sectionFull = Criteria.SectionCriteria != 0;
            bool tagFull = Criteria.TagCriteria != string.Empty;
            bool itemFull = Criteria.ItemCriteria != string.Empty;

            if (projectFull)
            {
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
                    if (tagFull && !itemFull) //Looking for a tag type
                    {
                        CurrentNode = new TagViewModel(_tagRepo.GetTag(criteria.TagCriteria));
                        Children.Clear();
                        Documents = new List<DocumentViewModel>();

                        foreach (var document in (CurrentNode as TagViewModel).Documents)
                        {
                            Documents.Add(new DocumentViewModel(document));
                        }
                    }
                    else if (!tagFull && itemFull) //Looking for an item type
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

                        foreach (var tag in (CurrentNode as SectionViewModel).Children)
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

                        foreach (var section in (CurrentNode as UnitViewModel).Children)
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
                    else if (!unitFull && sectionFull) //Looking for a section type ???
                    {
                        CurrentNode = new SectionViewModel(_sectionRepo.getSections(criteria.SectionCriteria).First());
                        Children.Clear();
                        Documents = new List<DocumentViewModel>();

                        foreach (var document in (CurrentNode as SectionViewModel).Documents)
                        {
                            Documents.Add(new DocumentViewModel(document));
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
        }
    }
}