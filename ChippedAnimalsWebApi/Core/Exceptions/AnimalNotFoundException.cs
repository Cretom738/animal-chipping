namespace Core.Exceptions
{
    public class AnimalNotFoundException : NotFoundException
    {
        public AnimalNotFoundException(long? animalId)
            : base($"Animal with id {animalId} not found")
        {
        }
    }
}
