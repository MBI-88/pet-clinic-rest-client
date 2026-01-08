
using pet_clinic_rest_client.src;

class Program
{
    static async Task Main(string[] args)
    {
        IClient client = Factory.NewClient();
        Console.WriteLine("Using Pet-Clinic Rest client");

        if (args.Length > 0)
        {
            switch (args[0])
            {
                case "req1":
                    {
                        await client.Request1();
                        break;
                    }


                case "req2":
                    {   
                        await client.Request2();
                        break;
                    }

                case "req3":
                    {
                        await client.Request3();
                        break;
                    }
                
                case "req4":
                    {
                        await client.Request4();
                        break;
                    }

                case "req5":
                    {
                        await client.Request5();
                        break;
                    }
                
                case "req6":
                    {
                        await client.Request6();
                        break;
                    }

                case "req7":
                    {
                        await client.Request7();
                        break;
                    }
                
                case "req8":
                    {
                        await client.Request8();
                        break;
                    }

                default:
                    {
                        Console.WriteLine("Use req1,req2,reqX for selecting resquest type");
                        break;
                    }

            }

        }
    }
}
