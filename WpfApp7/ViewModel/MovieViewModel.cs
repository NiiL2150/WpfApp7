using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp7.Model;
using WpfApp7.Service;
using WpfApp7.Service.Command;

namespace WpfApp7.ViewModel
{
    public class MovieViewModel : INotifyPropertyChanged
    {
        public MovieViewModel(IFileService fileService)
        {
            this._fileService = fileService;

            Movies = new ObservableCollection<Movie>
            {
                new Movie{Name = "Harry Potter", Genre = "Fantasy", Imdb = 9.1f, Year = new DateTime(2018,10,10) },
                new Movie {Name = "Kill Bill", Genre = "Action", Imdb = 8.9f, Year = new DateTime(2000,02,12)},
                new Movie {Name = "End of Evangelion", Genre = "Anime", Imdb = 10f, Year = new DateTime(1990,01,01)}
            };
        }

        private Movie selectedMovie;
        private int selectedMovieIndex;
        private IFileService _fileService;
        public ObservableCollection<Movie> Movies { get; set; }
        private RelayCommand _saveCommand;
        private RelayCommand _openCommand;
        private RelayCommand _addCommand;
        private RelayCommand _removeCommand;

        public Movie SelectedMovie { get => selectedMovie;
            set 
            {
                selectedMovie = value;
                OnPropertyChanged(nameof(SelectedMovie));
            }
        }
        public int SelectedMovieIndex { get => selectedMovieIndex; set => selectedMovieIndex = value; }

        internal RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(obj =>
                _fileService.Save("movies.json", Movies.Select(m => new Movie
                {
                    Genre = m.Genre,
                    Imdb = m.Imdb,
                    Name = m.Name,
                    Year = m.Year
                }).ToList())
                ));
            }
        }

        internal RelayCommand OpenCommand
        {
            get
            {
                return _openCommand ?? (_openCommand = new RelayCommand(obj =>
                {
                    var movies = _fileService.Open("movies.json");
                    Movies.Clear();
                    foreach (var m in movies)
                    {
                        Movies.Add(m);
                    }
                }
                ));
            }
        }

        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new RelayCommand(obj =>
                {
                    Movie movie = new Movie();
                    movie.Genre = (obj as Movie).Genre;
                    movie.Imdb = (obj as Movie).Imdb;
                    movie.Name = (obj as Movie).Name;
                    movie.Year = (obj as Movie).Year;
                    Movies.Add(movie);
                    SelectedMovie = movie;
                }));
            }
        }

        public RelayCommand RemoveCommand
        {
            get
            {
                return _removeCommand ?? (_removeCommand = new RelayCommand(obj =>
                {
                    Movie movie = obj as Movie;
                    int selectedIndex = SelectedMovieIndex;
                    if (movie != null)
                    {
                        Movies.Remove(movie);
                    }
                    SelectedMovie = Movies.Count > 0 ? Movies[0] : null;
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;



        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
