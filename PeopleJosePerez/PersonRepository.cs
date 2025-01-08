using PeopleRgualoto.Models;
using SQLite;

namespace PeopleRgualoto
{
    public class PersonRepository
    {
        string _dbPath;

        private SQLiteConnection conn;
        public string StatusMessage { get; set; }
        // TODO: Add variable for the SQLite connection
        private void Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<PersonRgualoto>();
        }

        public PersonRepository(string dbPath)
        {
            _dbPath = dbPath;
        }
        public void AddNewPerson(string name)
        {
            int result = 0;
            try
            {
                Init();
                // basic validation to ensure a name was entered
                if (string.IsNullOrEmpty(name))
                    throw new Exception("Valid name required");
                result = conn.Insert(new PersonRgualoto { Name = name });

                StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, name);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
            }
        }
        public List<PersonRgualoto> GetAllPeople()
        {
            try
            {
                Init();
                return conn.Table<PersonRgualoto>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }
            return new List<PersonRgualoto>();
        }

        public void DeletePerson(int id)
        {
            try
            {
                Init();
                var personToDelete = conn.Find<PersonRgualoto>(id);
                if (personToDelete != null)
                {
                    conn.Delete(personToDelete);
                    StatusMessage = $"Registro con Id {id} eliminado.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al eliminar el registro deseado: {ex.Message}";
            }
        }
    }
}
