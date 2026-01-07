namespace pet_clinic_rest_client.src;

public static class Factory
{
    public static IClient NewClient()
    {
        return new Client();
    }
}