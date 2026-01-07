using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace pet_clinic_rest_client.src;



internal class Client : IClient
{
    static readonly HttpClient client = new HttpClient();
    private const string Url = "http://localhost:9966/petclinic/api/";

    /* Models */
    internal class VetSpeciality
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    internal class Vet
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public List<VetSpeciality> specialities { get; set; }
    }

    internal class Visit
    {
        public int id { get; set; }
        public string description { get; set; }
        public int petId { get; set; }
        public DateTime date { get; set; }
        public override string ToString()
        {
            return "id: " + id + " Description " + description + " date " + date + " petId " + petId;
        }
    }

    public class Speciality
    {
        public int id { get; set; }
        public string name { get; set; }
        public override string ToString()
        {
            return "id: " + id + " name " + name;
        }
    }



    /* Methods */
    //Pregunta 1
    public async Task Request1()
    {
        try
        {
            using HttpResponseMessage response = await client.GetAsync(Url + "vets");
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    // Pregunta 2
    public async Task Request2()
    {
        try
        {
            using HttpResponseMessage response = await client.GetAsync(Url + "vets");
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                List<Vet> vets = JsonSerializer.Deserialize<List<Vet>>(jsonString);
                Console.WriteLine(vets.Count);

            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    // Pregunta 3, 4 y 5
    public async Task Request3()
    {
        try
        {
            using HttpResponseMessage response = await client.GetAsync(Url + "visits");
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                List<Visit> visits = JsonSerializer.Deserialize<List<Visit>>(jsonString);
                int count = 0;
                foreach (Visit v in visits)
                {
                    Console.WriteLine("Visita " + count++ + " -> " + v);
                }

            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    // Pregunta 6
    private async Task<Uri> CreateSpecialityAsync(Speciality speciality)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("specialties", speciality);
        response.EnsureSuccessStatusCode();
        return response.Headers.Location;
    }

    public async Task Request4()
    {
        try
        {
            client.BaseAddress = new Uri(Url);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            Speciality speciality = new Speciality()
            {
                id = 100,
                name = "MySpec100"
            };
            var url = await CreateSpecialityAsync(speciality);
            Console.WriteLine($"Created at {url}");

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.ToString());
        }
    }

    private async Task<Speciality> CreateSpecialityAsyncBody(Speciality speciality)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync(
                "specialties", speciality);
        response.EnsureSuccessStatusCode();
        string jsonContent = response.Content.ReadAsStringAsync().Result;
        Speciality dev = JsonSerializer.Deserialize<Speciality>(jsonContent);
        return dev;
    }

    private async Task<List<Speciality>> GetSpecialitiesAsync()
    {
        HttpResponseMessage response = await client.GetAsync(
                "specialties");
        response.EnsureSuccessStatusCode();
        string jsonString = await response.Content.ReadAsStringAsync();
        List<Speciality> specialities = JsonSerializer.Deserialize<List<Speciality>>(jsonString);
        return specialities;

    }
    private async Task<Speciality> GetSpecialityAsync(int id)
    {
        HttpResponseMessage response = await client.GetAsync(
                "specialties" + "/" + id);
        response.EnsureSuccessStatusCode();
        string jsonString = await response.Content.ReadAsStringAsync();
        Speciality dev = JsonSerializer.Deserialize<Speciality>(jsonString);
        return dev;
    }

    static Speciality findByName(List<Speciality> specialityList, String name)
    {
        foreach (Speciality s in specialityList)
        {
            if (s.name.Equals(name))
                return s;
        }
        return null;
    }

    // Pregunta 7
    public async Task Request5()
    {
        try
        {
            client.BaseAddress = new Uri(Url);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            Speciality specialityToBeCreated = new Speciality()
            {
                id = 123,
                name = "MySpecialityDemo"
            };
            // Añadir
            Speciality specialityCreated = await CreateSpecialityAsyncBody(specialityToBeCreated);
            Console.WriteLine("specialityCreated -> " + specialityCreated);

            // Buscar todos
            List<Speciality> specialityList = await GetSpecialitiesAsync();
            Console.WriteLine("specialityList size -> " + specialityList.Count);
            Speciality specialityFounded = findByName(specialityList, "MySpecialityDemo");
            Console.WriteLine("specialityFounded -> " + specialityFounded);

            // Buscar por id
            Speciality specialityFoundedIndividual = await GetSpecialityAsync(specialityFounded.id);
            Console.WriteLine("specialityFoundedIndividual -> " + specialityFoundedIndividual);

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.ToString());
        }
    }

    private async Task<Speciality> UpdateSpecialityAsync(Speciality speciality)
    {
        HttpResponseMessage response = await client.PutAsJsonAsync(
                $"specialties/{speciality.id}", speciality);
        response.EnsureSuccessStatusCode();
        string jsonString = await response.Content.ReadAsStringAsync();
        Speciality dev = JsonSerializer.Deserialize<Speciality>(jsonString);
        return dev;
    }

    public async Task Request6()
    {
        try
        {
            client.BaseAddress = new Uri(Url);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            Speciality specialityToBeCreated = new Speciality()
            {
                name = "MySpecialityDemo-Pregunta8"
            };

            // Añadir
            Speciality specialityCreated = await CreateSpecialityAsyncBody(specialityToBeCreated);
            Console.WriteLine("specialityCreated -> " + specialityCreated);

            // Buscar todos
            List<Speciality> specialityList = await GetSpecialitiesAsync();
            Console.WriteLine("specialityList size -> " + specialityList.Count);
            Speciality specialityFounded = findByName(specialityList, "MySpecialityDemo-Pregunta8");
            Console.WriteLine("specialityFounded -> " + specialityFounded);

            // Buscar por id
            Speciality specialityFoundedIndividual = await GetSpecialityAsync(specialityFounded.id);
            Console.WriteLine("specialityFoundedIndividual -> " + specialityFoundedIndividual);

            // Actualizar
            specialityFoundedIndividual.name = "MySpecialityDemo-Changed";
            Speciality specialityChanged = await UpdateSpecialityAsync(specialityFoundedIndividual);
            Console.WriteLine("specialityChanged -> " + specialityChanged);

            // Buscar por id de nuevo
            specialityFoundedIndividual = await GetSpecialityAsync(specialityFounded.id);
            Console.WriteLine("specialityFoundedIndividual tras cambio -> " + specialityFoundedIndividual);

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.ToString());
        }
    }

    private async Task<HttpStatusCode> DeleteSpecialityAsync(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync(
                "specialties/" + id);
        return response.StatusCode;
    }

    public async Task Request7()
    {
        try
        {
            client.BaseAddress = new Uri(Url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            Speciality specialityToBeCreated = new Speciality()
            {
                name = "MySpecialityDemo-Pregunta9"
            };

            // Añadir
            Speciality specialityCreated = await CreateSpecialityAsyncBody(specialityToBeCreated);
            Console.WriteLine("specialityCreated -> " + specialityCreated);

            // Buscar todos
            List<Speciality> specialityList = await GetSpecialitiesAsync();
            Console.WriteLine("specialityList size -> " + specialityList.Count);
            Speciality specialityFounded = findByName(specialityList, "MySpecialityDemo-Pregunta9");
            Console.WriteLine("specialityFounded -> " + specialityFounded);

            // Buscar por id
            Speciality specialityFoundedIndividual = await GetSpecialityAsync(specialityFounded.id);
            Console.WriteLine("specialityFoundedIndividual -> " + specialityFoundedIndividual);

            // Actualizar
            specialityFoundedIndividual.name = "MySpecialityDemo-PRegunta9-Changed";
            Speciality json = await UpdateSpecialityAsync(specialityFoundedIndividual);
            Console.WriteLine("specialityChanged -> " + json);

            // Buscar por id de nuevo
            specialityFoundedIndividual = await GetSpecialityAsync(specialityFounded.id);
            Console.WriteLine("specialityFoundedIndividual tras cambio -> " + specialityFoundedIndividual);

            // Borrar
            var statusCode = await DeleteSpecialityAsync(specialityFounded.id);
            Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");

        }
        catch (Exception e)
        {

            Console.WriteLine(e.Message);
            Console.WriteLine(e.ToString());

        }
    }

}

