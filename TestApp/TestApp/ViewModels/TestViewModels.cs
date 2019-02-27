using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestApp.Common;
using TestApp.Model;
using TestApp.Sqlite;
using Windows.UI.Popups;

namespace TestApp.ViewModels
{
    public class TestViewModels : INotifyPropertyChanged
    {
        public TestViewModels()
        {
            
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            var Handler = PropertyChanged;
            if (Handler != null)
                Handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand GuardarCommand
        {
            get
            {
                return new RelayCommand(Registar);
            }
        }
        

        private ObservableCollection<Result> _ListResult;
        public ObservableCollection<Result> ListResult
        {
            get
            {
                return _ListResult;
            }
            set
            {
                _ListResult = value;
                RaisePropertyChanged();
            }
        }

        private string _Cantidad;
        public string Cantidad
        {
            get
            {
                return _Cantidad;
            }
            set
            {
                _Cantidad = value;
                RaisePropertyChanged();
            }
        }
        public async Task<string> GetrespondeAsync()
        {
            HttpClient client = new HttpClient();
            string url = "https://randomuser.me/api?results=100";
            var response = client.GetAsync(url).Result;
            return response.Content.ReadAsStringAsync().Result;
        }
        public async void TsAsync()
        {
            var result = await GetrespondeAsync();
            JObject jObject = JObject.Parse(result);
            JArray array = (JArray)jObject["results"];
            Cantidad = $"Total de datos a registrar: {array.Count}";
            ListResult = array.ToObject<ObservableCollection<Result>>();
        }
        async void Registar()
        {
            SqliteTest sqliteTest = new SqliteTest();
            if(ListResult.Count > 0)
            {
                try
                {
                    List<User> users = new List<User>();
                    foreach (var item in ListResult)
                    {
                        var user = new User
                        {
                            Gender = item.gender,
                            Name = item.name.first,
                            Street = item.location.street,
                            Imagen = item.picture.medium
                        };
                        /*sqliteTest.Insertar(user)*/;
                        users.Add(user);
                    }
                    if (sqliteTest.Insertar(users))
                    {
                        
                        var m = new MessageDialog($"Se ha registrado la catidad de {sqliteTest.GetUsers().Count} con exito.");
                        await m.ShowAsync();
                    }
                    
                }
                catch (Exception ex)
                {
                    var m = new MessageDialog(ex.Message);
                    await m.ShowAsync();
                    throw;
                }
                
            }
        }
    }
}

