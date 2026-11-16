namespace restauration.Models
{
    public enum TableStatus
    {
        Disponible = 0,
        Occupee = 1,
        Reservee = 2
    }

    public enum ReservationStatus
    {
        EnAttente = 0,
        Confirmee = 1,
        Annulee = 2,
        Terminee = 3
    }

    public enum OrderStatus
    {
        EnAttente = 0,
        EnPreparation = 1,
        Prete = 2,
        Servie = 3,
        Payee = 4,
        Annulee = 5
    }

    public enum OrderType
    {
        SurPlace = 0,
        AEmporter = 1,
        Livraison = 2
    }
}

