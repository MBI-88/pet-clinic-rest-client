using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace pet_clinic_rest_client.src;



internal class Client : IClient
{
    private readonly HttpClient client;

    public Client()
    {
        client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:9966/petclinic/api/");
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    }

    /* Models */
    internal class VetSpeciality
    {
        public int id { get; set; }
        public string? name { get; set; }
    }

    internal class Vet
    {
        public int id { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public List<VetSpeciality>? specialities { get; set; }
    }

    internal class Visit
    {
        public int id { get; set; }
        public string? description { get; set; }
        public int petId { get; set; }
        public DateTime date { get; set; }
        public override string ToString()
        {
            return "id: " + id + " Description " + description + " date " + date + " petId " + petId;
        }
    }

    internal class Speciality
    {
        public int id { get; set; }
        public string? name { get; set; }
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
            using HttpResponseMessage response = await client.GetAsync("vets");
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
            using HttpResponseMessage response = await client.GetAsync("vets");
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                List<Vet> vets = JsonSerializer.Deserialize<List<Vet>>(jsonString)!;
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
            using HttpResponseMessage response = await client.GetAsync("visits");
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                List<Visit> visits = JsonSerializer.Deserialize<List<Visit>>(jsonString)!;
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
        using HttpResponseMessage response = await client.PostAsJsonAsync("specialties", speciality);
        response.EnsureSuccessStatusCode();
        return response.Headers.Location!;
    }

    public async Task Request4()
    {
        try
        {

            Speciality speciality = new Speciality()
            {
                //id = 100,
                name = "MySpec101"
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
        using HttpResponseMessage response = await client.PostAsJsonAsync(
                "specialties", speciality);
        response.EnsureSuccessStatusCode();
        string jsonContent = response.Content.ReadAsStringAsync().Result;
        Speciality dev = JsonSerializer.Deserialize<Speciality>(jsonContent)!;
        return dev;
    }

    private async Task<List<Speciality>> GetSpecialitiesAsync()
    {
        using HttpResponseMessage response = await client.GetAsync(
                "specialties");
        response.EnsureSuccessStatusCode();
        string jsonString = await response.Content.ReadAsStringAsync();
        List<Speciality> specialities = JsonSerializer.Deserialize<List<Speciality>>(jsonString)!;
        return specialities;

    }
    private async Task<Speciality> GetSpecialityAsync(int id)
    {
        using HttpResponseMessage response = await client.GetAsync(
                "specialties/" + id);
        response.EnsureSuccessStatusCode();
        string jsonString = await response.Content.ReadAsStringAsync();
        Speciality dev = JsonSerializer.Deserialize<Speciality>(jsonString)!;
        return dev;
    }

    static Speciality findByName(List<Speciality> specialityList, String name)
    {
        foreach (Speciality s in specialityList)
        {
            if (s.name!.Equals(name))
                return s;
        }
        return new Speciality();
    }

    // Pregunta 7
    public async Task Request5()
    {
        try
        {

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
        using HttpResponseMessage response = await client.PutAsJsonAsync(
                $"specialties/{speciality.id}", speciality);
        response.EnsureSuccessStatusCode();
        string jsonString = await response.Content.ReadAsStringAsync();
        Speciality dev = JsonSerializer.Deserialize<Speciality>(jsonString)!;
        return dev;
    }
   
   // Pregunta 8
    public async Task Request6()
    {
        try
        {

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
        using HttpResponseMessage response = await client.DeleteAsync(
                "specialties/" + id);
        return response.StatusCode;
    }

    // Pregunta 9
    public async Task Request7()
    {
        try
        {
    
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

    // Pregunta 10
    public async Task Request8()
    {
        try
        {
            var clientId = "admin";
            var clientSecret = "admin";

            var auth = $"{clientId}:{clientSecret}";
            var base64Enc = Convert.ToBase64String(Encoding.UTF8.GetBytes(auth));
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + base64Enc);

            // Buscar todos
            List<Speciality> specialityList = await GetSpecialitiesAsync();

            if (specialityList != null)
            {
                Console.WriteLine("specialityList size -> " + specialityList.Count);
                foreach (Speciality s in specialityList)
                {
                    Console.WriteLine("Specialty -> " + s);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.ToString());
        }
    }
}

