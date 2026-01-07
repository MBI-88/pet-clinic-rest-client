# 🐾 Pet-Clinic REST Client (.NET)

![.NET Logo](https://upload.wikimedia.org/wikipedia/commons/7/7d/Microsoft_.NET_logo.svg)

Este proyecto implementa un **cliente en .NET** para interactuar con la API REST de **Spring PetClinic**.  
Permite realizar consultas automatizadas y operaciones CRUD sobre recursos como **Specialties**, **Owners**, **Pets**, etc.

---

## 📂 Estructura del proyecto

```bash
pet-clinic-rest-client
│   .gitignore
│   LICENSE
│   pet-clinic-rest-client.csproj
│   Program.cs
│   README.md
│
├───bin
│   └───Debug
│       └───net10.0
│               pet-clinic-rest-client.deps.json
│               pet-clinic-rest-client.dll
│               pet-clinic-rest-client.exe
│               pet-clinic-rest-client.pdb
│               pet-clinic-rest-client.runtimeconfig.json
│
├───obj
│   │   pet-clinic-rest-client.csproj.nuget.dgspec.json
│   │   pet-clinic-rest-client.csproj.nuget.g.props
│   │   pet-clinic-rest-client.csproj.nuget.g.targets
│   │   project.assets.json
│   │   project.nuget.cache
│   │
│   └───Debug
│       └───net10.0
│           │   .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
│           │   apphost.exe
│           │   pet-clinic-rest-client.AssemblyInfo.cs
│           │   pet-clinic-rest-client.AssemblyInfoInputs.cache
│           │   pet-clinic-rest-client.assets.cache
│           │   pet-clinic-rest-client.csproj.CoreCompileInputs.cache
│           │   pet-clinic-rest-client.csproj.FileListAbsolute.txt
│           │   pet-clinic-rest-client.dll
│           │   pet-clinic-rest-client.GeneratedMSBuildEditorConfig.editorconfig
│           │   pet-clinic-rest-client.genruntimeconfig.cache
│           │   pet-clinic-rest-client.GlobalUsings.g.cs
│           │   pet-clinic-rest-client.pdb
│           │   pet-clinic-rest-client.sourcelink.json
│           │
│           ├───ref
│           │       pet-clinic-rest-client.dll
│           │
│           └───refint
│                   pet-clinic-rest-client.dll
│
└───src
        Client.cs
        Factory.cs
        IClient.cs
```

```bash
Speciality speciality = new Speciality()
{
    name = "MySpec100"
};

var url = await CreateSpecialityAsync(speciality);
Console.WriteLine($"Created at {url}");

List<Speciality> specialityList = await GetSpecialitiesAsync();
Console.WriteLine("specialityList size -> " + specialityList.Count);

Speciality specialityFounded = findByName(specialityList, "MySpec100");
Console.WriteLine("specialityFounded -> " + specialityFounded);

```

## 🧪 Funcionalidades implementadas

- POST → Crear nuevas especialidades (/api/specialties)
- GET → Listar todas las especialidades (/api/specialties)
- GET by ID → Consultar una especialidad específica (/api/specialties/{id})
- Factory Pattern → Instanciación del cliente
- Switch de comandos → Ejecución de distintas operaciones desde consola

## 📖 Notas

- El servidor ignora el id enviado en el POST y asigna uno nuevo.
- Se recomienda no enviar el campo id al crear entidades.
- El cliente está preparado para extenderse a otros recursos de PetClinic (owners, pets, visits).